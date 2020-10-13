using FluxoDeCaixa.Modulos.Consolidacao;
using FluxoDeCaixa.Modulos.Lancamentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluxoDeCaixa
{
    public class FluxoDeCaixaDbContext : DbContext
    {
        public DbSet<Conta> Contas { get; set; }

        public DbSet<Lancamento> Lancamentos { get; set; }

        public FluxoDeCaixaDbContext(DbContextOptions<FluxoDeCaixaDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lancamento>(builder => builder.HasKey(p => p.ProtocoloId));

            modelBuilder.Entity<Conta>()
                .HasIndex(conta => new { conta.Numero, conta.Banco, conta.Tipo, conta.Documento })
                .IsUnique();

            modelBuilder.Entity<Conta>()
                .HasData(
                    new Conta { Id = "1", Numero = "123", Banco = "001", Tipo = TipoDeConta.Corrente, Documento = "096", Email = "mardsystems@gmail.com", Saldo = 0 }
                );
        }
    }
}
