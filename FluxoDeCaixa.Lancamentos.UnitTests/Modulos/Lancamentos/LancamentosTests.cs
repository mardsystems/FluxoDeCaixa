using System;
using Xunit;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class AoLancarUmPagamentoNumaConta : Test
    {
        protected Conta conta;

        protected Pagamento pagamento;

        protected Lancamento lancamento;

        public override void Act()
        {
            lancamento = conta.LancaPagamento(pagamento);
        }

        public class ComSucesso : AoLancarUmPagamentoNumaConta
        {
            public ComSucesso()
            {
                conta = ContasStub.ObtemContaComSaldoQualquer();

                pagamento = PagamentosStub.ObtemPagamentoComValorQualquer();
            }

            [Fact]
            public void O_Saldo_Da_Conta_Deve_Ser_Decrescido_Do_Valor_Do_Pagamento()
            {
                var saldoHoje = conta.ObtemSaldoNaData(DateTime.Today);

                var saldoDaContaEsperadoHoje = saldoHoje.Valor - pagamento.Valor;

                //

                Act();

                //

                Assert.Equal(saldoDaContaEsperadoHoje, saldoHoje.Valor);
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

                Assert.Equal(conta, lancamento.Conta);
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
                conta = ContasStub.ObtemContaComSaldoNoLimite();

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
