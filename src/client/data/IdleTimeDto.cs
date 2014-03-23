using System;

namespace Nohros.Aion.Client.Data
{
  public class IdleTimeDto
  {
    /// <summary>
    /// Gets the date and time when the <seealso cref="IdleTimeDto"/> data
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
  }
}
