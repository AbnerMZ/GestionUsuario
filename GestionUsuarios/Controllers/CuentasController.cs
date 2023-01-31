using GestionUsuarios.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsuarios.Controllers
{
    //[Authorize(Roles ="Administrador")]
    //[Authorize(Roles ="Usuario")]
    public class CuentasController : Controller
    {
        //Administrar y gestionar usuarios
        private readonly UserManager<IdentityUser> gestionUsuarios;
        //Metodos para login
        private readonly SignInManager<IdentityUser> gestionLogin;

        public CuentasController(UserManager<IdentityUser> gestionUsuarios, SignInManager<IdentityUser> gestionLogin)
        {
            this.gestionUsuarios = gestionUsuarios;
            this.gestionLogin = gestionLogin;
        }

        [HttpGet]
        [Route("Cuentas/Registro")]
        [AllowAnonymous]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [Route("Cuentas/Registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroModelo model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var resultado = await gestionUsuarios.CreateAsync(usuario, model.Password);

                if (resultado.Succeeded)
                {
                    await gestionLogin.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);           
        }

        [HttpPost]
        [Route("Cuentas/CerrarSesion")]
        public async Task<IActionResult> CerrarSesion()
        {
            await gestionLogin.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Cuentas/Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Cuentas/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModelo modelo)
        {
            if (ModelState.IsValid)
            {
                var result = await gestionLogin.PasswordSignInAsync(
                    modelo.Email, modelo.Password, modelo.Recuerdame, false);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesion no valido");
            }

            return View(modelo);

        }

    }
}
