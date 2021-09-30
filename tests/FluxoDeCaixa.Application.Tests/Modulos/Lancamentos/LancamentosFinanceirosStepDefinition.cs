using BoDi;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
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

        private readonly ProcessadorDeLancamentos sut;

        private readonly Mock<IMediator> mediatorMock;

        private readonly IRepositorioDeContas repositorioDeContas;

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

            repositorioDeContas = scope.ServiceProvider.GetRequiredService<IRepositorioDeContas>();

            sut = new ProcessadorDeLancamentos(
                unitOfWork,
                mediatorMock.Object,
                repositorioDeContas
            );

            world.Add("hoje", DateTime.Now.Date);
        }

        [Given("que estou na minha conta \\(banco: (.*), conta (.*): (.*), cpf: (.*)\\) com saldo positivo de (.*) reais")]
        public void DadaConta(string numeroDoBanco, string tipoDaContaString, string numeroDaConta, string documentoDaConta, decimal saldoDaConta)
        {
            var hoje = (DateTime)world["hoje"];

            world.Add(nameof(numeroDoBanco), numeroDoBanco);

            var tipoDaConta = (TipoDeConta)Enum.Parse(typeof(TipoDeConta), tipoDaContaString);

            world.Add(nameof(tipoDaConta), tipoDaConta);

            world.Add(nameof(numeroDaConta), numeroDaConta);

            world.Add(nameof(documentoDaConta), documentoDaConta);

            world.Add(nameof(saldoDaConta), saldoDaConta);

            var conta = new Conta { Id = "2", Numero = numeroDaConta, Banco = numeroDoBanco, Tipo = tipoDaConta, Documento = documentoDaConta, repositorio = repositorioDeContas };

            conta.Saldos = new HashSet<Saldo>()
            {
                new Saldo { ContaId = conta.Id, Data = hoje, Valor = saldoDaConta }
            };

            db.Contas.Add(conta);

            db.SaveChanges();
        }

        [When("lanço um pagamento de \"(.*)\" de (.*) reais nela no dia de hoje sob o protocolo (.*)")]
        public async Task QuandoLancoUmPagamento(string descricao, decimal valorDoPagamento, string numeroDoProtocolo)
        {
            var hoje = (DateTime)world["hoje"];

            var comando = new ComandoDeLancamentoFinanceiro
            {
                TipoDeLancamento = TipoDeLancamento.Pagamento,
                Descricao = descricao,
                ContaDestino = world["numeroDaConta"].ToString(),
                BancoDestino = world["numeroDoBanco"].ToString(),
                TipoDeConta = (TipoDeConta)world["tipoDaConta"],
                CpfCnpjDestino = world["documentoDaConta"].ToString(),
                Valor = valorDoPagamento,
                Encargos = 0,
                data_de_lancamento = hoje,
            };

            comando.AnexaProtocolo(new Protocolo(numeroDoProtocolo));

            world.Add(nameof(numeroDoProtocolo), numeroDoProtocolo);

            await sut.Handle(comando, CancellationToken.None);
        }

        [Then("o saldo da conta no dia de hoje deve ser de (.*) reais")]
        public void EntaoConta(decimal saldoEsperado)
        {
            var hoje = (DateTime)world["hoje"];

            var table = DatabaseModule.ExecuteForTest($"SELECT Valor FROM ContaSaldos WHERE ContaId = 2 AND Data = '{hoje:yyyy-MM-dd 00:00:00}'"); // TODO: ephoc.

            table.Rows.Should().HaveCountGreaterThan(0);

            var row = table.Rows[0];

            var saldoDaConta = Convert.ToDecimal(row["Valor"], CultureInfo.InvariantCulture);

            saldoDaConta.Should().Be(saldoEsperado);
        }

        [Then("um lançamento no valor de (.*) reais deve ser adicionado sob o número de protocolo (.*)")]
        public void EntaoLancamento(decimal valorDoLancamento, string numeroDoProtocolo)
        {
            var numeroDoProtocoloEsperado = world[nameof(numeroDoProtocolo)].ToString();

            var table = DatabaseModule.ExecuteForTest($"SELECT * FROM ContaLancamentos WHERE ProtocoloId = {numeroDoProtocoloEsperado}");

            table.Rows.Should().HaveCountGreaterThan(0);

            var row = table.Rows[0];

            var saldoDaConta = Convert.ToDecimal(row["Valor"], CultureInfo.InvariantCulture);

            saldoDaConta.Should().Be(valorDoLancamento);

            numeroDoProtocolo.Should().Be(numeroDoProtocoloEsperado);

            mediatorMock.Verify(mediator => mediator.Publish(It.IsAny<EventoDeLancamentoFinanceiroProcessado>(), CancellationToken.None));
        }
    }
}
