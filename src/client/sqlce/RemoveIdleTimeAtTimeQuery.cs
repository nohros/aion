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
  public class RemoveIdleTimeAtTimeQuery
  {
    const string kClassName = "Nohros.Aion.SqlCe.RemoveIdleTimeAtTimeQuery";
    const string kExecute = @"
delete aion_idle_time
where timestamp = @timestamp";

    readonly AionLogger logger_ = AionLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveIdleTimeAtTimeQuery"/>
    /// using the given <param ref="sql_connection_provider" />
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlCeConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public RemoveIdleTimeAtTimeQuery(
      SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = AionLogger.ForCurrentProcess;
    }

    public void Execute(long timestamp) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(kExecute)
          .SetType(CommandType.Text)
          .AddParameter("@timestamp", timestamp)
          .Build();
        try {
          conn.Open();
          cmd.ExecuteNonQuery();
        } catch (SqlCeException e) {
          logger_.Error(
            StringResources.Log_MethodThrowsException.Fmt("Execute", kClassName),
            e);
          throw new ProviderException(e);
        }
      }
    }
  }
}
