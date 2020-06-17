using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBankingFinal.Models
{
    public class UserLogin
    {
        [Display(Name = "Correo Electronico:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Correo Electronico requerido")]
        public string Correo { get; set; }

        [Display(Name = "Contraseña: ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recuerdame")]
        public bool RememberMe { get; set; }
    }
}