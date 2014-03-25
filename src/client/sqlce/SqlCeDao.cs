using System;
using Nohros.Data.SqlCe;

namespace Nohros.Aion.SqlCe
{
  public class SqlCeDao
  {
    readonly SqlCeConnectionProvider sql_ce_connection_provider_;

    public SqlCeDao(SqlCeConnectionProvider sql_ce_connection_provider) {
      sql_ce_connection_provider_ = sql_ce_connection_provider;
    }

    public void Initialize() {
      var table_exists = new TableExistsQuery(sql_ce_connection_provider_);
      if (!table_exists.Execute("aion_idle_time")) {
        new CreateIdleTimeTable(sql_ce_connection_provider_).Execute();
      }
    }

    public void AddIdleTime(IdleTimeInfo idle_time_info) {
      new AddIdleTimeQuery(sql_ce_connection_provider_)
        .Execute(idle_time_info);
    }

    public void RemoveIdleTimeAtTime(long timestamp) {
      new RemoveIdleTimeAtTimeQuery(sql_ce_connection_provider_)
        .Execute(timestamp);
    }

    public bool GetIdleTimeAtTime(long timestamp, out IdleTimeInfo idle_time) {
      return new IdleTimeAtTimeQuery(sql_ce_connection_provider_)
        .Execute(timestamp, out idle_time);
    }

    public bool GetOldestIdleTime(out IdleTimeInfo idle_time) {
      return new OldestIdleTimeQuery(sql_ce_connection_provider_)
        .Execute(out idle_time);
    }
  }
}
