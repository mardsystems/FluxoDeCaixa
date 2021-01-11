namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public static class ContasStub
    {
        public static Conta ObtemContaComSaldoQualquer()
        {
            var saldo = 200;

            var conta = new Conta(saldo);

            return conta;
        }

        public static Conta ObtemContaComSaldoNoLimite()
        {
            var saldo = Conta.LIMITE_DE_SALDO;

            var conta = new Conta(saldo);

            return conta;
        }
    }
}
