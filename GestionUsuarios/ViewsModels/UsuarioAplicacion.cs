using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace GestionUsuarios.ViewsModels
{
    public class UsuarioAplicacion:IdentityUser
    {
        public string nombre { get; set; }
    }
}
