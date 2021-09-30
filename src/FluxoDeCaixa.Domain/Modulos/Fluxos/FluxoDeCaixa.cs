using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Fluxos
{
    public class FluxoDeCaixa
    {
        public string Id { get; set; }

        public DateTime Data { get; set; }

        public ICollection<Lancamento> Entradas { get; set; }

        public ICollection<Lancamento> Saidas { get; set; }

        public ICollection<Lancamento> Encargos { get; set; }

        public decimal Total { get; set; }

        public decimal PosicaoDoDia { get; set; }

        public FluxoDeCaixa()
        {
            Entradas = new List<Lancamento>();

            Saidas = new List<Lancamento>();

            Encargos = new List<Lancamento>();
        }

        public void AdicionaEntrada(Lancamentos.Lancamento lancamento)
        {
            var entrada = new Lancamento(lancamento.Data, lancamento.Descricao, lancamento.Valor);

            Entradas.Add(entrada);

            Total = Total + entrada.Valor;
        }

        public void AdicionaSaida(Lancamentos.Lancamento lancamento)
        {
            var saida = new Lancamento(lancamento.Data, lancamento.Descricao, lancamento.Valor);

            Saidas.Add(saida);

            Total = Total - saida.Valor;
        }
    }

    public class Lancamento : ValueObject
    {
        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public Lancamento(DateTime data, string descricao, decimal valor)
        {
            Data = data;

            Descricao = descricao;

            Valor = valor;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Data;

            yield return Descricao;

            yield return Valor;
        }

        public Lancamento()
        {

        }
    }

    public interface IRepositorioDeFluxoDeCaixa
    {
        Task<FluxoDeCaixa> ObtemFluxoDeCaixa(DateTime data);

        Task Adiciona(FluxoDeCaixa fluxoDeCaixa);

        Task Atualiza(FluxoDeCaixa fluxoDeCaixa);
    }
}
