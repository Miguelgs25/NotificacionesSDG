using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotificacionesDatos;
using NotificacionesDatos.Entitades;
using RabbitMQ.Client;
using System.Text;

namespace NotificacionesNegocio
{
    public class NotificacionNegocio :INotificacionNegocio
    {
        #region Propiedades
        private readonly string HOST_NAME = "localhost";
        private readonly string EXCHANGE = "ex";
        private ConnectionFactory factoria;
        private IConnection conexion;
        private IModel canal;
        private readonly DbContexto _contexto;
        private readonly ILogger<INotificacionNegocio> _logger;
        #endregion

        #region Constructores
        public NotificacionNegocio(ILogger<NotificacionNegocio> logger, DbContexto contexto)
        {
            _contexto = contexto;
            _logger = logger;
            factoria = new ConnectionFactory() { HostName = HOST_NAME };
            conexion = factoria.CreateConnection();
            canal = conexion.CreateModel();
            canal.ExchangeDeclare(EXCHANGE, ExchangeType.Fanout);
        }
        #endregion

        #region Métodos
        public void EnviarMensaje(string mensaje)
        {
            byte[] body = Encoding.UTF8.GetBytes(mensaje);
            canal.BasicPublish(EXCHANGE, String.Empty, null, body);
        }

        public async Task<List<Lector>> ObtenerLectoresSuscritos()
        {
            return await _contexto.Lectores.ToListAsync();
        }

        public async Task<bool> SuscribeDesuscribe(Lector lector, bool suscribirse)
        {
            bool res = true;

            if(string.IsNullOrEmpty(lector.Nombre))
            {
                res = false;
            }

            Lector? lectorExistente = await _contexto.Lectores
                .Where(l => l.Nombre == lector.Nombre)
                .FirstOrDefaultAsync();

            if(suscribirse && lectorExistente != null
                || !suscribirse && lectorExistente == null)
            {
                res = false;
            }
            else if(suscribirse && lectorExistente == null)
            {
                _contexto.Lectores.Add(lector);
            }
            else if(!suscribirse && lectorExistente != null)
            {
                _contexto.Lectores.Remove(lectorExistente);
            }
            _contexto.SaveChanges();

            return res;
        }
        #endregion
    }
}