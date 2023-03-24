using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public estados_reservaController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllEstadosReserva")]
        public IActionResult Get()
        {
            List<estados_reserva> listadoEstadosReserva = (from e in _equiposContext.estados_reserva
                                              select e).ToList();
            if (listadoEstadosReserva.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEstadosReserva);
        }

        [HttpPost]
        [Route("AddEstadoReserva")]
        public IActionResult CreatMotorista([FromBody] estados_reserva estadoReserva)
        {
            try
            {
                _equiposContext.estados_reserva.Add(estadoReserva);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updateEstadoReserva/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] estados_reserva estadoReservaModificar)
        {
            estados_reserva? estadoReservaActual = (from e in _equiposContext.estados_reserva
                                       where e.estado_res_id == id
                                       select e).FirstOrDefault();
            if (estadoReservaActual == null)
            {
                return NotFound();
            }

            estadoReservaActual.estado = estadoReservaModificar.estado;


            _equiposContext.Entry(estadoReservaActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(estadoReservaModificar);

        }

        [HttpDelete]
        [Route("eliminarEstadoReserva/{id}")]
        public IActionResult EliminarEstadoReserca(int id)
        {
            estados_reserva? estadoReserva = (from e in _equiposContext.estados_reserva
                                 where e.estado_res_id == id
                                 select e).FirstOrDefault();
            if (estadoReserva == null)
                return NotFound();

            _equiposContext.estados_reserva.Attach(estadoReserva);
            _equiposContext.estados_reserva.Remove(estadoReserva);
            _equiposContext.SaveChanges();

            return Ok(estadoReserva);
        }
    }
}
