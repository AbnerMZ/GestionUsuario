using GestionUsuarios.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsuarios.Controllers
{
    //[Authorize(Roles ="Administrador")]
    public class AdministracionController : Controller
    {
        private readonly RoleManager<IdentityRole> gestionRoles;
        private readonly UserManager<IdentityUser> gestionUsuarios;

        public AdministracionController(RoleManager<IdentityRole> gestionRoles,UserManager<IdentityUser> gestionUsuarios)
        {
            this.gestionRoles = gestionRoles;
            this.gestionUsuarios = gestionUsuarios;
        }

        [HttpGet]
        [Route("Administracion/CrearRol")]
        public IActionResult CrearRol()
        {
            return View();
        }

        [HttpPost]
        [Route("Administracion/CrearRol")]
        public async Task<IActionResult> CrearRol(CrearRolViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = modelo.NombreRol
                };

                IdentityResult result = await gestionRoles.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles","Administracion");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(modelo);
            
        }

        [HttpGet]
        [Route("Administracion/Roles")]
        public IActionResult listaRoles()
        {
            var roles = gestionRoles.Roles;
            return View(roles);
        }


        [HttpGet]
        [Route("Administracion/EditarRol")]
        public async Task<IActionResult> EditarRol(string id)
        {
            var rol = await gestionRoles.FindByIdAsync(id);

            if (rol == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id = {id} no fue encontrado";
                return View("Error");
            }

            var model = new EditarRolViewModel
            {
                Id = rol.Id,
                RolNombre = rol.Name
            };

            foreach (var usuario in gestionUsuarios.Users)
            {
                if(await gestionUsuarios.IsInRoleAsync(usuario, rol.Name))
                {
                    model.Usuarios.Add(usuario.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Route("Administracion/EditarRol")]
        public async Task<IActionResult>EditRole(EditarRolViewModel model)
        {
            var rol = await gestionRoles.FindByIdAsync(model.Id);
            if (rol == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id = {model.Id} no fue encontrado";
                return View("Error");
            }
            else
            {
                rol.Name = model.RolNombre;

                var resultado = await gestionRoles.UpdateAsync(rol);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListaRoles");
                }
                foreach(var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

        }

        [HttpGet]
        [Route("Administracion/EditarUsuarioRol")]
        public async Task<IActionResult> EditarUsuarioRol(string rolId)
        {
            ViewBag.roleId = rolId;

            var role = await gestionRoles.FindByIdAsync(rolId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id = {rolId} no fue encontrado";
                return View("Error");
            }

            var model = new List<UsuarioRolModelo>();

            foreach (var user in gestionUsuarios.Users)
            {
                var usuarioRolModelo = new UsuarioRolModelo
                {
                    UsuarioId = user.Id,
                    UsuarioNombre = user.UserName
                };

                if (await gestionUsuarios.IsInRoleAsync(user, role.Name))
                {
                    usuarioRolModelo.EstaSeleccionado = true;
                }
                else
                {
                    usuarioRolModelo.EstaSeleccionado = false;
                }

                model.Add(usuarioRolModelo);
            }

            return View(model);

        }

        [HttpPost]
        [Route("Administracion/EditarUsuarioRol")]
        public async Task<IActionResult> EditarUsuarioRol(List<UsuarioRolModelo> model,string rolId)
        {
            var rol =await gestionRoles.FindByIdAsync(rolId);

            if (rol == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id = {rolId} no fue encontrado";
                return View("Error");
            }

            for(int i = 0; i < model.Count; i++)
            {
                var user = await gestionUsuarios.FindByIdAsync(model[i].UsuarioId);

                IdentityResult result = null;

                if (model[i].EstaSeleccionado && !(await gestionUsuarios.IsInRoleAsync(user, rol.Name)))
                {
                    result = await gestionUsuarios.AddToRoleAsync(user, rol.Name);
                }
                else if (!model[i].EstaSeleccionado && await gestionUsuarios.IsInRoleAsync(user, rol.Name))
                {
                    result= await gestionUsuarios.RemoveFromRoleAsync(user, rol.Name);  
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditarRol", new { Id = rolId });

                }

            }
            return RedirectToAction("EditarRol",new { Id = rolId });
        }

    }
}
