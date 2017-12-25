using System;
using System.Collections.Specialized;
using System.Linq;

namespace CastleGo.Common.Modules
{
    public class SimpleSettingsReader : ISettingsReader
    {
        private readonly NameValueCollection _settings;

        public SimpleSettingsReader(NameValueCollection settings)
        {
            _settings = settings;
        }

        public object Load(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var settingsPrefix = type.Name.Replace("Settings", "") + ":";
            return LoadSetting(type, settingsPrefix);
        }

        private object LoadSetting(Type type, string settingsPrefix)
        {
            var settingsObj = Activator.CreateInstance(type);
            foreach (var key in _settings.AllKeys.Where(x => x.StartsWith(settingsPrefix)))
            {
                var propertyName = key.Replace(settingsPrefix, "");
                var property = type.GetProperty(propertyName);
                if (property == null)
                    throw new Exception($"Settings class {type.Name} has no property called {propertyName}");
                var propertyValue = Convert.ChangeType(_settings[key], property.PropertyType);
                property.SetValue(settingsObj, propertyValue, null);
            }
            return settingsObj;
        }

        public object Load(Type type, string prefix)
        {
            return LoadSetting(type, prefix + ":");
        }
    }
}