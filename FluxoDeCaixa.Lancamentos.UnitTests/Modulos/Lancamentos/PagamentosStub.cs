using System;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public static class PagamentosStub
    {
        public static Pagamento ObtemPagamentoComValorQualquer()
        {
            var pagamento = new Pagamento(
                new Protocolo(null),
                123.45m,
                null,
                DateTime.Today
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
