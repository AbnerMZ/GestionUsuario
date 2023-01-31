using System.ComponentModel.DataAnnotations;

namespace GestionUsuarios.ViewsModels
{
    public class RegistroModelo
    {
        [Required(ErrorMessage ="El Email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name ="Repetir Contraseña")]
        [Compare("Password",ErrorMessage ="Las contraseñas no coinciden")]
        public string ValidarPassword { get; set; }


    }
}
