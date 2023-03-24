using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public marcasController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        [HttpGet]
        [Route("GetAllMarcas")]
        public IActionResult Get()
        {
            List<marcas> listadoMarcas = (from e in _equiposContext.marcas
                                              select e).ToList();
            if (listadoMarcas.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoMarcas);
        }

        [HttpPost]
        [Route("AddMarca")]
        public IActionResult CreatMotorista([FromBody] marcas marca)
        {
            try
            {
                _equiposContext.marcas.Add(marca);
                _equiposContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("updatemarca/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] marcas marcaModificar)
        {
            marcas? marcaActual = (from e in _equiposContext.marcas
                                       where e.id_marcas == id
                                       select e).FirstOrDefault();
            if (marcaActual == null)
            {
                return NotFound();
            }

            marcaActual.nombre_marca = marcaModificar.nombre_marca;
            marcaActual.estados = marcaModificar.estados;


            _equiposContext.Entry(marcaActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();

            return Ok(marcaModificar);

        }

        [HttpDelete]
        [Route("eliminarmarca/{id}")]
        public IActionResult Eliminarmarca(int id)
        {
            marcas? marca = (from e in _equiposContext.marcas
                                 where e.id_marcas == id
                                 select e).FirstOrDefault();
            if (marca == null)
                return NotFound();

            _equiposContext.marcas.Attach(marca);
            _equiposContext.marcas.Remove(marca);
            _equiposContext.SaveChanges();

            return Ok(marca);
        }
    }
}
