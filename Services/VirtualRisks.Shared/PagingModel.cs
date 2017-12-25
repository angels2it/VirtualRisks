// Decompiled with JetBrains decompiler
// Type: CastleGo.Shared.PagingModel
// Assembly: CastleGo.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 157A9D10-4624-400D-A7F3-8771FF84E829
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Application.dll

using Newtonsoft.Json;

namespace CastleGo.Shared
{
    public class PagingModel
    {
        public int Page { get; set; }

        public int Take { get; set; }

        [JsonIgnore]
        public int Skip => Page * Take;

        public string Query { get; set; }
    }

    public class PagingByIdModel : PagingModel
    {
        public string Id { get; set; }
    }
}
