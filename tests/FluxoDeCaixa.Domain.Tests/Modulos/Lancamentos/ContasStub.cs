using Moq;
using System;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public static class ContasStub
    {
        public static Conta ObtemConta(decimal saldo, DateTime data, Mock<IRepositorioDeContas> repositorioDeContasMock)
        {
            var conta = new Conta(
                id: "2",
                tipo: TipoDeConta.Corrente,
                numero: "123",
                banco: "001",
                documento: "096",
                email: "",
                repositorio: repositorioDeContasMock.Object);

            repositorioDeContasMock.Setup(r => r.ObtemSaldoDaContaNaDataOrDefault(conta, data)).ReturnsAsync(new Saldo(conta, data, saldo));

            return conta;
        }
    }
}
