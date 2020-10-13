using FluxoDeCaixa.Modulos.Lancamentos;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public class ConsolidacaoMongoDBService : IRepositorioDeFluxos,
        IRequestHandler<SolicitacaoDeConsultaDeFluxoDeCaixa, FluxoDeCaixa>
    {
        private readonly IMongoCollection<BsonDocument> collection;

        public ConsolidacaoMongoDBService()
        {
            var client = new MongoClient("mongodb://banco_analitico:27017");

            var database = client.GetDatabase("FluxoDeCaixa");

            collection = database.GetCollection<BsonDocument>("fluxos");
        }

        public async Task<FluxoDeCaixa> Handle(SolicitacaoDeConsultaDeFluxoDeCaixa request, CancellationToken cancellationToken)
        {
            var data = new DateTime(request.Ano, request.Mes, request.Dia);

            var filter = Builders<BsonDocument>.Filter.Eq("data", data);

            var document = await collection.Find(filter).SingleOrDefaultAsync();

            if (document == default)
            {
                throw new EntityNotFoundException<FluxoDeCaixa>("Fluxo não encontrado.");
            }

            var fluxo = MapFrom2(document);

            return fluxo;

            //var fluxo = await db.Fluxos
            //    .Include(fluxo => fluxo.Saidas)
            //    //.Include(fluxo => fluxo.Entradas)
            //    .Where(fluxo => fluxo.Data == new DateTime(ano, mes, dia))
            //    .Select(fluxo => new FluxoDeCaixa
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

        public async Task<FluxoConsolidado> ObtemFluxo(DateTime data)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("data", data);

            var document = await collection.Find(filter).SingleOrDefaultAsync();

            if (document == default)
            {
                throw new EntityNotFoundException<FluxoConsolidado>("Fluxo não encontrado.");
            }

            var fluxo = MapFrom(document);

            return fluxo;
        }

        public async Task Adiciona(FluxoConsolidado fluxo)
        {
            var document = new BsonDocument
            {
                { "_id", ObjectId.GenerateNewId(fluxo.Data) },
                { "data", fluxo.Data },
                { "entradas", new BsonArray(fluxo.Entradas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "saidas", new BsonArray(fluxo.Saidas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "encargos", new BsonArray(fluxo.Encargos.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "total", fluxo.Total },
                { "percentual", fluxo.Percentual },
            };

            await collection.InsertOneAsync(document);
        }

        public async Task Atualiza(FluxoConsolidado fluxo)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(fluxo.Id));

            var document = new BsonDocument
            {
                { "entradas", new BsonArray(fluxo.Entradas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "saidas", new BsonArray(fluxo.Saidas.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "encargos", new BsonArray(fluxo.Encargos.Select(entrada=> new BsonDocument { { "data", entrada.Data }, { "valor", entrada.Valor } })) },
                { "data", fluxo.Data },
                { "total", fluxo.Total },
                { "percentual", fluxo.Percentual },
            };

            await collection.ReplaceOneAsync(filter, document);
        }

        private static FluxoConsolidado MapFrom(BsonDocument document)
        {
            return new FluxoConsolidado
            {
                Id = document["_id"].AsObjectId.ToString(),
                Data = document["data"].ToUniversalTime(),
                Entradas = document["entradas"].AsBsonArray.Select(entrada => new Lancamento { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToList(),
                Saidas = document["saidas"].AsBsonArray.Select(entrada => new Lancamento { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToList(),
                Encargos = document["encargos"].AsBsonArray.Select(entrada => new Lancamento { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToList(),
                Total = document["total"].AsDecimal,
                Percentual = document["percentual"].AsDecimal,
            };
        }

        private static FluxoDeCaixa MapFrom2(BsonDocument document)
        {
            return new FluxoDeCaixa
            {
                Data = document["data"].ToUniversalTime(),
                Entradas = document["entradas"].AsBsonArray.Select(entrada => new Entrada { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToArray(),
                Saidas = document["saidas"].AsBsonArray.Select(entrada => new Saida { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToArray(),
                Encargos = document["encargos"].AsBsonArray.Select(entrada => new Encargo { Data = entrada["data"].ToUniversalTime(), Valor = entrada["valor"].AsDecimal }).ToArray(),
                Total = document["total"].AsDecimal,
                PosicaoDoDia = document["percentual"].AsDecimal,
            };
        }
    }
}
