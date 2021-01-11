using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public interface ILancamentos
    {
        Lancamento LancaPagamento(Pagamento pagamento, IRepositorioDeContas repositorioDeContas);
    }

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

        //public ICollection<Saldo> Saldos { get; set; }

        internal Conta(decimal saldo)
        {
            Saldo = saldo;

            //Saldos = new HashSet<Saldo>();

            //Saldos.Add(new Saldo(DateTime.Today, saldo));
        }

        //public Saldo ObtemSaldoNaData(DateTime data)
        //{
        //    var saldo = Saldos.SingleOrDefault(saldo => saldo.Data.Date == data.Date);

        //    if (saldo == default)
        //    {
        //        var saldoNovo = CriaSaldoNovoNaData(data);

        //        return saldoNovo;
        //    }
        //    else
        //    {
        //        return saldo;
        //    }
        //}

        //private Saldo CriaSaldoNovoNaData(DateTime data)
        //{
        //    var saldoNovo = new Saldo(data, valor: 0);

        //    Saldos.Add(saldoNovo);

        //    return saldoNovo;
        //}

        internal Lancamento LancaPagamento(Pagamento pagamento)
        {
            //var saldo = ObtemSaldoNaData(pagamento.Data);

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

        internal Conta()
        {

        }
    }

    public class Saldo
    {
        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public Saldo(DateTime data, decimal valor)
        {
            Data = data;

            Valor = valor;
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

        //decimal ObtemSaldoDaContaNaData(Conta conta, DateTime data);

        Task Atualiza(Conta conta);
    }
}
