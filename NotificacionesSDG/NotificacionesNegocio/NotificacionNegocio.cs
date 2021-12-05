using RabbitMQ.Client;
using System.Text;

namespace NotificacionesNegocio
{
    public class NotificacionNegocio :INotificacionNegocio
    {
        #region Propiedades
        private ConnectionFactory factoria;
        private IConnection conexion;
        private IModel canal;
        #endregion

        #region Constructores
        public NotificacionNegocio()
        {
            factoria = new ConnectionFactory() { HostName = "localhost" };
            conexion = factoria.CreateConnection();
            canal = conexion.CreateModel();
            canal.ExchangeDeclare(exchange: "ex", type: ExchangeType.Fanout);
        }
        #endregion

        #region Métodos
        public void EnviarMensaje(string mensaje)
        {
            byte[] body = Encoding.UTF8.GetBytes(mensaje);
            canal.BasicPublish("ex", "", null, body);
        }
        #endregion
    }
}