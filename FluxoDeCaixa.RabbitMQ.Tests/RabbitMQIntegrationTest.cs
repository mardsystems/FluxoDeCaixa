using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Xunit;

namespace FluxoDeCaixa
{
    public abstract class RabbitMQIntegrationTest
    {
        protected readonly IConfiguration configuration;

        protected readonly IServiceCollection services;

        protected readonly ConnectionFactory connectionFactory;

        public RabbitMQIntegrationTest()
        {
            configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            services = new ServiceCollection();

            connectionFactory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ_HostName"]
            };
        }

        public abstract void Act();

        public void ActAndPublish<T>(T message, string queueName)
        {
            Act();

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var content = JsonConvert.SerializeObject(message);

                    var body = Encoding.UTF8.GetBytes(content);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueName,
                        basicProperties: null,
                        body: body
                    );
                }
            }            
        }

        public T ActAndReceive<T>(string queueName)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    //channel.QueuePurge(queueName);

                    //

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume(
                        queue: queueName,
                        autoAck: false,
                        consumer: consumer
                    );

                    var @event = Assert.Raises<BasicDeliverEventArgs>(
                        a => consumer.Received += a,
                        a => consumer.Received -= a,
                        Act
                    );

                    var args = @event.Arguments;

                    var body = args.Body.ToArray();

                    var content = Encoding.UTF8.GetString(body);

                    var message = JsonConvert.DeserializeObject<T>(content);

                    channel.BasicAck(args.DeliveryTag, false);

                    return message;
                }
            }
        }
    }
}
