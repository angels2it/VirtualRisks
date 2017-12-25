
namespace CastleGo.Application.Clients
{
  public class ClientDto : BaseDto
  {
    public string ClientId { get; set; }

    public string Secret { get; set; }

    public string Name { get; set; }

    public ClientAppTypes ClientAppType { get; set; }

    public bool Active { get; set; }

    public int RefreshTokenLifeTime { get; set; }

    public string AllowedOrigin { get; set; }
  }
}
