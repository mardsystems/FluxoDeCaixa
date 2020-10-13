using FluxoDeCaixa.Modulos.Lancamentos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public class ServicoDeConsolidacaoDeFinancas : INotificationHandler<EventoDeLancamentoFinanceiroProcessado>
    {
        private readonly IRepositorioDeFluxos repositorioDeFluxos;

        public ServicoDeConsolidacaoDeFinancas(IRepositorioDeFluxos repositorioDeFluxos)
        {
            this.repositorioDeFluxos = repositorioDeFluxos;
        }

        public async Task Handle(EventoDeLancamentoFinanceiroProcessado evento, CancellationToken cancellationToken)
        {
            var lancamento = evento.Lancamento;

            try
            {
                var fluxo = await repositorioDeFluxos.ObtemFluxo(lancamento.Data);

                if (lancamento.Tipo == TipoDeLancamento.Pagamento)
                {
                    fluxo.AdicionaSaida(lancamento);
                }
                else
                {
                    fluxo.AdicionaEntrada(lancamento);
                }

                await repositorioDeFluxos.Atualiza(fluxo);
            }
            catch (EntityNotFoundException<FluxoConsolidado> ex)
            {
                var _ = ex.Message;

                var fluxo = new FluxoConsolidado
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

                await repositorioDeFluxos.Adiciona(fluxo);
            }
            catch (Exception ex)
            {
                var _ = ex.Message;

                throw;
            }
        }
    }
}
