using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace FluxoDeCaixa.Modulos.Contas
{
    public class ProcessadorDeLancamentos : IRequestHandler<ComandoDeLancamentoFinanceiro>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMediator mediator;

        private readonly IRepositorioDeContas repositorioDeContas;

        public ProcessadorDeLancamentos(IUnitOfWork unitOfWork, IMediator mediator, IRepositorioDeContas repositorioDeContas)
        {
            this.unitOfWork = unitOfWork;

            this.mediator = mediator;

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

                    var lancamento = await conta.Lanca(pagamento);

                    //

                    await repositorioDeContas.Atualiza(conta);

                    //await repositorioDeLancamentos.Adiciona(lancamento);

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
