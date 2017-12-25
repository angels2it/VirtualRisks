using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleGo.Providers
{
    public class NotifySettings
    {
        public string iOSCertificatePath { get; set; }
        public string iOSCertificatePassword { get; set; }
        public string AndroidSendId { get; set; }
        public string AndroidApplicationId { get; set; }
        public string GameInviteTitle { get; set; }
        public string GameInviteMessage { get; set; }
    }
}
