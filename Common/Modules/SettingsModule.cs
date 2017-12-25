using System.Collections.Specialized;
using Autofac;

namespace CastleGo.Common.Modules
{
    public class SettingsModule : Module
    {
        private readonly NameValueCollection _settings;

        public SettingsModule(NameValueCollection settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleSettingsReader>()
                .As<ISettingsReader>()
                .WithParameter(TypedParameter.From(_settings));

            builder.RegisterSource(new SettingsSource());
        }
    }
}