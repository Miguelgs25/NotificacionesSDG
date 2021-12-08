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
            try
            {
                _logger.LogInformation("El Publicador está enviando el mensaje: '{mensaje}'.", mensaje);
                byte[] body = Encoding.UTF8.GetBytes(mensaje);
                canal.BasicPublish(EXCHANGE, String.Empty, null, body);
            }
            catch(Exception e)
            {
                _logger.LogError("Ha ocurrido un error durante el envío del mensaje: '{mensaje}'.", mensaje);
                throw new Exception("Ha ocurrido un error durante el envío del mensaje.", e);
            }
        }

        public async Task<List<Lector>> ObtenerLectoresSuscritos()
        {
            _logger.LogInformation("Publicador está obteniendo los lectores suscritos.");
            return await _contexto.Lectores.ToListAsync();
        }

        public async Task<bool> SuscribeDesuscribe(Lector lector, bool suscribirse)
        {
            bool res = true;

            try
            {
                if(string.IsNullOrEmpty(lector.Nombre))
                {
                    _logger.LogWarning("El nombre del lector no puede ser vacío o nulo.");
                    res = false;
                }
                else
                {
                    Lector? lectorExistente = await _contexto.Lectores
                        .Where(l => l.Nombre == lector.Nombre)
                        .FirstOrDefaultAsync();

                    if(suscribirse && lectorExistente != null)
                    {
                        _logger.LogWarning("El lector no puede suscribirse porque ya existe otro con el mismo nombre.");
                        res = false;
                    }
                    else if(!suscribirse && lectorExistente == null)
                    {
                        _logger.LogWarning("El lector no puede desuscribirse porque no está suscrito.");
                        res = false;
                    }
                    else if(suscribirse && lectorExistente == null)
                    {
                        _logger.LogInformation("Lector {lector.Nombre} se ha suscrito.", lector.Nombre);
                        _contexto.Lectores.Add(lector);
                    }
                    else if(!suscribirse && lectorExistente != null)
                    {
                        _logger.LogInformation("Lector {lectorExistente.Nombre} se ha desuscrito.", lectorExistente.Nombre);
                        _contexto.Lectores.Remove(lectorExistente);
                    }
                    _contexto.SaveChanges();
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Ha ocurrido un error durante la operacion de suscripción-desuscripción.");
                throw new Exception("Ha ocurrido un error durante la operacion de suscripción-desuscripción.", e);
            }

            return res;
        }
        #endregion
    }
}