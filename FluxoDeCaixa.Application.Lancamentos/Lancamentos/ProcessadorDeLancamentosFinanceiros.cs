using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class ProcessadorDeLancamentosFinanceiros : IRequestHandler<ComandoDeLancamentoFinanceiro>
    {
        private readonly IMediator mediator;

        private readonly IRepositorioDeLancamentos repositorioDeLancamentos;

        private readonly IRepositorioDeContas repositorioDeContas;

        public ProcessadorDeLancamentosFinanceiros(IMediator mediator, IRepositorioDeLancamentos repositorioDeLancamentos, IRepositorioDeContas repositorioDeContas)
        {
            this.mediator = mediator;

            this.repositorioDeLancamentos = repositorioDeLancamentos;

            this.repositorioDeContas = repositorioDeContas;
        }

        public async Task<Unit> Handle(ComandoDeLancamentoFinanceiro comando, CancellationToken cancellationToken)
        {
            try
            {
                var conta = await repositorioDeContas.ObtemConta(comando.ContaDestino, comando.BancoDestino, comando.TipoDeConta, comando.CpfCnpjDestino);

                if (comando.TipoDeLancamento == TipoDeLancamento.Pagamento)
                {
                    if (conta.Saldo - comando.Valor < -20000)
                    {
                        throw new ApplicationException("A conta não pode ficar mais de R$ 20.000,00 negativos");
                    }
                }

                var lancamento = new Lancamento(
                    comando.Protocolo,
                    conta,
                    new DateTime(comando.data_de_lancamento.Year, comando.data_de_lancamento.Month, comando.data_de_lancamento.Day),
                    comando.Descricao,
                    comando.Valor,
                    TipoDeLancamento.Pagamento
                );

                if (lancamento.Tipo == TipoDeLancamento.Pagamento)
                {
                    conta.Saldo = conta.Saldo + lancamento.Valor;
                }
                else
                {
                    conta.Saldo = conta.Saldo - lancamento.Valor;
                }

                await repositorioDeContas.Atualiza(conta);

                await repositorioDeLancamentos.Adiciona(lancamento);

                var evento = new EventoDeLancamentoFinanceiroProcessado
                {
                    Lancamento = lancamento
                };

                await mediator.Publish(evento);

                return await Unit.Task;
            }
            catch (Exception ex)
            {
                var _ = ex.Message;

                throw;
            }
        }
    }
}
