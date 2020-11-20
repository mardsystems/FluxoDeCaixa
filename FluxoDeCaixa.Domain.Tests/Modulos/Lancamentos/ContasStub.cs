namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public static class ContasStub
    {
        public static Conta ObtemContaComSaldoQualquer()
        {
            var conta = new Conta();

            conta.Saldo = 200;

            return conta;
        }

        public static Conta ObtemContaComSaldoNoLimite()
        {
            var conta = new Conta();

            conta.Saldo = Conta.LIMITE_DE_SALDO;

            return conta;
        }
    }
}
