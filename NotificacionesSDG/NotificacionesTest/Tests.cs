using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificacionesDatos;
using NotificacionesDatos.Entitades;
using NotificacionesNegocio;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificacionesTest
{
    public class Tests
    {
        private INotificacionNegocio _notificacionesNegocio;
        private ILogger<NotificacionNegocio> logger;
        private DbContexto contexto;

        [SetUp]
        public void Setup()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();

            logger = factory.CreateLogger<NotificacionNegocio>();

            DbContextOptions<DbContexto> options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(databaseName: "DBTests")
                .Options;
            contexto = new DbContexto(options);

            _notificacionesNegocio = new NotificacionNegocio(logger, contexto);
        }

        [TearDown]
        public void TearDown()
        {
            contexto.Database.EnsureDeleted();
        }

        [Test]
        public async Task ObtenerLectoresSuscritos_Despues1Suscripcion()
        {
            bool resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, true);
            Assert.IsTrue(resSuscripcion);

            List<Lector> lectoresSuscritos = await _notificacionesNegocio.ObtenerLectoresSuscritos();
            Assert.IsNotNull(lectoresSuscritos);
            Assert.IsNotEmpty(lectoresSuscritos);
            Assert.AreEqual(1, lectoresSuscritos.Count);
            Assert.AreEqual("Manolo", lectoresSuscritos.Select(l => l.Nombre).First());
        }

        [Test]
        public async Task ObtenerLectoresSuscritos_Despues1Suscripcion1Desuscripcion()
        {
            bool resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, true);
            Assert.IsTrue(resSuscripcion);
            resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, false);
            Assert.IsTrue(resSuscripcion);

            List<Lector> lectoresSuscritos = await _notificacionesNegocio.ObtenerLectoresSuscritos();
            Assert.IsNotNull(lectoresSuscritos);
            Assert.IsEmpty(lectoresSuscritos);
        }

        [Test]
        public async Task Suscribir2LectorConNombreDiferente()
        {
            bool resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, true);
            Assert.IsTrue(resSuscripcion);
            resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Maria" }, true);
            Assert.IsTrue(resSuscripcion);

            List<Lector> lectoresSuscritos = await _notificacionesNegocio.ObtenerLectoresSuscritos();
            Assert.IsNotNull(lectoresSuscritos);
            Assert.IsNotEmpty(lectoresSuscritos);
            Assert.AreEqual(2, lectoresSuscritos.Count);
        }

        [Test]
        public async Task Suscribir1LectorConNombreExistente()
        {
            bool resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, true);
            Assert.IsTrue(resSuscripcion);
            resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, true);
            Assert.IsFalse(resSuscripcion);

            List<Lector> lectoresSuscritos = await _notificacionesNegocio.ObtenerLectoresSuscritos();
            Assert.IsNotNull(lectoresSuscritos);
            Assert.IsNotEmpty(lectoresSuscritos);
            Assert.AreEqual(1, lectoresSuscritos.Count);
        }

        [Test]
        public async Task Desuscribir1LectorSuscrito()
        {
            bool resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, true);
            Assert.IsTrue(resSuscripcion);
            resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, false);
            Assert.IsTrue(resSuscripcion);
        }

        [Test]
        public async Task Desuscribir1LectorNoSuscrito()
        {
            bool resSuscripcion = await _notificacionesNegocio.SuscribeDesuscribe(new Lector { Nombre = "Manolo" }, false);
            Assert.IsFalse(resSuscripcion);
        }

        [Test]
        public async Task EnviarMensaje()
        {
            // RabbitMQ tiene que estar funcionando para que el test sea válido
            string mensajeEnviado = "mensajeTest";
            string mensajeRecibido = string.Empty;
            ConnectionFactory factoria = new ConnectionFactory() { HostName = "localhost" };
            using(IConnection conexion = factoria.CreateConnection())
            using(IModel canal = conexion.CreateModel())
            {
                canal.ExchangeDeclare(exchange: "ex", type: ExchangeType.Fanout);

                string nombreCola = canal.QueueDeclare().QueueName;
                canal.QueueBind(nombreCola, "ex", "");

                EventingBasicConsumer? lectorConsumidor = new EventingBasicConsumer(canal);
                lectorConsumidor.Received += (model, ea) =>
                {
                    mensajeRecibido = Encoding.UTF8.GetString(ea.Body.ToArray());
                };

                canal.BasicConsume(nombreCola, true, lectorConsumidor);

                Assert.DoesNotThrow(() => _notificacionesNegocio.EnviarMensaje(mensajeEnviado));

                while(mensajeRecibido == string.Empty)
                {
                    // Esperando a recibir el mensaje 
                }

                Assert.AreEqual(mensajeEnviado, mensajeRecibido);
            }
        }
    }
}