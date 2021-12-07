using NotificacionesDatos.Entitades;

namespace NotificacionesNegocio
{
    public interface INotificacionNegocio
    {
        /// <summary>
        /// Envía un mensaje a todos lectores suscritos.
        /// </summary>
        /// <param name="mensaje">El mensaje que se envía.</param>
        void EnviarMensaje(string mensaje);

        /// <summary>
        /// Obtiene los lectores suscritos en ese instante.
        /// </summary>
        /// <returns>Los lectores suscritos.</returns>
        Task<List<Lector>> ObtenerLectoresSuscritos();

        /// <summary>
        /// Suscribe o desuscribe un lector por nombre.
        /// </summary>
        /// <param name="lector">La info del lector.</param>
        /// <param name="subcribirse">True: Suscribe. False: Desuscribe.</param>
        /// <returns>Si la acción se ejecutado correctamente.</returns>
        Task<bool> SuscribeDesuscribe(Lector lector, bool suscribirse);
    }
}
