
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace FormulaAirline.API.Services
{
    public class MessageProducer : IMessageProducer
    {
        public void SendingMessage<T>(T message)
        {
            var factory = new ConnectionFactory(){
                HostName = "localhost",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

            var conn = factory.CreateConnection();
            using var channel = conn.CreateModel(); // we are using the using statement to make sure the channel is closed after we are done with it
            channel.QueueDeclare(queue: "bookings",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish(exchange: "",
                routingKey: "bookings",
                basicProperties: null,
                body: body);
        }
    }
}