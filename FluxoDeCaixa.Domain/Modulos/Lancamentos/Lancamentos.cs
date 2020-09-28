using MediatR;
using System;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class Lancamento
    {
        public int Id { get; set; }

        public Protocolo Protocolo { get; set; }

        public Conta Conta { get; set; }

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public TipoDeLancamento Tipo { get; set; }

        public Lancamento(Protocolo protocolo, Conta conta, DateTime data, string descricao, decimal valor, TipoDeLancamento tipo)
        {
            Protocolo = protocolo;

            Conta = conta;

            Data = data;

            Descricao = descricao;

            Valor = valor;

            Tipo = tipo;
        }

        private Lancamento()
        {

        }
    }

    public enum TipoDeLancamento
    {
        Pagamento,
        Recebimento
    }

    public interface IRepositorioDeLancamentos
    {
        Task Adiciona(Lancamento lancamento);
    }

    public class EventoDeLancamentoFinanceiroProcessado : INotification
    {
        public Lancamento Lancamento { get; set; }
    }
}
