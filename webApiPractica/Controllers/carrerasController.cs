using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public carrerasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllCarreras")]
        public IActionResult Get()
        {
            var listadoCarreras = (from c in _equiposContext.carreras
                                   join f in _equiposContext.facultades
                                   on c.facultad_id equals f.facultad_id
                                   select new
                                   {
                                       c.carrera_id,
                                       c.nombre_carrera,
                                       f.nombre_facultad
                                   }).ToList();


            if (listadoCarreras.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoCarreras);
        }

        [HttpPost]
        [Route("AddCarrera")]
        public IActionResult CreatMotorista([FromBody] carreras carrera)
        {
            try
            {
                _equiposContext.carreras.Add(carrera);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updateCarrera/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] carreras carreraModificar)
        {
            carreras? carreraActual = (from e in _equiposContext.carreras
                                           where e.carrera_id == id
                                           select e).FirstOrDefault();
            if (carreraActual == null)
            {
                return NotFound();
            }

            carreraActual.nombre_carrera = carreraModificar.nombre_carrera;
            carreraActual.facultad_id = carreraModificar.facultad_id;


            _equiposContext.Entry(carreraActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(carreraModificar);

        }

        [HttpDelete]
        [Route("eliminarCarrera/{id}")]
        public IActionResult Eliminarcarrera(int id)
        {
            carreras? carrera = (from e in _equiposContext.carreras
                                     where e.carrera_id == id
                                     select e).FirstOrDefault();
            if (carrera == null)
                return NotFound();

            _equiposContext.carreras.Attach(carrera);
            _equiposContext.carreras.Remove(carrera);
            _equiposContext.SaveChanges();

            return Ok(carrera);
        }
    }
}
