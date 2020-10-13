using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class ContasDbService : IRepositorioDeContas
    {
        private readonly FluxoDeCaixaDbContext db;

        public ContasDbService(FluxoDeCaixaDbContext db)
        {
            this.db = db;
        }

        public async Task<Conta> ObtemConta(string numeroDaConta, string numeroDoBanco, TipoDeConta tipoDeConta, string numeroDoCpfOuCnpj)
        {
            var conta = await db.Contas
                .Where(conta =>
                    conta.Numero == numeroDaConta &&
                    conta.Banco == numeroDoBanco &&
                    conta.Tipo == tipoDeConta &&
                    conta.Documento == numeroDoCpfOuCnpj)
                .SingleOrDefaultAsync();

            if (conta == default)
            {
                throw new EntityNotFoundException<Conta>("Conta não encontrada.");
            }

            return conta;
        }

        public async Task Atualiza(Conta conta)
        {
            await db.SaveChangesAsync();
        }
    }
}
