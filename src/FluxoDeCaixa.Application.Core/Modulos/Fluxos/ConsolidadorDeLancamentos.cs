using FluxoDeCaixa.Modulos.Contas;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Fluxos
{
    public class ConsolidadorDeLancamentos : INotificationHandler<EventoDeLancamentoFinanceiroProcessado>
    {
        private readonly IRepositorioDeFluxoDeCaixa repositorioDeFluxoDeCaixa;

        public ConsolidadorDeLancamentos(IRepositorioDeFluxoDeCaixa repositorioDeFluxoDeCaixa)
        {
            this.repositorioDeFluxoDeCaixa = repositorioDeFluxoDeCaixa;
        }

        public async Task Handle(EventoDeLancamentoFinanceiroProcessado evento, CancellationToken cancellationToken)
        {
            var lancamento = evento.Lancamento;

            try
            {
                var fluxo = await repositorioDeFluxoDeCaixa.ObtemFluxoDeCaixa(lancamento.Data);

                if (lancamento.Tipo == TipoDeLancamento.Pagamento)
                {
                    fluxo.AdicionaSaida(lancamento);
                }
                else
                {
                    fluxo.AdicionaEntrada(lancamento);
                }

                await repositorioDeFluxoDeCaixa.Atualiza(fluxo);
            }
            catch (EntityNotFoundException<FluxoDeCaixa> ex)
            {
                var _ = ex.Message;

                var fluxo = new FluxoDeCaixa
                {
                    Data = lancamento.Data
                };

                if (lancamento.Tipo == TipoDeLancamento.Pagamento)
                {
                    fluxo.AdicionaSaida(lancamento);
                }
                else
                {
                    fluxo.AdicionaEntrada(lancamento);
                }

                await repositorioDeFluxoDeCaixa.Adiciona(fluxo);
            }
            catch (Exception ex)
            {
                var _ = ex.Message;

                throw;
            }
        }
    }
}
