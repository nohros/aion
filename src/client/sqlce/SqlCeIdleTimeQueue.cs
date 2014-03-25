using System;

namespace Nohros.Aion.SqlCe
{
  public class SqlCeIdleTimeQueue : IIdleTimeQueue
  {
    readonly SqlCeDao dao_;

    public SqlCeIdleTimeQueue(SqlCeDao dao) {
      dao_ = dao;
    }

    public void Enqueue(IdleTimeInfo idle_time) {
      IdleTimeInfo already_existent_idle_time;
      if (!dao_.GetIdleTimeAtTime(idle_time.Timestamp,
        out already_existent_idle_time)) {
        dao_.AddIdleTime(idle_time);
      }
    }

    public bool Pick(out IdleTimeInfo idle_info_time) {
      return dao_.GetOldestIdleTime(out idle_info_time);
    }

    public void Remove(long timestamp) {
      dao_.RemoveIdleTimeAtTime(timestamp);
    }
  }
}
