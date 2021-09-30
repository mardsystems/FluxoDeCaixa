using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Fluxos
{
    public class ConsolidacaoMongoDBService :
        IRepositorioDeFluxoDeCaixa,
        IRequestHandler<SolicitacaoDeConsultaDeFluxoDeCaixaDoDia, FluxoDeCaixaDoDia>
    {
        private readonly IMongoCollection<BsonDocument> collection;

        public ConsolidacaoMongoDBService()
        {
            var client = new MongoClient("mongodb://banco_analitico:27017");

            var database = client.GetDatabase("FluxoDeCaixa");

            collection = database.GetCollection<BsonDocument>("fluxos");
        }

        public async Task<FluxoDeCaixaDoDia> Handle(SolicitacaoDeConsultaDeFluxoDeCaixaDoDia request, CancellationToken cancellationToken)
        {
            var data = new DateTime(request.Ano, request.Mes, request.Dia);

            var filter = Builders<BsonDocument>.Filter.Eq("data", data);

            var document = await collection.Find(filter).SingleOrDefaultAsync();

            if (document == default)
            {
                throw new EntityNotFoundException<FluxoDeCaixaDoDia>("Fluxo de caixa não encontrado.");
            }

            var fluxoDeCaixaDoDia = MapFluxoDeCaixaDoDiaFrom(document);

            return fluxoDeCaixaDoDia;

            //var fluxo = await db.Fluxos
            //    .Include(fluxo => fluxo.Saidas)
            //    //.Include(fluxo => fluxo.Entradas)
            //    .Where(fluxo => fluxo.Data == new DateTime(ano, mes, dia))
            //    .Select(fluxo => new FluxoDeCaixaDoDia
            //    {
            //        Data = fluxo.Data,
            //        Saidas = fluxo.Saidas.Select(saida => new Saida
            //        {
            //            Data = saida.Data,
            //            Valor = saida.Valor
            //        }).ToArray(),
            //        Total = fluxo.Total

            //    })
            //    .SingleOrDefaultAsync();

            //if (fluxo == default)
            //{
            //    throw new ApplicationException("Fluxo não encontrado.");
            //}

            //return fluxo;
        }

        public async Task<FluxoDeCaixa> ObtemFluxoDeCaixa(DateTime data)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("data", data);

            var document = await collection.Find(filter).SingleOrDefaultAsync();

            if (document == default)
            {
                throw new EntityNotFoundException<FluxoDeCaixa>("Fluxo não encontrado.");
            }

            var fluxoDeCaixa = MapFluxoDeCaixaFrom(document);

            return fluxoDeCaixa;
        }

        public async Task Adiciona(FluxoDeCaixa fluxoDeCaixa)
        {
            var document = new BsonDocument
            {
                { "_id", ObjectId.GenerateNewId(fluxoDeCaixa.Data) },
                { "data", fluxoDeCaixa.Data },
                { "entradas", new BsonArray(fluxoDeCaixa.Entradas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "saidas", new BsonArray(fluxoDeCaixa.Saidas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "encargos", new BsonArray(fluxoDeCaixa.Encargos.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "total", fluxoDeCaixa.Total },
                { "posicao_do_dia", fluxoDeCaixa.PosicaoDoDia },
            };

            await collection.InsertOneAsync(document);
        }

        public async Task Atualiza(FluxoDeCaixa fluxoDeCaixa)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(fluxoDeCaixa.Id));

            var document = new BsonDocument
            {
                { "entradas", new BsonArray(fluxoDeCaixa.Entradas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "saidas", new BsonArray(fluxoDeCaixa.Saidas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "encargos", new BsonArray(fluxoDeCaixa.Encargos.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "data", fluxoDeCaixa.Data },
                { "total", fluxoDeCaixa.Total },
                { "posicao_do_dia", fluxoDeCaixa.PosicaoDoDia },
            };

            await collection.ReplaceOneAsync(filter, document);
        }

        private static FluxoDeCaixa MapFluxoDeCaixaFrom(BsonDocument document)
        {
            return new FluxoDeCaixa
            {
                Id = document["_id"].AsObjectId.ToString(),
                Data = document["data"].ToUniversalTime(),
                Entradas = document["entradas"].AsBsonArray.Select(entrada => new Lancamento { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToList(),
                Saidas = document["saidas"].AsBsonArray.Select(entrada => new Lancamento { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToList(),
                Encargos = document["encargos"].AsBsonArray.Select(entrada => new Lancamento { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToList(),
                Total = document["total"].AsDecimal,
                PosicaoDoDia = document["posicao_do_dia"].AsDecimal,
            };
        }

        private static FluxoDeCaixaDoDia MapFluxoDeCaixaDoDiaFrom(BsonDocument document)
        {
            return new FluxoDeCaixaDoDia
            {
                Data = document["data"].ToUniversalTime(),
                Entradas = document["entradas"].AsBsonArray.Select(entrada => new Entrada { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToArray(),
                Saidas = document["saidas"].AsBsonArray.Select(entrada => new Saida { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToArray(),
                Encargos = document["encargos"].AsBsonArray.Select(entrada => new Encargo { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToArray(),
                Total = document["total"].AsDecimal,
                PosicaoDoDia = document["posicao_do_dia"].AsDecimal,
            };
        }
    }
}
