using System;
using Topshelf;

namespace Nohros.Aion
{
  public class Service
  {
    readonly IdleTimeMonitor idle_time_monitor_;

    public Service(IdleTimeMonitor idle_time_monitor) {
      idle_time_monitor_ = idle_time_monitor;
    }

    public bool Start(HostControl host) {
      idle_time_monitor_.Start();
      return true;
    }

    public bool Stop(HostControl host) {
      idle_time_monitor_.Stop();
      return true;
    }
  }
}
