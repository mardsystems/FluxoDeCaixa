using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class Protocolo : ValueObject
    {
        public string Id { get; set; }

        public Protocolo(string id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
        }

        public Protocolo()
        {

        }
    }

    public interface IGeracaoDeProtocolos
    {
        Task<Protocolo> GeraProtocolo();
    }
}
