using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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

        public virtual ICollection<Saldo> Saldos { get; set; }

        public virtual ICollection<Lancamento> Lancamentos { get; set; }

        internal IRepositorioDeContas repositorio;

        internal Conta(
            string id,
            TipoDeConta tipo,
            string numero,
            string banco,
            string documento,
            string email,
            IRepositorioDeContas repositorio)
        {
            Id = id;

            Tipo = tipo;

            Numero = numero;

            Banco = banco;

            Documento = documento;

            Email = email;

            //Saldos = new HashSet<Saldo>();

            //Saldos.Add(new Saldo(this, DateTime.Today, saldo));

            this.repositorio = repositorio;
        }

        public async Task<Saldo> ObtemSaldoNaData(DateTime data)
        {
            var saldo = await repositorio.ObtemSaldoDaContaNaDataOrDefault(this, data);

            if (saldo == default)
            {
                var saldoNovo = await CriaSaldoNovoNaData(data);

                return saldoNovo;
            }
            else
            {
                return saldo;
            }
        }

        private async Task<Saldo> CriaSaldoNovoNaData(DateTime data)
        {
            var saldoNovo = new Saldo(this, data, valor: 0);

            await repositorio.Adiciona(saldoNovo);

            return saldoNovo;
        }

        internal async Task<Lancamento> Lanca(Pagamento pagamento)
        {
            var saldo = await ObtemSaldoNaData(pagamento.Data);

            if (saldo.Valor - pagamento.Valor < LIMITE_DE_SALDO)
            {
                throw new ApplicationException("A conta não pode ficar mais de R$ 20.000,00 negativos");
            }

            saldo.Valor = saldo.Valor - pagamento.Valor;

            var lancamento = new Lancamento(
                pagamento.Protocolo,
                this,
                pagamento.Data,
                pagamento.Descricao,
                pagamento.Valor,
                TipoDeLancamento.Pagamento
            );

            await repositorio.Adiciona(lancamento);

            return lancamento;
        }

        public Conta()
        {
            Saldos = new HashSet<Saldo>();
        }
    }

    public class Saldo
    {
        [JsonIgnore]
        public virtual Conta Conta { get; set; }

        public string ContaId { get; set; }

        public DateTime Data { get; set; }

        public decimal Valor { get; set; }

        public Saldo(Conta conta, DateTime data, decimal valor)
        {
            ContaId = conta.Id;

            Data = data.Date;

            Valor = valor;
        }

        public Saldo()
        {

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

        Task<Saldo> ObtemSaldoDaContaNaDataOrDefault(Conta conta, DateTime data);

        Task Atualiza(Conta conta);

        Task Adiciona(Saldo saldo);

        Task Adiciona(Lancamento lancamento);
    }
}
