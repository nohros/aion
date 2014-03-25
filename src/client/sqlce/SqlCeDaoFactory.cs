using System;
using System.Collections.Generic;
using Nohros.Data.SqlCe;
using Nohros.Providers;

namespace Nohros.Aion.SqlCe
{
  internal class SqlCeDaoFactory : IProviderFactory<SqlCeDao>
  {
    object IProviderFactory.CreateProvider(IDictionary<string, string> options) {
      return CreateProvider(options);
    }

    public SqlCeDao CreateProvider(IDictionary<string, string> options) {
      var factory = new SqlCeConnectionProviderFactory();
      var sql_connection_provider = factory
        .CreateProvider(options) as SqlCeConnectionProvider;
      return new SqlCeDao(sql_connection_provider);
    }
  }
}
