using System;

namespace Nohros.Aion.Client.Data
{
  public interface IQueueIdleTime
  {
    /// <summary>
    /// Adds the given <see cref="IdleTimeDto"/> to the queue.
    /// </summary>
    /// <param name="idle_time">
    /// The <see cref="IdleTimeDto"/> to be queued.
    /// </param>
    /// <remarks>
    /// If a idle time object is already in the queue this method
    /// fails silently.The <seealso cref="IdleTimeDto.Timestamp"/> is used as
    /// the idle time unique identifier.
    /// </remarks>
    void Enqueue(IdleTimeDto idle_time);

    /// <summary>
    /// Gets the next available <seealso cref="IdleTimeDto"/> from the queue
    /// without removing it.
    /// </summary>
    IdleTimeDto Pick();

    /// <summary>
    /// Removes the idle time that is associated with the given timestamp.
    /// </summary>
    /// <param name="timestamp"></param>
    void Remove(long timestamp);
  }
}
