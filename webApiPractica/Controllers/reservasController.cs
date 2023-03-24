using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public reservasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllReservas")]
        public IActionResult Get()
        {
            var listadoReservas = (from r in _equiposContext.reservas
                                   join e in _equiposContext.equipos on r.equipo_id equals e.id_equipos
                                   join u in _equiposContext.usuarios on r.usuario_id equals u.usuario_id
                                   join re in _equiposContext.estados_reserva on r.estado_reserva_id equals re.estado_res_id
                                   select new
                                   {
                                       r.reserva_id,
                                       nombreEquipo=e.nombre,
                                       nombreUsuario=u.nombre,
                                       r.hora_salida,
                                       r.hora_retorno,
                                       r.tiempo_reserva,
                                       re.estado

                                   }).ToList();

            if (listadoReservas.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoReservas);
        }

        [HttpPost]
        [Route("AddReserva")]
        public IActionResult CreatMotorista([FromBody] reservas reserva)
        {
            try
            {
                _equiposContext.reservas.Add(reserva);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updatereserva/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] reservas reservaModificar)
        {
            reservas? reservaActual = (from e in _equiposContext.reservas
                                       where e.reserva_id == id
                                       select e).FirstOrDefault();
            if (reservaActual == null)
            {
                return NotFound();
            }

            reservaActual.equipo_id = reservaModificar.equipo_id;
            reservaActual.usuario_id = reservaModificar.usuario_id;
            reservaActual.fecha_salida = reservaModificar.fecha_salida;
            reservaActual.hora_salida = reservaModificar.hora_salida;
            reservaActual.tiempo_reserva = reservaModificar.tiempo_reserva;
            reservaActual.estado_reserva_id = reservaModificar.estado_reserva_id;
            reservaActual.fecha_retorno = reservaActual.fecha_retorno;
            reservaActual.hora_retorno = reservaActual.hora_retorno;


            _equiposContext.Entry(reservaActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(reservaModificar);

        }

        [HttpDelete]
        [Route("eliminarReserva/{id}")]
        public IActionResult Eliminarreserva(int id)
        {
            reservas? reserva = (from e in _equiposContext.reservas
                                 where e.reserva_id == id
                                 select e).FirstOrDefault();
            if (reserva == null)
                return NotFound();

            _equiposContext.reservas.Attach(reserva);
            _equiposContext.reservas.Remove(reserva);
            _equiposContext.SaveChanges();

            return Ok(reserva);
        }
    }
}
