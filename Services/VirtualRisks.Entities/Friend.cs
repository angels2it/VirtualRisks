﻿// Decompiled with JetBrains decompiler
// Type: CastleGo.Entities.Friend
// Assembly: CastleGo.Entities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F3E4C664-4D9D-4620-942E-2E9C80E1452B
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Entities.dll

using MongoRepository;

namespace CastleGo.Entities
{
  public class Friend : Entity
  {
    public string Name { get; set; }

    public string Avatar { get; set; }
  }
}
