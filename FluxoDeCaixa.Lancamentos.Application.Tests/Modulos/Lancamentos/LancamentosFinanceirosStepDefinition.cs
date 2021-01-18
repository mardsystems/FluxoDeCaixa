using MediatR;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    [Binding]
    public sealed class LancamentosFinanceirosStepDefinition
    {
        private readonly IMediator mediator;

        private readonly FluxoDeCaixaDbContext db;

        private readonly ScenarioContext context;

        public LancamentosFinanceirosStepDefinition(
            ScenarioContext context,
            FluxoDeCaixaDbContext db,
            IMediator mediator)
        {
            this.context = context;

            this.db = db;

            this.mediator = mediator;
        }

        [Given("que estou na minha conta \\(banco: (.*), conta (.*): (.*), cpf: (.*)\\) com saldo positivo de (.*) reais")]
        public void DadaConta(string numeroDoBanco, string tipoDaContaString, string numeroDaConta, string documentoDaConta, decimal saldoDaConta)
        {
            context.Add(nameof(numeroDoBanco), numeroDoBanco);

            var tipoDaConta = (TipoDeConta)Enum.Parse(typeof(TipoDeConta), tipoDaContaString);

            context.Add(nameof(tipoDaConta), tipoDaConta);

            context.Add(nameof(numeroDaConta), numeroDaConta);

            context.Add(nameof(documentoDaConta), documentoDaConta);

            context.Add(nameof(saldoDaConta), saldoDaConta);

            //db.Contas.Add(new Conta { Id = "2", Numero = numeroDaConta, Banco = numeroDoBanco, Tipo = tipoDaConta, Documento = documentoDaConta, Saldo = saldoDaConta });

            db.SaveChanges();
        }

        [When("lanço um pagamento de (.*) reais nela sob o protocolo (.*)")]
        public async Task QuandoLancoUmPagamento(decimal valorDoPagamento, string numeroDoProtocolo)
        {
            var comando = new ComandoDeLancamentoFinanceiro
            {
                BancoDestino = context["numeroDoBanco"].ToString(),
                TipoDeConta = (TipoDeConta)context["tipoDaConta"],
                ContaDestino = context["numeroDaConta"].ToString(),
                CpfCnpjDestino = context["documentoDaConta"].ToString(),
                Valor = valorDoPagamento
            };

            comando.AnexaProtocolo(new Protocolo(Guid.NewGuid().ToString()));

            context.Add(nameof(numeroDoProtocolo), numeroDoProtocolo);

            await mediator.Send(comando);
        }

        [Then("o saldo da conta deve ser de (.*) reais")]
        public void EntaoConta(decimal saldoEsperado)
        {
            // TODO: usar um wrapper para buscar no banco de dados relacional se existe uma conta com saldo atualizado.
        }

        [Then("um lançamento no valor de (.*) reais deve ser adicionado")]
        public void EntaoLancamento(decimal valorDoLancamento)
        {
            // TODO: usar um wrapper para buscar no banco de dados relacional se existe um lançamento.
        }
    }
}
