using System;
using System.Configuration;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using Nohros.Extensions;

namespace Nohros.Aion
{
  public class App : INeedInitialization
  {
    public void Init() {
      string config_file_name = ConfigurationManager
        .AppSettings[Strings.kConfigFileNameKey];

      Settings settings = config_file_name == null
        ? new Settings.Builder().Build()
        : new Settings.Loader().Load(config_file_name, "aion");

      IConfigureComponents configurer =
        Configure
          .Instance
          .Configurer
          .RegisterSingleton<Settings>(settings);

      foreach (object provider in settings.CreateProviders()) {
        Type type = provider.GetType();
        Type[] interfaces = type.GetInterfaces();
        foreach (var @interface in interfaces) {
          configurer.RegisterSingleton(@interface, provider);
        }
        configurer.RegisterSingleton(type, provider);
      }
    }
  }
}