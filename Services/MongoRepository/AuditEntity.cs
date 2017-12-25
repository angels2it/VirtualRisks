using System;
using System.Runtime.Serialization;

namespace MongoRepository
{
     [Serializable]
     [DataContract]
    public class AuditEntity : Entity, IAuditEntity
    {
        [DataMember]
        public virtual DateTime Created { get; set; }
        [DataMember]
        public virtual DateTime Updated { get; set; }
        [DataMember]
        public virtual CreatorRef CreatedBy { get; set; }
        [DataMember]
        public virtual CreatorRef UpdatedBy { get; set; }        
    }
}
