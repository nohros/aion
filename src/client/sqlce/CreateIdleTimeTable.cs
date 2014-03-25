using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using Nohros.Data;
using Nohros.Data.SqlCe;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Aion.SqlCe
{
  public class CreateIdleTimeTable
  {
    const string kClassName = "Nohros.Aion.SqlCe.CreateIdleTimeTable";
    const string kExecute = @"
create table aion_idle_time (
  Timestamp bigint not null unique,
  MachineUpTime bigint not null,
  LastInputTime bigint not null
)";

    readonly AionLogger logger_ = AionLogger.ForCurrentProcess;
    readonly SqlCeConnectionProvider sql_connection_provider_;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateIdleTimeTable"/>
    /// using the given <param ref="sql_connection_provider" />
    /// </summary>
    /// <param name="sql_connection_provider">
    /// A <see cref="SqlCeConnectionProvider"/> object that can be used to
    /// create connections to the data provider.
    /// </param>
    public CreateIdleTimeTable(SqlCeConnectionProvider sql_connection_provider) {
      sql_connection_provider_ = sql_connection_provider;
      logger_ = AionLogger.ForCurrentProcess;
    }

    public void Execute() {
      using (SqlCeConnection conn = sql_connection_provider_.CreateConnection())
      using (var builder = new CommandBuilder(conn)) {
        IDbCommand cmd = builder
          .SetText(kExecute)
          .SetType(CommandType.Text)
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
