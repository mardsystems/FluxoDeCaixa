using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Xunit;

namespace FluxoDeCaixa
{
    public abstract class RabbitMQIntegrationTest
    {
        protected ConnectionFactory connectionFactory;

        public RabbitMQIntegrationTest()
        {

        }

        public abstract void Act();

        public T Act<T>(string CONST_FILA_NOME)
        {
            var connection = connectionFactory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: CONST_FILA_NOME,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            //channel.QueuePurge(CONST_FILA_NOME);

            //

            var consumer = new EventingBasicConsumer(channel);

            //pagamentosConsumer.Received += new EventHandler<BasicDeliverEventArgs>((sender, args) =>
            //{
            //});

            channel.BasicConsume(
                queue: CONST_FILA_NOME,
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

            var comando = JsonConvert.DeserializeObject<T>(content);

            channel.BasicAck(args.DeliveryTag, false);

            return comando;
        }
    }
}
