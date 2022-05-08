using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

List<Person> persons = new List<Person>();

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox", 
    durable: false, 
    exclusive: false, 
    autoDelete: false, 
    arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var deSerialized = JsonSerializer.Deserialize<Person>(message);

    System.Console.WriteLine($"Person recieved {deSerialized}");
    persons.Add(deSerialized);
};

channel.BasicConsume(queue: "letterbox", autoAck: true, consumer: consumer);


Console.ReadKey();


public class Person {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

};