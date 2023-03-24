using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public estados_equipoController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllEstadosEquipos")]
        public IActionResult Get()
        {
            List<estados_equipo> listadoEstadosEquipos = (from e in _equiposContext.estados_equipo
                                              select e).ToList();
            if (listadoEstadosEquipos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEstadosEquipos);
        }

        [HttpPost]
        [Route("AddEstadoEquipo")]
        public IActionResult CreatMotorista([FromBody] estados_equipo estadoEquipo)
        {
            try
            {
                _equiposContext.estados_equipo.Add(estadoEquipo);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updateEstadoEquipo/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] estados_equipo estadoEquipoModificar)
        {
            estados_equipo? estadoEquipoActual = (from e in _equiposContext.estados_equipo
                                       where e.id_estados_equipo == id
                                       select e).FirstOrDefault();
            if (estadoEquipoActual == null)
            {
                return NotFound();
            }

            estadoEquipoActual.descripcion = estadoEquipoModificar.descripcion;
            estadoEquipoActual.estado = estadoEquipoModificar.estado;


            _equiposContext.Entry(estadoEquipoActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(estadoEquipoModificar);

        }

        [HttpDelete]
        [Route("eliminarEstadoEquipo/{id}")]
        public IActionResult EliminarEstadoEquipo(int id)
        {
            estados_equipo? estadoEquipo = (from e in _equiposContext.estados_equipo
                                 where e.id_estados_equipo == id
                                 select e).FirstOrDefault();
            if (estadoEquipo == null)
                return NotFound();

            _equiposContext.estados_equipo.Attach(estadoEquipo);
            _equiposContext.estados_equipo.Remove(estadoEquipo);
            _equiposContext.SaveChanges();

            return Ok(estadoEquipo);
        }
    }
}
