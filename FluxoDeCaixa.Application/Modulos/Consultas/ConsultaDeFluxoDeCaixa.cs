﻿using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consultas
{
    public interface IConsultaDeFluxoDeCaixa
    {
        Task<FluxoDeCaixa> ConsultaFluxoDeCaixa(int dia, int mes, int ano);
    }

    public class FluxoDeCaixa
    {
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        [JsonPropertyName("entradas")]
        public Entrada[] Entradas { get; set; }

        [JsonPropertyName("saidas")]
        public Saida[] Saidas { get; set; }

        [JsonPropertyName("encargos")]
        public Encargo[] Encargos { get; set; }

        /// <summary>
        /// Total de lançamentos do dia em reais no formato (R$ 0.000,00).
        /// </summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        /// <summary>
        /// Comparando com o dia anterior se houve aumento do caixa ou queda em percentual no formato(xxx.x%).
        /// </summary>
        [JsonPropertyName("posicao_do_dia")]
        public decimal PosicaoDoDia { get; set; }
    }

    public class Entrada
    {
        /// <summary>
        /// Data em que a entrada foi lançada no fluxo de caixa no formato(dd-mm-aaaa).
        /// </summary>
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor da entrada em reais no formato (R$ 0.000,00).
        /// </summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }

    public class Saida
    {
        /// <summary>
        /// "Data em que a saida foi lançada no fluxo de caixa no formato(dd-mm-aaaa)".
        /// </summary>
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor da saida em reais no formato (R$ 0.000,00).
        /// </summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }

    public class Encargo
    {
        /// <summary>
        /// Data em que o encargo foi lançada no fluxo de caixa no formato(dd-mm-aaaa).
        /// </summary>
        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor do encargo em reais no formato (R$ 0.000,00).
        /// </summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
