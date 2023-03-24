using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public tipo_equipoController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllTipoEquipos")]
        public IActionResult Get()
        {
            List<tipo_equipo> listadoTipoEquipos = (from e in _equiposContext.tipos_equipo
                                              select e).ToList();
            if (listadoTipoEquipos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoTipoEquipos);
        }

        [HttpPost]
        [Route("AddTipoEquipo")]
        public IActionResult CreatMotorista([FromBody] tipo_equipo tipoEquipo)
        {
            try
            {
                _equiposContext.tipos_equipo.Add(tipoEquipo);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updateTipoEquipo/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] tipo_equipo tipoEquipoModificar)
        {
            tipo_equipo? tipoEquipoActual = (from e in _equiposContext.tipos_equipo
                                       where e.id_tipo_equipo == id
                                       select e).FirstOrDefault();
            if (tipoEquipoActual == null)
            {
                return NotFound();
            }

            tipoEquipoActual.descripcion = tipoEquipoModificar.descripcion;
            tipoEquipoActual.estado = tipoEquipoModificar.estado;


            _equiposContext.Entry(tipoEquipoActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(tipoEquipoModificar);

        }

        [HttpDelete]
        [Route("eliminarTipoEquipo/{id}")]
        public IActionResult EliminartipoEquipo(int id)
        {
            tipo_equipo? tipoEquipo = (from e in _equiposContext.tipos_equipo
                                 where e.id_tipo_equipo == id
                                 select e).FirstOrDefault();
            if (tipoEquipo == null)
                return NotFound();

            _equiposContext.tipos_equipo.Attach(tipoEquipo);
            _equiposContext.tipos_equipo.Remove(tipoEquipo);
            _equiposContext.SaveChanges();

            return Ok(tipoEquipo);
        }
    }
}
