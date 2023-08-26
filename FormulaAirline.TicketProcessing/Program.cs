// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Welcome to the ticketing service!");

 var factory = new ConnectionFactory(){
                HostName = "localhost",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

   var conn = factory.CreateConnection();
    using var channel = conn.CreateModel();
    channel.QueueDeclare(queue: "bookings",
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: null
        );
    
    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (ModuleHandle, ea) => // ea is the event arguments
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"Received message: {message}");
    };

    channel.BasicConsume(queue: "bookings",
        autoAck: true,
        consumer: consumer);
    
    // Console.WriteLine("Press any key to exit");

    Console.ReadKey();
