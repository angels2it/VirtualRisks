using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CastleGo.Application.Users;

namespace CastleGo.WebApi.Models
{
    internal class NotifyGameInviteModel
    {
        public UserDto CurrentUser { get; set; }
        public UserDto Opponent { get; set; }
    }
}