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
    public class LectorController :ControllerBase
    {
        #region Propiedades
        private readonly INotificacionNegocio _notificacionesNegocio;
        #endregion

        #region Constructor
        public LectorController(ILogger<NotificacionNegocio> logger, DbContexto contexto)
        {
            _notificacionesNegocio = new NotificacionNegocio(logger, contexto);
        }
        #endregion

        #region Endpoints
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
            return await _notificacionesNegocio.SuscribeDesuscribe(lector, false);
        }
        #endregion
    }
}