using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsuarios.Controllers
{
    public class AdministracionController : Controller
    {
        private readonly RoleManager<IdentityRole> gestionRoles;

        public AdministracionController(RoleManager<IdentityRole> gestionRoles)
        {
            this.gestionRoles = gestionRoles;
        }

        [HttpGet]
        [Route("Administracion/CrearRol")]
        public IActionResult CrearRol()
        {
            return View();
        }
    }
}
