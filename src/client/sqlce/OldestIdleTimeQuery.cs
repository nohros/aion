using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using Nohros.Data;
using Nohros.Data.SqlCe;
using Nohros.Resources;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Aion.SqlCe
{
  public class OldestIdleTimeQuery
  {
    const string kClassName = "Nohros.Aion.SqlCe.OldestIdleTimeQuery";
    const string kExecute = @"
select top 1 Timestamp
  ,MachineUpTime
  ,LastInputTime
from aion_idle_time
order by timestamp asc";

    readonly AionLogger logger_ = AionLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;
    readonly IDataReaderMapper<IdleTimeInfo> mapper_;

    /// <summary>
    /// Initializes a new instance of the <see cref="OldestIdleTimeQuery"/>
    /// using the given <param ref="sql_connection_provider" />
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlCeConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public OldestIdleTimeQuery(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = AionLogger.ForCurrentProcess;
      mapper_ = Mappers.IdleTimeInfo();
    }

    public bool Execute(out IdleTimeInfo idle_time_info) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(kExecute)
          .SetType(CommandType.Text)
          .Build();
        try {
          conn.Open();
          using (IDataReader reader = cmd.ExecuteReader()) {
            return mapper_.Map(reader, out idle_time_info);
          }
        } catch (SqlException e) {
          logger_.Error(
            StringResources.Log_MethodThrowsException.Fmt("Execute", kClassName),
            e);
          throw new ProviderException(e);
        }
      }
    }
  }
}
