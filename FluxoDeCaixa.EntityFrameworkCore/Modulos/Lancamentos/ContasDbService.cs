using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class ContasDbService : IConsultaDeContas, IRepositorioDeContas
    {
        private readonly FluxoDeCaixaDbContext db;

        public ContasDbService(FluxoDeCaixaDbContext db)
        {
            this.db = db;
        }

        public async Task<Conta[]> ConsultaContas()
        {
            var contas = await db.Contas.ToArrayAsync();

            return contas;
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
                throw new ApplicationException("Conta não encontrada.");
            }

            return conta;
        }
    }
}
