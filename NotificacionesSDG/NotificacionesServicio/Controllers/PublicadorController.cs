using Microsoft.AspNetCore.Mvc;
using NotificacionesNegocio;

namespace NotificacionesServicio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicadorController : ControllerBase
    {
        #region Propiedades
        private readonly ILogger<PublicadorController> _logger;
        private readonly INotificacionNegocio _notificacionesNegocio;
        #endregion

        #region Constructor
        public PublicadorController(ILogger<PublicadorController> logger)
        {
            _logger = logger;
            _notificacionesNegocio = new NotificacionNegocio();
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Envía un mensaje a todos lectores suscritos.
        /// </summary>
        /// <param name="mensaje">El mensaje que se envía.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Enviar")]
        public void EnviarMensaje([FromBody] string mensaje)
        {
            try
            {
                _notificacionesNegocio.EnviarMensaje(mensaje);
            }
            catch (Exception e)
            {
                throw new Exception("Ha ocurrido un error durante el envío del mensaje.", e);
            }
        }

        [HttpGet]
        [Route("LectoresSuscritos")]
        public List<Lector> ObtenerLectoresSuscritos()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}