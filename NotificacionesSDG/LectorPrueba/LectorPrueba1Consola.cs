using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factoria = new ConnectionFactory() { HostName = "localhost" };
using(IConnection conexion = factoria.CreateConnection())
using(IModel canal = conexion.CreateModel())
{
    canal.ExchangeDeclare(exchange: "ex", type: ExchangeType.Fanout);

    string nombreCola = canal.QueueDeclare().QueueName;
    canal.QueueBind(nombreCola, "ex", "");

    Console.WriteLine("[*] Lector 1. Esperando mensajes.");

    EventingBasicConsumer? lector = new EventingBasicConsumer(canal);
    lector.Received += (model, ea) =>
    {
        byte[] body = ea.Body.ToArray();
        string message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Mensaje entrante: {0}", message);
    };

    canal.BasicConsume(nombreCola, true, lector);

    Console.WriteLine(" Presiona [enter] en cualquier momento para salir.");
    Console.ReadLine();
}