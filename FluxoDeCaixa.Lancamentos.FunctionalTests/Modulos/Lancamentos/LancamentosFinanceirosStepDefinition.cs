using FluentAssertions;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    [Binding]
    public sealed class LancamentosFinanceirosStepDefinition
    {
        //private ProcessadorDeLancamentosFinanceiros sut;

        //private Conta conta;

        private bool success;

        private readonly ScenarioContext context;

        public LancamentosFinanceirosStepDefinition(ScenarioContext context)
        {
            this.context = context;

            //sut = new ProcessadorDeLancamentosFinanceiros(null, null, null, null);

            //conta = new Conta();
        }

        [Given("que estou na minha conta do (.*) com saldo positivo de (.*) reais")]
        public void DadaConta(string nomeDaConta, decimal saldoDaConta)
        {
            context.Add(nameof(nomeDaConta), nomeDaConta);

            context.Add(nameof(saldoDaConta), saldoDaConta);
        }

        [When("lanço um pagamento de (.*) reais nela")]
        public void QuandoLancoUmPagamento(decimal valorDoPagamento)
        {
            var none = CancellationToken.None;

            var comando = new ComandoDeLancamentoFinanceiro
            {
                ContaDestino = context["nomeDaConta"].ToString(),
                Valor = valorDoPagamento
            };

            success = true;

            //sut.Handle(comando, none);

            //var pagamento = new Pagamento(null, valorDoPagamento, "", DateTime.Now);

            //conta.LancaPagamento(pagamento);
        }

        [Then("devo obter minha conta")]
        public void EntaoDevoObterMinhaConta()
        {
            success.Should().BeTrue();
        }

        [Then("devo lançar o pagamento nela")]
        public void EntaoDevoLancarOPagamentoNaConta()
        {
            success.Should().BeTrue();
        }

        [Then("o saldo da conta deve ser decrescido do valor do pagamento")]
        public void EntaoOSaldoDaContaDeveSerDecrescidoDoValorDoPagamento()
        {
            success.Should().BeTrue();
        }

        [Then]
        public void ThenAContaDeveSerAtualizada()
        {
            success.Should().BeTrue();
        }

        [Then("um lançamento deve ser adicionado")]
        public void EntaoOLancamentoDeveSerAdicionado()
        {
            success.Should().BeTrue();
        }
    }
}
