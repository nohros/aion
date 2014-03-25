using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using Nohros.Data;
using Nohros.Data.SqlCe;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Aion.SqlCe
{
  public class AddIdleTimeQuery
  {
    const string kClassName = "Nohros.Aion.SqlCe.AddIdleTimeQuery";
    const string kExecute = @"
insert into aion_idle_time(Timestamp, MachineUpTime, LastInputTime)
values(@timestamp, @machineUpTime, @lastInputTime)
";

    readonly AionLogger logger_ = AionLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="AddIdleTimeQuery"/>
    /// using the given <param ref="sql_connection_provider" />
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlCeConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public AddIdleTimeQuery(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = AionLogger.ForCurrentProcess;
    }
    #endregion

    public void Execute(IdleTimeInfo idle_time_info) {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(kExecute)
          .SetType(CommandType.Text)
          .AddParameter("@lastInputTime", idle_time_info.LastInputTime)
          .AddParameter("@machineUpTime", idle_time_info.MachineUpTime)
          .AddParameter("@timestamp", idle_time_info.Timestamp)
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
