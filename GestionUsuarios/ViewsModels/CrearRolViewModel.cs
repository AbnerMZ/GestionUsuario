using System.ComponentModel.DataAnnotations;

namespace GestionUsuarios.ViewsModels
{
    public class CrearRolViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name ="Rol")]
        public string NombreRol { get; set; }


    }
}
