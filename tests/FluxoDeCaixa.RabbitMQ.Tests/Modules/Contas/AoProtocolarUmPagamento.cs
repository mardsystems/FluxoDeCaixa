using System;
using System.Threading;
using Xunit;

namespace FluxoDeCaixa.Modules.Contas
{
    public abstract class AoProtocolarUmPagamento : RabbitMQIntegrationTest
    {
        private readonly ProtocoladorDeLancamentosRabbitMQ sut;

        public const string CONST_FILA_NOME = "pagamentos";

        private ComandoDeLancamentoFinanceiro comandoOriginal;

        public AoProtocolarUmPagamento()
        {
            sut = new ProtocoladorDeLancamentosRabbitMQ(configuration);
        }

        public override void Act()
        {
            sut.Handle(comandoOriginal, CancellationToken.None).GetAwaiter().GetResult();
        }

        public class ComSucesso : AoProtocolarUmPagamento
        {
            public ComSucesso()
            {
                comandoOriginal = new ComandoDeLancamentoFinanceiro
                {
                    TipoDeLancamento = TipoDeLancamento.Pagamento,
                    ContaDestino = "conta falsa", // deve existir uma conta falsa, até mesmo em produção, para fins de teste
                    Descricao = "teste_protocoloamento_rabbit_mq",
                    Valor = 123.45m,
                };

                var protocolo = new Protocolo(Guid.NewGuid().ToString());

                comandoOriginal.AnexaProtocolo(protocolo);
            }

            [Fact]
            public void O_Comando_De_LancamentoFinanceiro_Deve_Ser_Enviado_Para_A_Fila_De_Pagamentos()
            {
                var comandoRecuperado = ActAndReceive<ComandoDeLancamentoFinanceiro>(CONST_FILA_NOME);

                Assert.Equal(comandoOriginal.Protocolo.Id, comandoRecuperado.Protocolo.Id);
            }
        }
    }
}
