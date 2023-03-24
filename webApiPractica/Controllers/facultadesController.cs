using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public facultadesController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllFacultades")]
        public IActionResult Get()
        {
            List<facultades> listadoFacultades = (from e in _equiposContext.facultades
                                              select e).ToList();
            if (listadoFacultades.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoFacultades);
        }

        [HttpPost]
        [Route("AddFacultad")]
        public IActionResult CreatMotorista([FromBody] facultades facultad)
        {
            try
            {
                _equiposContext.facultades.Add(facultad);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updatefacultad/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] facultades facultadModificar)
        {
            facultades? facultadActual = (from e in _equiposContext.facultades
                                       where e.facultad_id == id
                                       select e).FirstOrDefault();
            if (facultadActual == null)
            {
                return NotFound();
            }

            facultadActual.nombre_facultad = facultadModificar.nombre_facultad;


            _equiposContext.Entry(facultadActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(facultadModificar);

        }

        [HttpDelete]
        [Route("eliminarFacultad/{id}")]
        public IActionResult Eliminarfacultad(int id)
        {
            facultades? facultad = (from e in _equiposContext.facultades
                                 where e.facultad_id == id
                                 select e).FirstOrDefault();
            if (facultad == null)
                return NotFound();

            _equiposContext.facultades.Attach(facultad);
            _equiposContext.facultades.Remove(facultad);
            _equiposContext.SaveChanges();

            return Ok(facultad);
        }
    }
}
