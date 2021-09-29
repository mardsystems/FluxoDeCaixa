using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class AoLancarUmPagamentoNumaConta : Test
    {
        protected Conta conta;

        protected Pagamento pagamento;

        protected Lancamento lancamento;

        protected readonly Mock<IRepositorioDeContas> repositorioDeContasMock;

        protected DateTime hoje = DateTime.Today;

        public AoLancarUmPagamentoNumaConta()
        {
            repositorioDeContasMock = new Mock<IRepositorioDeContas>();
        }

        public override void Act()
        {
            try
            {
                lancamento = conta.Lanca(pagamento).GetAwaiter().GetResult();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public class ComSucesso : AoLancarUmPagamentoNumaConta
        {
            public ComSucesso()
            {
                var saldo = 200;

                conta = ContasStub.ObtemConta(saldo, hoje, repositorioDeContasMock);

                pagamento = PagamentosStub.ObtemPagamentoComValorQualquer(hoje);
            }

            [Fact]
            public async Task O_Saldo_Da_Conta_Deve_Ser_Decrescido_Do_Valor_Do_Pagamento()
            {
                var saldoHoje = await conta.ObtemSaldoNaData(hoje);

                var saldoDaContaEsperadoHoje = saldoHoje.Valor - pagamento.Valor;

                //

                Act();

                //

                var saldoDaConta = await conta.ObtemSaldoNaData(hoje);

                Assert.Equal(saldoDaContaEsperadoHoje, saldoDaConta.Valor);
            }

            [Fact]
            public void Um_Lancamento_Deve_Ser_Retornado()
            {
                Act();

                //

                Assert.NotNull(lancamento);
            }

            [Fact]
            public void O_Protocolo_Do_Lancamento_Deve_Ser_Igual_Ao_Protocolo_Do_Pagamento()
            {
                Act();

                //

                Assert.Equal(pagamento.Protocolo, lancamento.Protocolo);
            }

            [Fact]
            public void A_Conta_Do_Lancamento_Deve_Ser_Igual_A_Conta()
            {
                Act();

                //

                Assert.Equal(conta.Id, lancamento.ContaId);
            }

            [Fact]
            public void A_Data_Do_Lancamento_Deve_Ser_Igual_A_Data_Do_Pagamento()
            {
                Act();

                //

                Assert.Equal(pagamento.Data, lancamento.Data);
            }

            [Fact]
            public void A_Descricao_Do_Lancamento_Deve_Ser_Igual_A_Descricao_Do_Pagamento()
            {
                Act();

                //

                Assert.Equal(pagamento.Descricao, lancamento.Descricao);
            }

            [Fact]
            public void O_Valor_Do_Lancamento_Deve_Ser_Igual_Ao_Valor_Do_Pagamento()
            {
                Act();

                //

                Assert.Equal(pagamento.Valor, lancamento.Valor);
            }

            [Fact]
            public void O_Tipo_Do_Lancamento_Deve_Ser_Igual_A_Pagamento()
            {
                Act();

                //

                Assert.Equal(TipoDeLancamento.Pagamento, lancamento.Tipo);
            }
        }

        public class ComSaldoNoLimite : AoLancarUmPagamentoNumaConta
        {
            public ComSaldoNoLimite()
            {
                var saldo = Conta.LIMITE_DE_SALDO;

                conta = ContasStub.ObtemConta(saldo, hoje, repositorioDeContasMock);

                pagamento = PagamentosStub.ObtemPagamentoComMenorValorPossivel();
            }

            [Fact]
            public void A_Conta_Deve_Lancar_Uma_Excecao()
            {
                Assert.Throws<ApplicationException>(Act);
            }
        }
    }
}
