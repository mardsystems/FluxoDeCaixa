using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class ProcessadorDeLancamentosFinanceiros : IRequestHandler<ComandoDeLancamentoFinanceiro>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMediator mediator;

        private readonly IRepositorioDeLancamentos repositorioDeLancamentos;

        private readonly IRepositorioDeContas repositorioDeContas;

        public ProcessadorDeLancamentosFinanceiros(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioDeLancamentos repositorioDeLancamentos, IRepositorioDeContas repositorioDeContas)
        {
            this.unitOfWork = unitOfWork;

            this.mediator = mediator;

            this.repositorioDeLancamentos = repositorioDeLancamentos;

            this.repositorioDeContas = repositorioDeContas;
        }

        public async Task<Unit> Handle(ComandoDeLancamentoFinanceiro comando, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransaction();
            
            try
            {
                var conta = await repositorioDeContas.ObtemConta(comando.ContaDestino, comando.BancoDestino, comando.TipoDeConta, comando.CpfCnpjDestino);

                if (comando.TipoDeLancamento == TipoDeLancamento.Pagamento)
                {
                    var pagamento = new Pagamento(
                        comando.Protocolo,
                        comando.Valor,
                        comando.Descricao,
                        new DateTime(comando.data_de_lancamento.Year, comando.data_de_lancamento.Month, comando.data_de_lancamento.Day)
                    );

                    //

                    var lancamento = conta.LancaPagamento(pagamento);

                    //

                    await repositorioDeContas.Atualiza(conta);

                    await repositorioDeLancamentos.Adiciona(lancamento);

                    var evento = new EventoDeLancamentoFinanceiroProcessado
                    {
                        Lancamento = lancamento
                    };

                    await mediator.Publish(evento);
                }

                await unitOfWork.Commit();

                return await Unit.Task;
            }
            catch (Exception ex)
            {
                var _ = ex.Message;

                await unitOfWork.Rollback();

                throw;
            }
        }
    }
}
