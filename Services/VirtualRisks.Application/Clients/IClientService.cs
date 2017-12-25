namespace CastleGo.Application.Clients
{
    public interface IClientService
    {
        ClientDto FindClient(string clientId);
    }
}
