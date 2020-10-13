using FluxoDeCaixa.Modulos.Lancamentos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public class FluxoConsolidado
    {
        public string Id { get; set; }

        public DateTime Data { get; set; }

        public ICollection<Lancamento> Entradas { get; set; }

        public ICollection<Lancamento> Saidas { get; set; }

        public ICollection<Lancamento> Encargos { get; set; }

        public decimal Total { get; set; }

        public decimal Percentual { get; set; }

        public FluxoConsolidado()
        {
            Entradas = new List<Lancamento>();

            Saidas = new List<Lancamento>();

            Encargos = new List<Lancamento>();
        }

        public void AdicionaEntrada(Lancamento entrada)
        {
            Entradas.Add(entrada);

            Total = Total + entrada.Valor;
        }

        public void AdicionaSaida(Lancamento saida)
        {
            Saidas.Add(saida);

            Total = Total - saida.Valor;
        }
    }

    public interface IRepositorioDeFluxos
    {
        Task<FluxoConsolidado> ObtemFluxo(DateTime data);

        Task Adiciona(FluxoConsolidado fluxo);

        Task Atualiza(FluxoConsolidado fluxo);
    }
}
