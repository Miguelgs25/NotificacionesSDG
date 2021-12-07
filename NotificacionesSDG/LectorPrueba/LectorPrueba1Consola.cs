using Newtonsoft.Json;
using NotificacionesDatos.Entitades;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


ConnectionFactory factoria = new ConnectionFactory() { HostName = "localhost" };
using(IConnection conexion = factoria.CreateConnection())
using(IModel canal = conexion.CreateModel())
{
    string valorLeido;
    Lector lector = new Lector { Nombre = "Lector 1" };
    StringContent data = new StringContent(JsonConvert.SerializeObject(lector), Encoding.UTF8, "application/json");
    string urlSus = "https://localhost:7150/Publicador/suscribirse";
    string urlDesus = "https://localhost:7150/Publicador/desuscribirse";
    HttpClient? client = new HttpClient();
    HttpResponseMessage? response;

    do
    {
        Console.WriteLine("[*] Lector 1. ¿Desea suscribirse? [y,n] y presione enter:");
        Console.WriteLine("[*] Ó [s] y presione enter, para salir:");
        valorLeido = Console.ReadLine();

        if(valorLeido == "y")
        {
            // Registra al lector 1 en la BD del publicador
            response = await client.PostAsync(urlSus, data);

            // Se suscribe a la cola de rabbiMQ
            canal.ExchangeDeclare(exchange: "ex", type: ExchangeType.Fanout);

            string nombreCola = canal.QueueDeclare().QueueName;
            canal.QueueBind(nombreCola, "ex", "");

            Console.WriteLine("[*] Lector 1. Suscrito");
            Console.WriteLine("[*] Lector 1. Esperando mensajes.");

            EventingBasicConsumer? lectorConsumidor = new EventingBasicConsumer(canal);
            lectorConsumidor.Received += (model, ea) =>
            {
                string mensaje = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(" [x] Mensaje entrante: '{0}'", mensaje);
            };

            canal.BasicConsume(nombreCola, true, lectorConsumidor);

            // Se desuscribe a la cola de rabbiMQ
            do
            {
                Console.WriteLine("[*] Lector 1. ¿Desea desuscribirse? [y,n] y presione enter:");
                Console.WriteLine("[*] Ó [s] y presione enter, para salir:");
                valorLeido = Console.ReadLine();
                if(valorLeido == "y")
                {
                    canal.BasicCancel(lectorConsumidor.ConsumerTags.First());
                }
            } while(valorLeido != "y" && valorLeido != "s");
        }

        // Borra al lector 1 en la BD del publicador
        if(valorLeido == "y" || valorLeido == "s")
        {
            response = await client.PostAsync(urlDesus, data);
        }
    } while(valorLeido != "s");
}