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
        private readonly DbContexto _context;
        private readonly ILogger<NotificacionNegocio> _logger;
        private readonly INotificacionNegocio _notificacionesNegocio;
        #endregion

        #region Constructor
        public PublicadorController(ILogger<NotificacionNegocio> logger, DbContexto contexto)
        {
            _context = contexto;
            _logger = logger;
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
            try
            {
                _notificacionesNegocio.EnviarMensaje(mensaje);
            }
            catch(Exception e)
            {
                throw new Exception("Ha ocurrido un error durante el envío del mensaje.", e);
            }
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

        /// <summary>
        /// Suscribe un lector por nombre.
        /// </summary>
        /// <param name="lector">La info del lector que se suscribe.</param>
        /// <returns>Si la acción se ejecutado correctamente.</returns>
        [HttpPost]
        [Route("suscribirse")]
        public async Task<bool> SuscribirLector([FromBody] Lector lector)
        {
            return await _notificacionesNegocio.SuscribeDesuscribe(lector, true);
        }

        /// <summary>
        /// Desuscribe un lector por nombre.
        /// </summary>
        /// /// <param name="lector">La info del lector que se desuscribe.</param>
        /// <returns>Si la acción se ejecutado correctamente.</returns>
        [HttpPost]
        [Route("desuscribirse")]
        public async Task<bool> DesuscribirLector([FromBody] Lector lector)
        {
            return await _notificacionesNegocio.SuscribeDesuscribe(lector,false);
        }
        #endregion
    }
}