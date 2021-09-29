using System;
using System.Collections.Generic;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class Pagamento : ValueObject
    {
        public virtual Protocolo Protocolo { get; set; }

        public decimal Valor { get; set; }

        public string Descricao { get; set; }

        public DateTime Data { get; set; }

        public Pagamento(Protocolo protocolo, decimal valor, string descricao, DateTime data)
        {
            Protocolo = protocolo;

            Descricao = descricao;

            Valor = valor;

            Data = data;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Protocolo;

            yield return Valor;

            yield return Descricao;

            yield return Data;
        }

        public Pagamento()
        {

        }
    }
}