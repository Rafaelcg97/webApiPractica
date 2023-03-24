using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public usuariosController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllUsuarios")]
        public IActionResult Get()
        {
            var listadoUsuarios = (from u in _equiposContext.usuarios
                                   join c in _equiposContext.carreras
                                   on u.carrera_id equals c.carrera_id
                                   select new
                                   {
                                       u.usuario_id,
                                       u.nombre,
                                       u.documento,
                                       u.tipo,
                                       u.carnet,
                                       c.nombre_carrera
                                   }).ToList();
            if (listadoUsuarios.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoUsuarios);

        }

        [HttpPost]
        [Route("AddUsuarios")]
        public IActionResult CreatMotorista([FromBody] usuarios usuario)
        {
            try
            {
                _equiposContext.usuarios.Add(usuario);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updateUsuario/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] usuarios usuarioModificar)
        {
            usuarios? usuarioActual = (from e in _equiposContext.usuarios
                                       where e.usuario_id == id
                                       select e).FirstOrDefault();
            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.documento = usuarioModificar.documento;
            usuarioActual.tipo = usuarioModificar.tipo;
            usuarioActual.carnet = usuarioModificar.carnet;
            usuarioActual.carrera_id = usuarioModificar.carrera_id;


            _equiposContext.Entry(usuarioActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(usuarioModificar);

        }

        [HttpDelete]
        [Route("eliminarUsuario/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            usuarios? usuario = (from e in _equiposContext.usuarios
                                 where e.usuario_id == id
                                 select e).FirstOrDefault();
            if (usuario == null)
                return NotFound();

            _equiposContext.usuarios.Attach(usuario);
            _equiposContext.usuarios.Remove(usuario);
            _equiposContext.SaveChanges();

            return Ok(usuario);
        }
    }
}
