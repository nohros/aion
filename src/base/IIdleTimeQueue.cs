using System;

namespace Nohros.Aion
{
  /// <summary>
  /// Defines teh method that is executed when an async operation is
  /// completed on a <seealso cref="IIdleTimeQueue"/>
  /// </summary>
  /// <param name="idle_time"></param>
  public delegate void IdleTimeQueueCallback(IdleTimeInfo idle_time);

  public interface IIdleTimeQueue
  {
    /// <summary>
    /// Adds the given <see cref="IdleTimeInfo"/> to the queue.
    /// </summary>
    /// <param name="idle_time">
    /// The <see cref="IdleTimeInfo"/> to be queued.
    /// </param>
    /// <remarks>
    /// If a idle time object is already in the queue this method
    /// fails silently.The <seealso cref="IdleTimeInfo.Timestamp"/> is used as
    /// the idle time unique identifier.
    /// </remarks>
    void Enqueue(IdleTimeInfo idle_time);

    /// <summary>
    /// Gets the next available <seealso cref="IdleTimeInfo"/> from the queue
    /// without removing it.
    /// </summary>
    bool Pick(out IdleTimeInfo idle_time_info);

    /// <summary>
    /// Removes the idle time that is associated with the given timestamp.
    /// </summary>
    /// <param name="timestamp"></param>
    void Remove(long timestamp);
  }
}
