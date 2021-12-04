using Microsoft.AspNetCore.Mvc;
using NotificacionesNegocio;

namespace NotificacionesServicio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicadorController : ControllerBase
    {
        private readonly ILogger<PublicadorController> _logger;

        public PublicadorController(ILogger<PublicadorController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Enviar")]
        public IActionResult EnviarMensaje([FromBody] string mensaje)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("LectoresSuscritos")]
        public List<Lector> ObtenerLectoresSuscritos()
        {
            throw new NotImplementedException();
        }
    }
}