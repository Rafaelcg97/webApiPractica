using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public equiposController(equiposContext equiposContext) 
        {
            _equiposContext= equiposContext;


        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<equipos> listadoEquipo = (from e in _equiposContext.equipos
                                           select e).ToList();
            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            equipos? equipo = (from e in _equiposContext.equipos
                                           where e.id_equipos == id
                                           select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

    }
}
