using System;
using Nohros.Logging;

namespace Nohros.Aion
{
  public class AionLogger : ForwardingLogger
  {
    static readonly AionLogger current_process_logger_;

    static AionLogger() {
      current_process_logger_ = new AionLogger(new NOPLogger());
    }

    public AionLogger(ILogger logger) : base(logger) {
    }

    public static AionLogger ForCurrentProcess {
      get { return current_process_logger_; }
    }
  }
}
