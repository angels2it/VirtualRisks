using MongoDB.Bson;
using System;
using System.Linq;

namespace CastleGo.WebApi.MapperProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public static class Test
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public static Guid AsGuid(this ObjectId oid)
        {
            return new Guid(oid.ToByteArray().Concat<byte>(new byte[4] { (byte)5, (byte)5, (byte)5, (byte)5 }).ToArray<byte>());
        }
    }
}
