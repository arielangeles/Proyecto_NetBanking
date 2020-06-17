using InternetBankingFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using log4net;
using System.Web.Security;

namespace InternetBankingFinal.Controllers
{
    
    public class UserController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: User
        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        // POST: User/Registro
        [HttpPost]
        public ActionResult Registro(User user)
        {
            bool Status = false;
            string message = "";

            // Model Validation 
            if (ModelState.IsValid)
            {

                #region Si el correo ya existe 
                var Existe = CorreoExist(user.Correo);
                if (Existe)
                {
                    message = "Este correo ya existe";
                    ModelState.AddModelError("CorreoExiste", message);
                    log.Error(message);
                    return View(user);
                }
                #endregion

                #region Generate Activation Code 
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion

                user.IsEmailVerified = false;

                #region Save to Database 

                try
                {
                           
                    using (BankingEF db = new BankingEF())
                    {
                        db.User.Add(user);
                        db.SaveChanges();

                        //Send Email to User
                        SendVerificationLinkEmail(user.Correo, user.ActivationCode.ToString());
                        message = $"\tEl registro ha sido realizado con exito. El link de activacion ha sido enviado a tu correo : {user.Correo} ";
                        log.Info(message);

                        Status = true;
                    }
                    

                }
                catch (Exception ex)
                {
                    message = "Error al agregar usuario - ";
                    ModelState.AddModelError("", message + ex.Message);
                    log.Error(message + ex.Message);
                }

                #endregion

            }
            else
            {
                message = "\tSolicitud invalida";
                log.Error(message);

            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        [HttpGet]
        public ActionResult VerificarCuenta(string id)
        {
            bool Status = false;
            using (BankingEF db = new BankingEF())
            {
                db.Configuration.ValidateOnSaveEnabled = false;

                var v = db.User.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Solicitud invalida";
                }

            }
            ViewBag.Status = Status;
            return View();
        }

        // Login 
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        // Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (BankingEF db = new BankingEF())
            {
                var v = db.User.Where(a => a.Correo == login.Correo).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Porfavor, verifique su correo primero";
                        return View();
                    }
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 año
                        var ticket = new FormsAuthenticationTicket(login.Correo, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);                          
                        }
                        else
                        {
                            return RedirectToAction("NetBank", "Home");
                        }
                        
                    }
                    else
                    {
                        message = "Credencial no valida";
                        log.Error(message);
                    }

                }
                else
                {
                    message = "Credencial no valida";
                    log.Error(message);

                }

            }

            ViewBag.Message = message;
            return View();
        }

        //Logout 
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
            
        }

        [NonAction]
        public bool CorreoExist(string correo)
        {
            using (BankingEF db = new BankingEF())
            {
                var v = db.User.Where(a => a.Correo == correo).FirstOrDefault();
                return v != null;

            }

        }
        [NonAction]
        public void SendVerificationLinkEmail(string correo, string activationCode)
        {
            var verifyUrl = "/User/VerificarCuenta/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("bankintec@gmail.com", "Bank Intec");
            var toEmail = new MailAddress(correo);
            var fromEmailPassword = "123angeles";
            string subject = "Tu cuenta ha sido creada con exito!";

            string body = "<br/><br/>Estamos emocionados de decirte que tu cuenta de BankIntec" +
            " ha sido creada de manera exitosa. Para finalizar, haz click en el link de abajo para verificar tu cuenta" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}
