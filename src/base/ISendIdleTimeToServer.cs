using System;

namespace Nohros.Aion
{
  public interface ISendIdleTimeToServer
  {
    void Send(IdleTimeInfo idle_time_info);
  }
}
