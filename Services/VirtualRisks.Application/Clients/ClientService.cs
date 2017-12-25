using AutoMapper;
using CastleGo.Entities;
using MongoRepository;
using System.Linq;

namespace CastleGo.Application.Clients
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _repository;

        public ClientService(IRepository<Client> repository)
        {
            this._repository = repository;
        }

        public ClientDto FindClient(string clientId)
        {
            Client client = _repository.FirstOrDefault<Client>(e => e.ClientId == clientId);
            if (client == null)
                return null;
            return Mapper.Map<ClientDto>(client);
        }
    }
}
