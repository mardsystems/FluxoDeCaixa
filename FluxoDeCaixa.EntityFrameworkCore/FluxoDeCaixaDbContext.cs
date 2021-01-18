using FluxoDeCaixa.Modulos.Lancamentos;
using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<Lancamento>()
                .HasKey(l => l.ProtocoloId);

            modelBuilder.Entity<Lancamento>()
                .Property(l => l.ProtocoloId).HasMaxLength(24);

            modelBuilder.Entity<Lancamento>()
                .OwnsOne(l => l.Protocolo, b => b.Property(p => p.Id).HasMaxLength(24).HasColumnName("ProtocoloId").IsRequired());

            modelBuilder.Entity<Conta>()
                .Property(p => p.Id).ValueGeneratedNever();

            modelBuilder.Entity<Conta>()
                .HasIndex(c => new { c.Numero, c.Banco, c.Tipo, c.Documento }).IsUnique();

            modelBuilder.Entity<Conta>()
                .HasData(
                    new Conta { Id = "1", Numero = "123", Banco = "001", Tipo = TipoDeConta.Corrente, Documento = "096", Email = "mardsystems@gmail.com", Saldo = 0 }
                );
        }
    }
}
