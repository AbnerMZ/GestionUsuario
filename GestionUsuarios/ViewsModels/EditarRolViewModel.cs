using System.ComponentModel.DataAnnotations;

namespace GestionUsuarios.ViewsModels
{
    public class EditarRolViewModel
    {

        public string Id { get; set; }

        public EditarRolViewModel()
        {
            Usuarios = new List<string>();
        }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        public string RolNombre { get; set; }

        public List<string> Usuarios { get; set; }
    }
}
