namespace NotificacionesNegocio
{
    public interface INotificacionNegocio
    {
        /// <summary>
        /// Envía un mensaje a todos lectores suscritos.
        /// </summary>
        /// <param name="mensaje">El mensaje que se envía.</param>
        void EnviarMensaje(string mensaje);
    }
}
