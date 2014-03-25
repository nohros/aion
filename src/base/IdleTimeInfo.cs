using System;

namespace Nohros.Aion
{
  public class IdleTimeInfo
  {
    /// <summary>
    /// Gets the date and time when the <seealso cref="IdleTimeInfo"/> data
    /// was collected.
    /// </summary>
    public long Timestamp { get; set; }

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
  }
}
