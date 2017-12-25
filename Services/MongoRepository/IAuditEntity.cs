using System;
using System.Runtime.Serialization;

namespace MongoRepository
{
    public interface IAuditEntity
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
        CreatorRef CreatedBy { get; set; }
        CreatorRef UpdatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class CreatorRef
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string ScreenName { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
    }
}
