using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class Conta
    {
        public string Id { get; set; }

        public TipoDeConta Tipo { get; set; }

        public string Numero { get; set; }

        public string Banco { get; set; }

        public string Documento { get; set; }

        public string Email { get; set; }

        public decimal Saldo { get; set; }
    }

    public enum TipoDeConta
    {
        Corrente,
        Poupanca
    }

    public interface IRepositorioDeContas
    {
        Task<Conta> ObtemConta(string numeroDaConta, string numeroDoBanco, TipoDeConta tipoDeConta, string numeroDoCpfOuCnpj);
        
        Task Atualiza(Conta conta);
    }
}
