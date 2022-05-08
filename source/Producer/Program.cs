using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "letterbox", 
    durable: false, 
    exclusive: false, 
    autoDelete: false, 
    arguments: null);

Person person = new Person() { 
    Id = 3,
    Name = "Kevin Kallin",
    Address = "Göteborgsvägen 130"
};

// Serialize object to JSON
string serialize = JsonSerializer.Serialize(person);


// Encode Json to byte[]
var encodedMessage = Encoding.UTF8.GetBytes(serialize);

channel.BasicPublish("", "letterbox", null, encodedMessage);

System.Console.WriteLine($"Published message: {serialize}");


public class Person {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
};