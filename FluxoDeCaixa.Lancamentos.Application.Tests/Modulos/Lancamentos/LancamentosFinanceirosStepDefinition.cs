using BoDi;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TechTalk.SpecFlow;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    [Binding]
    public sealed class LancamentosFinanceirosStepDefinition
    {
        private readonly ScenarioContext world;

        private readonly FluxoDeCaixaDbContext db;

        private readonly ProcessadorDeLancamentosFinanceiros sut;

        private readonly Mock<IMediator> mediatorMock;

        public LancamentosFinanceirosStepDefinition(
            ScenarioContext world,
            FluxoDeCaixaDbContext db,
            IServiceScopeFactory scopeFactory,
            IObjectContainer container)
        {
            this.world = world;

            this.db = db;

            var scope = scopeFactory.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            mediatorMock = new Mock<IMediator>();

            var repositorioDeLancamentos = scope.ServiceProvider.GetRequiredService<IRepositorioDeLancamentos>();

            var repositorioDeContas = scope.ServiceProvider.GetRequiredService<IRepositorioDeContas>();

            sut = new ProcessadorDeLancamentosFinanceiros(
                unitOfWork,
                mediatorMock.Object,
                repositorioDeLancamentos,
                repositorioDeContas
            );
        }

        [Given("que estou na minha conta \\(banco: (.*), conta (.*): (.*), cpf: (.*)\\) com saldo positivo de (.*) reais")]
        public void DadaConta(string numeroDoBanco, string tipoDaContaString, string numeroDaConta, string documentoDaConta, decimal saldoDaConta)
        {
            world.Add(nameof(numeroDoBanco), numeroDoBanco);

            var tipoDaConta = (TipoDeConta)Enum.Parse(typeof(TipoDeConta), tipoDaContaString);

            world.Add(nameof(tipoDaConta), tipoDaConta);

            world.Add(nameof(numeroDaConta), numeroDaConta);

            world.Add(nameof(documentoDaConta), documentoDaConta);

            world.Add(nameof(saldoDaConta), saldoDaConta);

            db.Contas.Add(new Conta { Id = "2", Numero = numeroDaConta, Banco = numeroDoBanco, Tipo = tipoDaConta, Documento = documentoDaConta, Saldo = saldoDaConta });

            db.SaveChanges();
        }

        [When("lanço um pagamento de (.*) reais nela sob o protocolo (.*)")]
        public async Task QuandoLancoUmPagamento(decimal valorDoPagamento, string numeroDoProtocolo)
        {
            var comando = new ComandoDeLancamentoFinanceiro
            {
                BancoDestino = world["numeroDoBanco"].ToString(),
                TipoDeConta = (TipoDeConta)world["tipoDaConta"],
                ContaDestino = world["numeroDaConta"].ToString(),
                CpfCnpjDestino = world["documentoDaConta"].ToString(),
                Valor = valorDoPagamento
            };

            comando.AnexaProtocolo(new Protocolo(numeroDoProtocolo));

            world.Add(nameof(numeroDoProtocolo), numeroDoProtocolo);

            await sut.Handle(comando, CancellationToken.None);
        }

        [Then("o saldo da conta deve ser de (.*) reais")]
        public void EntaoConta(decimal saldoEsperado)
        {
            var table = Database.ExecuteForTest($"SELECT Saldo FROM Contas WHERE Id = 2");

            table.Rows.Should().HaveCountGreaterThan(0);

            var row = table.Rows[0];

            var saldoDaConta = Convert.ToDecimal(row["Saldo"], CultureInfo.InvariantCulture);

            saldoDaConta.Should().Be(saldoEsperado);
        }

        [Then("um lançamento no valor de (.*) reais deve ser adicionado sob o número de protocolo (.*)")]
        public void EntaoLancamento(decimal valorDoLancamento, string numeroDoProtocolo)
        {
            var numeroDoProtocoloEsperado = world[nameof(numeroDoProtocolo)].ToString();

            var table = Database.ExecuteForTest($"SELECT * FROM Lancamentos WHERE ProtocoloId = {numeroDoProtocoloEsperado}");

            table.Rows.Should().HaveCountGreaterThan(0);

            var row = table.Rows[0];

            var saldoDaConta = Convert.ToDecimal(row["Valor"], CultureInfo.InvariantCulture);

            saldoDaConta.Should().Be(valorDoLancamento);

            numeroDoProtocolo.Should().Be(numeroDoProtocoloEsperado);

            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<EventoDeLancamentoFinanceiroProcessado>(), CancellationToken.None));
        }
    }
}
