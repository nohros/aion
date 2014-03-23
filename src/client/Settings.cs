using System;
using System.Collections.Generic;
using System.Data;
using Nohros.Configuration;
using Nohros.Configuration.Builders;

namespace Nohros.Aion
{
  public class Settings : Configuration
  {
    public Settings(Builder builder) : base(builder) {
    }

    public class Builder : AbstractConfigurationBuilder<Settings>
    {
      public Builder() {
      }

      public override Settings Build() {
        return new Settings(this);
      }
    }

    public class Loader : AbstractConfigurationLoader<Settings>
    {
      public Loader() : this(new Builder()) {
      }

      public Loader(Builder builder) : base(builder) {
      }
    }
  }
}
