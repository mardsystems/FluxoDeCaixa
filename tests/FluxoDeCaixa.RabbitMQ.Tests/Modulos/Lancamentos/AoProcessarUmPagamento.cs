using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public abstract class AoProcessarUmPagamento : RabbitMQIntegrationTest
    {
        private readonly ProcessadorDeLancamentosProtocoladosRabbitMQ sut;

        private readonly Mock<IMediator> mediatorMock;

        public const string CONST_FILA_NOME = "pagamentos";

        private ComandoDeLancamentoFinanceiro comandoOriginal;

        public AoProcessarUmPagamento()
        {
            mediatorMock = new Mock<IMediator>();

            services.AddTransient((p) => mediatorMock.Object);

            IServiceScopeFactory serviceScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            sut = new ProcessadorDeLancamentosProtocoladosRabbitMQ(configuration, serviceScopeFactory);
        }

        public override void Act()
        {
            var comando = new ComandoParaProcessarLancamentosFinanceiros();

            sut.Handle(comando, CancellationToken.None).GetAwaiter().GetResult();
        }

        public class ComSucesso : AoProcessarUmPagamento
        {
            public ComSucesso()
            {
                comandoOriginal = new ComandoDeLancamentoFinanceiro
                {
                    TipoDeLancamento = TipoDeLancamento.Pagamento,
                    ContaDestino = "conta falsa", // deve existir uma conta falsa, até mesmo em produção, para fins de teste
                    Descricao = "teste_processamento_rabbit_mq",
                    Valor = 543.21m,
                };

                var protocolo = new Protocolo(Guid.NewGuid().ToString());

                comandoOriginal.AnexaProtocolo(protocolo);
            }

            [Fact]
            public void O_Comando_De_LancamentoFinanceiro_Deve_Ser_Enviado_Para_A_Fila_De_Pagamentos()
            {
                ActAndPublish(comandoOriginal, CONST_FILA_NOME);

                mediatorMock.Verify(mediator => mediator.Send(It.Is<ComandoDeLancamentoFinanceiro>(comando => comando.Protocolo.Id == comandoOriginal.Protocolo.Id), CancellationToken.None));
            }
        }
    }
}
