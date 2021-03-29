using apiPlenitude.Context;
using apiPlenitude.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiPlenitude.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext context;
        public UsuarioController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Usuarios1> Get()
        {
            var users = context.Usuario;
            return users.ToList();
        }

        [HttpGet("{id}")]
        public Usuarios1 Get(int id)
        {
            var usuario = context.Usuario.FirstOrDefault(p => p.Id_Usr == id);
            return usuario;
        }

    }


}
