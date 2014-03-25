using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Nohros.Aion.SqlCe;
using Nohros.Extensions;
using Nohros.Logging;
using Topshelf;
using Topshelf.Autofac;

namespace Nohros.Aion
{
  public static class App
  {
    public static void Main(string[] args) {
      string config_file_name = ConfigurationManager
        .AppSettings[Strings.kConfigFileNameKey];

      Settings settings = config_file_name == null
        ? new Settings.Builder().Build()
        : new Settings.Loader().Load(config_file_name, "aion");

      var builder = new ContainerBuilder();
      builder
        .RegisterInstance(settings)
        .SingleInstance();

      // register the default idle time queue implementation and its
      // dependencies.
      builder
        .RegisterInstance(new SqlCeIdleTimeQueueFactory().CreateIdleTimeQueue())
        .AsImplementedInterfaces()
        .SingleInstance();

      builder
        .RegisterType<IdleTimeMonitor>()
        .SingleInstance();

      builder
        .RegisterType<IdleTimeProcessor>()
        .AsImplementedInterfaces()
        .SingleInstance();

      builder
        .RegisterType<IdleTimeGateway>()
        .AsImplementedInterfaces();

      builder
        .RegisterType<Service>();

      foreach (object provider in settings.CreateProviders()) {
        builder
          .RegisterInstance(provider)
          .AsImplementedInterfaces()
          .SingleInstance();
      }

      IContainer container = builder.Build();
      HostFactory.Run(x => {
        x.UseAutofacContainer(container);

        x.Service<Service>(s => {
          s.ConstructUsingAutofacContainer();

          s.WhenStarted((service, host) => service.Start(host));
          s.WhenStopped((service, host) => service.Stop(host));
        });
      });
    }
  }
}
