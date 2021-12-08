using Microsoft.AspNetCore.Mvc;
using NotificacionesDatos;
using NotificacionesDatos.Entitades;
using NotificacionesNegocio;

namespace NotificacionesServicio.Controllers
{
    /// <summary>
    /// Controller del Publicador
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PublicadorController :ControllerBase
    {
        #region Propiedades
        private readonly INotificacionNegocio _notificacionesNegocio;
        #endregion

        #region Constructor
        public PublicadorController(ILogger<NotificacionNegocio> logger, DbContexto contexto)
        {
            _notificacionesNegocio = new NotificacionNegocio(logger, contexto);
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
            _notificacionesNegocio.EnviarMensaje(mensaje);
        }

        /// <summary>
        /// Obtiene los lectores suscritos en ese instante.
        /// </summary>
        /// <returns>Los lectores suscritos.</returns>
        [HttpGet]
        [Route("lectoressuscritos")]
        public async Task<List<Lector>> ObtenerLectoresSuscritos()
        {
            return await _notificacionesNegocio.ObtenerLectoresSuscritos();
        }
        #endregion
    }
}