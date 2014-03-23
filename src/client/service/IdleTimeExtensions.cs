using System;
using Nohros.Aion.Client.Data;
using Nohros.Extensions.Time;

namespace Nohros.Aion
{
  public static class IdleTimeExtensions
  {
    /// <summary>
    /// Gets the idle time represented by a <seealso cref="IdleTimeDto"/> in
    /// seconds.
    /// </summary>
    /// <returns>
    /// The idle time represented by the <seealso cref="idle_time"/> in
    /// seconds.
    /// </returns>
    public static long InSeconds(this IdleTimeDto idle_time) {
      return (idle_time.MachineUpTime - idle_time.LastInputTime)
        .ToSeconds(TimeUnit.Milliseconds);
    }
  }
}
