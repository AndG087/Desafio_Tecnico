// Agenda.Infrastructure/Messaging/RabbitPublisher.cs
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Agenda.Infrastructure.Messaging;

public class RabbitPublisher : IDisposable
{
    private readonly IConnection _conn;
    private readonly IModel _ch;
    private const string Exchange = "agenda.events";

    public RabbitPublisher(IConfiguration cfg)
    {
        var factory = new ConnectionFactory { Uri = new Uri(cfg["RabbitMQ:ConnectionString"]!) };
        _conn = factory.CreateConnection();
        _ch = _conn.CreateModel();
        _ch.ExchangeDeclare(Exchange, ExchangeType.Topic, durable: true);
    }

    public void Publish(string routingKey, object payload)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        _ch.BasicPublish(Exchange, routingKey, basicProperties: null, body);
    }

    public void Dispose()
    {
        _ch?.Dispose();
        _conn?.Dispose();
    }
}
