using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Contas
{
    public class ContasMongoDBService :
        IRequestHandler<SolicitacaoDeConsultaDeContas, Conta[]>
    {
        private readonly IMongoCollection<BsonDocument> collection;

        public ContasMongoDBService()
        {
            var client = new MongoClient("mongodb://banco_analitico:27017");

            var database = client.GetDatabase("FluxoDeCaixa");

            collection = database.GetCollection<BsonDocument>("contas");

            //

            var conta_id = ObjectId.Parse("000000000000000000000001");

            var conta = new Conta { Id = conta_id.ToString(), Numero = "123", Banco = "001", Tipo = TipoDeConta.Corrente, Documento = "096", Email = "mardsystems@gmail.com" };

            var document = new BsonDocument
            {
                {"_id", conta.Id },
                {"numero", conta.Numero },
                {"banco", conta.Banco },
                {"tipo", conta.Tipo },
                {"documento", conta.Documento },
                {"email", conta.Email },
                //{"saldo", conta.Saldo },
            };

            try
            {
                collection.InsertOne(document);
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
            }
        }

        public async Task<Conta[]> ConsultaContas()
        {
            var documents = await collection.Find(new BsonDocument()).ToListAsync();

            var contas = documents.Select(document => MapFrom(document)).ToArray();

            return contas;
        }

        //public async Task<Conta> ObtemConta(string numeroDaConta, string numeroDoBanco, TipoDeConta tipoDeConta, string numeroDoCpfOuCnpj)
        //{
        //    var filter =
        //        Builders<BsonDocument>.Filter.Eq("numero", numeroDaConta) &
        //        Builders<BsonDocument>.Filter.Eq("banco", numeroDoBanco) &
        //        Builders<BsonDocument>.Filter.Eq("tipo", tipoDeConta) &
        //        Builders<BsonDocument>.Filter.Eq("documento", numeroDoCpfOuCnpj);

        //    var document = await collection.Find(filter).SingleOrDefaultAsync();

        //    if (document == default)
        //    {
        //        throw new ApplicationException("Conta não encontrada.");
        //    }

        //    var conta = MapFrom(document);

        //    return conta;
        //}

        private static Conta MapFrom(BsonDocument document)
        {
            return new Conta
            {
                Id = document["_id"].AsString,
                Numero = document["numero"].AsString,
                Banco = document["banco"].AsString,
                Tipo = (TipoDeConta)document["tipo"].AsInt32,
                Documento = document["documento"].AsString,
                Email = document["email"].AsString,
                //Saldo = document["saldo"].AsDecimal,
            };
        }

        public async Task<Conta[]> Handle(SolicitacaoDeConsultaDeContas request, CancellationToken cancellationToken)
        {
            var documents = await collection.Find(new BsonDocument()).ToListAsync();

            var contas = documents.Select(document => MapFrom(document)).ToArray();

            return contas;
        }
    }
}
