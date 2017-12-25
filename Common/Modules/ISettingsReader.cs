using System;

namespace CastleGo.Common.Modules
{
    public interface ISettingsReader
    {
        object Load(Type type);
        object Load(Type type, string prefix);
    }
}
