using System;

namespace Nohros.Aion
{
  public class IdleTimeGateway : ISendIdleTimeToServer
  {
    public void Send(IdleTimeInfo idle_time_info) {
      Console.WriteLine(idle_time_info.InSeconds());
    }
  }
}
