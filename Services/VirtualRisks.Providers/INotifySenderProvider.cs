using System.Collections.Generic;
using System.Threading.Tasks;
using CastleGo.Application.Users;

namespace CastleGo.Providers
{
    public interface INotifySenderProvider
    {
        Task SendNotify(string device, string token, string id, string type, string icon, string title, string content);
        Task SendNotify(List<NotifyTokenDto> tokens, string id, string type, string icon, string title, string message);
    }
    public class DefaultNotifySenderProvider : INotifySenderProvider
    {
        public Task SendNotify(string device, string token, string id, string type, string icon, string title, string content)
        {
            // do nothing
            return Task.FromResult(true);
        }

        public Task SendNotify(List<NotifyTokenDto> tokens, string id, string type, string icon, string title, string message)
        {
            return Task.FromResult(true);
        }
    }
}
