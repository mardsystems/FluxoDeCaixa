using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    /// <summary>
    /// Representa um lançamento de um pagamento ou um recebimento numa conta.
    /// </summary>
    public class Lancamento
    {
        public virtual Protocolo Protocolo { get; set; }

        public string ProtocoloId { get; set; }

        [JsonIgnore]
        public virtual Conta Conta { get; set; }

        public string ContaId { get; set; }

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public TipoDeLancamento Tipo { get; set; }

        public Lancamento(Protocolo protocolo, Conta conta, DateTime data, string descricao, decimal valor, TipoDeLancamento tipo)
        {
            Protocolo = protocolo
                ?? throw new ArgumentNullException(nameof(protocolo));

            ProtocoloId = protocolo.Id;

            ContaId = conta.Id;

            Data = data;

            Descricao = descricao;

            Valor = valor;

            Tipo = tipo;
        }

        public Lancamento()
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
