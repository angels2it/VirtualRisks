using MongoRepository;
using System.Collections.Generic;
using CastleGo.Shared.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace CastleGo.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public List<CastleGo.Entities.Logins> Logins { get; set; }

        public string Avatar { get; set; }

        public List<Hero> Heroes { get; set; }

        public List<Friend> Friends { get; set; }
        public List<NotifyToken> Tokens { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class UnwindUserFriend
    {
        public string Id { get; set; }
        public Friend Friends { get; set; }
    }

    public class NotifyToken
    {
        public MobileDevice Device { get; set; }
        public string Token { get; set; }   
    }
}
