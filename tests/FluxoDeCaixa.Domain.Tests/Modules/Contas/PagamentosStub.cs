using System;

namespace FluxoDeCaixa.Modules.Contas
{
    public static class PagamentosStub
    {
        public static Pagamento ObtemPagamentoComValorQualquer(DateTime data)
        {
            var pagamento = new Pagamento(
                new Protocolo(null),
                123.45m,
                null,
                data
            );

            return pagamento;
        }

        public static Pagamento ObtemPagamentoComMenorValorPossivel()
        {
            var pagamento = new Pagamento(
                new Protocolo(null),
                0.001m,
                null,
                DateTime.Today
            );

            return pagamento;
        }
    }
}
