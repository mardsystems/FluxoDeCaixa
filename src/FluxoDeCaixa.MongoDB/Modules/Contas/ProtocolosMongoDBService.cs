using MongoDB.Bson;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modules.Contas
{
    public class ProtocolosMongoDBService : IGeracaoDeProtocolos
    {
        public Task<Protocolo> GeraProtocolo()
        {
            var id = ObjectId.GenerateNewId();

            var value = id.ToString();

            var protocolo = new Protocolo(value);

            return Task.FromResult(protocolo);
        }
    }
}
