using System;
using Nohros.Data;

namespace Nohros.Aion.SqlCe
{
  public static class Mappers
  {
    const string kClassName = "Nohros.Aion.SqlCe.Mappers";

    public static IDataReaderMapper<IdleTimeInfo> IdleTimeInfo() {
      return new DataReaderMapperBuilder<IdleTimeInfo>(kClassName)
        .AutoMap()
        .Build();
    }
  }
}
