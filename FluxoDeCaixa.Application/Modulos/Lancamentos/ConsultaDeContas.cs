using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public interface IConsultaDeContas
    {
        Task<Conta[]> ConsultaContas();
    }
}
