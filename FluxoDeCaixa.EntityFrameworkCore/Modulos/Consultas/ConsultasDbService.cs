using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consultas
{
    public class ConsultasDbService : IConsultaDeFluxoDeCaixa
    {
        private readonly FluxoDeCaixaDbContext db;

        public ConsultasDbService(FluxoDeCaixaDbContext db)
        {
            this.db = db;
        }

        public async Task<FluxoDeCaixa> ConsultaFluxoDeCaixa(int dia, int mes, int ano)
        {
            var fluxo = await db.Fluxos
                .Include(fluxo => fluxo.Saidas)
                //.Include(fluxo => fluxo.Entradas)
                .Where(fluxo => fluxo.Data == new DateTime(ano, mes, dia))
                .Select(fluxo => new FluxoDeCaixa
                {
                    Data = fluxo.Data,
                    Saidas = fluxo.Saidas.Select(saida => new Saida
                    {
                        Data = saida.Data,
                        Valor = saida.Valor
                    }).ToArray(),
                    Total = fluxo.Total

                })
                .SingleOrDefaultAsync();

            if (fluxo == default)
            {
                throw new ApplicationException("Fluxo não encontrado.");
            }

            return fluxo;
        }
    }
}
