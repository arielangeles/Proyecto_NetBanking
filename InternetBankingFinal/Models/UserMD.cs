using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetBankingFinal.Models
{
    [MetadataType(typeof(UserMD))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }
    public class UserMD
    {
        [Display(Name = "Nombre:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere un nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere un apellido")]
        public string Apellido { get; set; }

        [Display(Name = "Correo Electronico:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere un correo valido")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Cedula:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere una cedula valida")]
        [MinLength(11, ErrorMessage = "Se requieren 11 caracteres")]
        [MaxLength(12, ErrorMessage = "Porfavor, inserte una cedula valida")]
        public string Cedula { get; set; }

        [Display(Name = "Fecha de nacimiento:")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd/MM/yyyy}")]
        public string DateBirth { get; set; }

        [Display(Name = "Contraseña:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere una contraseña")]
        [MinLength(6, ErrorMessage = "Se requieren minimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña:")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirmar contraseña y contraseña no coinciden")]
        public string ConfirmPassword { get; set; }

    }
}