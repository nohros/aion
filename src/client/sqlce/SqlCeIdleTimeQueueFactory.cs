using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;

namespace Nohros.Aion.SqlCe
{
  public class SqlCeIdleTimeQueueFactory
  {
    public SqlCeIdleTimeQueue CreateIdleTimeQueue() {
      var builder =
        new SqlConnectionStringBuilder {
          DataSource = IO.Path.AbsoluteForCallingAssembly("aion.queue.db")
        };

      if (!File.Exists(builder.DataSource)) {
        new SqlCeEngine(builder.ConnectionString)
          .CreateDatabase();
      }

      var sql_ce_dao = new SqlCeDaoFactory()
        .CreateProvider(new Dictionary<string, string> {
          {"connectionString", builder.ConnectionString}
        });
      sql_ce_dao.Initialize();
      return new SqlCeIdleTimeQueue(sql_ce_dao);
    }
  }
}
