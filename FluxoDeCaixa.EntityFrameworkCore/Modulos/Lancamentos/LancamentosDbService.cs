using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class LancamentosDbService : IRepositorioDeLancamentos
    {
        private readonly FluxoDeCaixaDbContext db;

        public LancamentosDbService(FluxoDeCaixaDbContext db)
        {
            this.db = db;
        }

        public async Task Adiciona(Lancamento lancamento)
        {
            await db.Lancamentos.AddAsync(lancamento);

            await db.SaveChangesAsync();
        }
    }
}
