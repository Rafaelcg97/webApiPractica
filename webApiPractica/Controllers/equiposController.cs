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

    }
}
