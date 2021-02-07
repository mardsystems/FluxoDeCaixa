using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Threading;
using Xunit;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public abstract class AoProtocolarUmPagamento : RabbitMQIntegrationTest
    {
        private readonly ProtocolamentoDeLancamentosFinanceirosRabbitMQService sut;

        private ComandoDeLancamentoFinanceiro comandoOriginal;

        public const string CONST_FILA_NOME = "pagamentos";

        public AoProtocolarUmPagamento()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            connectionFactory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ_HostName"]
            };

            sut = new ProtocolamentoDeLancamentosFinanceirosRabbitMQService(configuration);
        }

        public override async void Act()
        {
            await sut.Handle(comandoOriginal, CancellationToken.None);
        }

        public class ComSucesso : AoProtocolarUmPagamento
        {
            public ComSucesso()
            {
                comandoOriginal = new ComandoDeLancamentoFinanceiro
                {
                    TipoDeLancamento = TipoDeLancamento.Pagamento,
                    ContaDestino = "conta falsa", // deve existir uma conta falsa, até mesmo em produção, para fins de teste
                    Protocolo = new Protocolo(Guid.NewGuid().ToString()),
                    Descricao = "teste_rabbit_mq",
                    Valor = 123.45m,
                };
            }

            [Fact]
            public void O_Comando_De_LancamentoFinanceiro_Deve_Ser_Enviado_Para_A_Fila_De_Pagamentos()
            {
                var comandoRecuperado = Act<ComandoDeLancamentoFinanceiro>(CONST_FILA_NOME);

                Assert.Equal(comandoOriginal.Protocolo.Id, comandoRecuperado.Protocolo.Id);
            }
        }
    }
}
