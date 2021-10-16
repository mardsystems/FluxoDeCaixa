using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modules.Contas
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

            conta.repositorio = this;

            return conta;
        }

        public async Task<Saldo> ObtemSaldoDaContaNaDataOrDefault(Conta conta, DateTime data)
        {
            var saldo = await db.ContaSaldos
                .Where(saldo => saldo.ContaId == conta.Id && saldo.Data.Date == data.Date)
                .SingleOrDefaultAsync();

            //if (saldo == default)
            //{
            //    throw new EntityNotFoundException<Conta>("Saldo não encontrado.");
            //}

            return saldo;
        }

        public async Task Atualiza(Conta conta)
        {
            await db.SaveChangesAsync();
        }

        public async Task Adiciona(Saldo saldo)
        {
            await db.ContaSaldos.AddAsync(saldo);

            await db.SaveChangesAsync();
        }

        public async Task Adiciona(Lancamento lancamento)
        {
            await db.ContaLancamentos.AddAsync(lancamento);

            await db.SaveChangesAsync();
        }
    }
}
