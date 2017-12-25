// Decompiled with JetBrains decompiler
// Type: CastleGo.DataAccess.Models.PagingModel
// Assembly: CastleGo.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C835FEBF-C9B9-4D1F-8860-3FDF1F4179C3
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.DataAccess.dll

namespace CastleGo.DataAccess.Models
{
    public class PagingModel
    {
        public int Page { get; set; }

        public int Take { get; set; }

        public int Skip => Page * Take;

        public string Query { get; set; }

        public string SearchTextFormated
        {
            get
            {
                if (!string.IsNullOrEmpty(Query))
                    return Query.ToLower();
                return string.Empty;
            }
        }
    }

    public class PagingByIdModel : PagingModel
    {
        public string Id { get; set; }
    }
}
