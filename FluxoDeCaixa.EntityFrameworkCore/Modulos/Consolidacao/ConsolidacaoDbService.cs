using FluxoDeCaixa.Modulos.Lancamentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public class ConsolidacaoDbService : IRepositorioDeFluxos
    {
        private readonly FluxoDeCaixaDbContext db;

        public ConsolidacaoDbService(FluxoDeCaixaDbContext db)
        {
            this.db = db;
        }

        public async Task<FluxoConsolidado> ObtemFluxo(DateTime data)
        {
            var fluxo = await db.Fluxos
                .Where(fluxo => fluxo.Data == data)
                .SingleOrDefaultAsync();

            if (fluxo == default)
            {
                throw new ApplicationException("Fluxo não encontrado.");
            }

            return fluxo;
        }

        public async Task Adiciona(FluxoConsolidado fluxo)
        {
            await db.Fluxos.AddAsync(fluxo);

            await db.SaveChangesAsync();
        }

        public Task AdicionaEntrada(Lancamento lancamento)
        {
            throw new NotImplementedException();
        }

        public async Task Atualiza(FluxoConsolidado fluxo)
        {
            await db.SaveChangesAsync();
        }
    }
}
