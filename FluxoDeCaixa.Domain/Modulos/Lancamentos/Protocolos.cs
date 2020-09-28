using System;
using System.Collections.Generic;
using System.Text;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class Protocolo
    {
        public string Id { get; set; }

        public Protocolo()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Protocolo(string id)
        {
            Id = id;
        }
    }
}
