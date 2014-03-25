using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nohros.Aion
{
  public interface IProcessIdleTime
  {
    void Process(IdleTimeInfo idle_time_info);
  }
}
