using System;
using Nohros.Extensions.Time;

namespace Nohros.Aion
{
  public struct IdleTimeInfo
  {
    public IdleTimeInfo(DateTime timestamp, long last_input_time,
      long machine_up_time) : this() {
      Timestamp = timestamp;
      LastInputTime = last_input_time;
      MachineUpTime = machine_up_time;
    }

    /// <summary>
    /// Gets the date and time when the <seealso cref="IdleTimeInfo"/> data
    /// was collected.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets the time of the last user input event.
    /// </summary>
    public long LastInputTime { get; set; }

    /// <summary>
    /// Retrieves the number of milliseconds that have elapsed since the system
    /// was started right after the time when the
    /// <seealso cref="LastInputTime"/> was collected.
    /// </summary>
    public long MachineUpTime { get; set; }

    /// <summary>
    /// Gets the idle time represented by a <seealso cref="IdleTimeInfo"/>.
    /// </summary>
    /// <returns>
    /// The idle time represented by this instance in seconds.
    /// </returns>
    public long Seconds {
      get { return (MachineUpTime - LastInputTime).ToSeconds(TimeUnit.Milliseconds); }
    }
  }
}
