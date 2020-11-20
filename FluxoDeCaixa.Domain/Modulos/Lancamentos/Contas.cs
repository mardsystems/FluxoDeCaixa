using System;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class Conta
    {
        public const decimal LIMITE_DE_SALDO = -20000;

        public string Id { get; set; }

        public TipoDeConta Tipo { get; set; }

        public string Numero { get; set; }

        public string Banco { get; set; }

        public string Documento { get; set; }

        public string Email { get; set; }

        public decimal Saldo { get; set; }

        internal Lancamento LancaPagamento(Pagamento pagamento)
        {
            if (Saldo - pagamento.Valor < LIMITE_DE_SALDO)
            {
                throw new ApplicationException("A conta não pode ficar mais de R$ 20.000,00 negativos");
            }

            Saldo = Saldo - pagamento.Valor;

            var lancamento = new Lancamento(
                pagamento.Protocolo,
                this,
                pagamento.Data,
                pagamento.Descricao,
                pagamento.Valor,
                TipoDeLancamento.Pagamento
            );

            return lancamento;
        }
    }

    public enum TipoDeConta
    {
        Corrente,
        Poupanca
    }

    public interface IRepositorioDeContas
    {
        Task<Conta> ObtemConta(string numeroDaConta, string numeroDoBanco, TipoDeConta tipoDeConta, string numeroDoCpfOuCnpj);

        Task Atualiza(Conta conta);
    }
}
