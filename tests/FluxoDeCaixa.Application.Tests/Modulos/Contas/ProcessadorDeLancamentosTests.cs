using MediatR;
using Moq;
using System.Threading;
using System.Transactions;
using Xunit;

namespace FluxoDeCaixa.Modulos.Contas
{
    public class AoProcessarLancamento : Test
    {
        protected readonly Mock<IUnitOfWork> unitOfWorkMock;

        protected readonly Mock<IMediator> mediatorMock;

        protected readonly Mock<IRepositorioDeContas> repositorioDeContasMock;

        protected readonly ProcessadorDeLancamentos sut;

        protected ComandoDeLancamentoFinanceiro comando;

        public AoProcessarLancamento()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();

            repositorioDeContasMock = new Mock<IRepositorioDeContas>();

            sut = new ProcessadorDeLancamentos(
                unitOfWorkMock.Object,
                mediatorMock.Object,
                repositorioDeContasMock.Object);
        }

        public async override void Act()
        {
            await sut.Handle(comando, CancellationToken.None);
        }

        public class ComSucesso : AoProcessarLancamento
        {
            public ComSucesso()
            {
                comando = new ComandoDeLancamentoFinanceiro
                {

                };
            }

            [Fact]
            public void x() //decimal valorDoLancamento, string numeroDoProtocolo
            {
                var numeroDoProtocoloEsperado = comando.Protocolo.Id;

                //var saldoDaConta = Convert.ToDecimal(row["Valor"], CultureInfo.InvariantCulture);

                //saldoDaConta.Should().Be(valorDoLancamento);

                //numeroDoProtocolo.Should().Be(numeroDoProtocoloEsperado);

                mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<EventoDeLancamentoFinanceiroProcessado>(), CancellationToken.None));
            }
        }
    }
}
