using System.ComponentModel.DataAnnotations;

namespace GestionUsuarios.ViewsModels
{
    public class LoginViewModelo
    {
        [Required(ErrorMessage ="Email obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Recuerdame")]
        public bool Recuerdame { get; set; }
    }
}
