using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nohros.Concurrent;
using Nohros.Extensions;
using Nohros.Resources;

namespace Nohros.Aion
{
  public class IdleTimeProcessor : IProcessIdleTime
  {
    const string kClassName = "Nohros.Aion.IdleTimeProcessor";

    readonly IIdleTimeQueue idle_time_queue_;
    readonly Mailbox<RunnableDelegate> mailbox_;
    readonly AionLogger logger_;
    readonly ISendIdleTimeToServer message_sender_;

    public IdleTimeProcessor(IIdleTimeQueue idle_time_queue,
      ISendIdleTimeToServer sender) {
      idle_time_queue_ = idle_time_queue;
      mailbox_ = new Mailbox<RunnableDelegate>(OnSend,
        Executors.ThreadPoolExecutor());
      logger_ = AionLogger.ForCurrentProcess;
      message_sender_ = sender;
    }

    void OnSend(RunnableDelegate runnable) {
      runnable();
    }

    public void Process(IdleTimeInfo idle_time_info) {
      // The Send method run asynchrously and the idle_time_info could be
      // modified in the time between the Send operation and its execution. To
      // prevent this we enqueue a deep copy of the idle_time.
      var info = new IdleTimeInfo {
        LastInputTime = idle_time_info.LastInputTime,
        Timestamp = idle_time_info.Timestamp,
        MachineUpTime = idle_time_info.MachineUpTime
      };

      mailbox_.Send(() => Send(info));
    }

    void Send(IdleTimeInfo idle_time_info) {
      try {
        idle_time_queue_.Enqueue(idle_time_info);

        while (idle_time_queue_.Pick(out idle_time_info)) {
          message_sender_.Send(idle_time_info);
          idle_time_queue_.Remove(idle_time_info.Timestamp);
        }
      } catch (Exception e) {
        logger_.Error(
          StringResources.Log_MethodThrowsException.Fmt("Send", kClassName), e);
      }
    }
  }
}
