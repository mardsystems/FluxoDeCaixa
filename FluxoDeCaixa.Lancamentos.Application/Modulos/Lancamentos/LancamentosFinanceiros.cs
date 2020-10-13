using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class ComandoDeLancamentoFinanceiro : IRequest
    {
        /// <summary>
        /// Tipos de lancamento (pagamento e recebimento).
        /// </summary>
        [Required]
        [JsonPropertyName("tipo_de_lancamento")]
        public TipoDeLancamento TipoDeLancamento { get; set; }

        /// <summary>
        /// Qualquer descrição sobre o pagamento.
        /// </summary>
        [Required]
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>
        /// Conta do destinatário.
        /// </summary>
        [Required]
        [JsonPropertyName("conta_destino")]
        public string ContaDestino { get; set; }

        /// <summary>
        /// Banco do destinatário.
        /// </summary>
        [Required]
        [JsonPropertyName("banco_destino")]
        public string BancoDestino { get; set; }

        /// <summary>
        /// Se a conta é corrente ou poupança.
        /// </summary>
        [Required]
        [JsonPropertyName("tipo_de_conta")]
        public TipoDeConta TipoDeConta { get; set; }

        /// <summary>
        /// Número formatado do CPF ou CNPJ.
        /// </summary>
        [Required]
        [JsonPropertyName("cpf_cnpj_destino")]
        public string CpfCnpjDestino { get; set; }

        /// <summary>
        /// Valor em reais do lançamento no formato (R$ 0.000,00).
        /// </summary>
        [Required]
        [JsonPropertyName("valor_do_lancamento")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Valor em reais dos encargos do lançamento no formato (R$ 0.000,00).
        /// </summary>
        [Required]
        [JsonPropertyName("encargos")]
        public decimal Encargos { get; set; }

        /// <summary>
        /// Data em que o lançamento ocorreu no formato (dd-mm-aaaa).
        /// </summary>
        [Required]
        [DataDeHojeOuNoFuturo]
        [JsonPropertyName("data_de_lancamento")]
        public DateTime data_de_lancamento { get; set; } // TODO: Renomear para Data quando resolver problema do RangeAttribute.

        public Protocolo Protocolo { get; set; }

        public void AnexaProtocolo(Protocolo protocolo)
        {
            Protocolo = protocolo;
        }
    }

    public class DataDeHojeOuNoFuturoAttribute : RangeAttribute
    {
        public DataDeHojeOuNoFuturoAttribute()
            : base(typeof(DateTime), DateTime.Today.ToShortDateString(), DateTime.MaxValue.ToShortDateString())
        {

        }
    }
}
