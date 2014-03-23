using System;
using Nohros.Concurrent;

namespace Nohros.Aion
{
  /// <summary>
  /// Monitors the current logged in user idle time.
  /// </summary>
  public class IdleTimeMonitor
  {
    long idle_threshold_;
    NonReentrantSchedule scheduler_;
    IdleTimeInfo last_idle_time_;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdleTimeMonitor"/> class,
    /// using the default check interval and idle threshold.
    /// </summary>
    public IdleTimeMonitor() {
      CheckIntervalInSeconds = 60;
      idle_threshold_ = 60;
    }

    /// <summary>
    /// Starts collecting user idle time.
    /// </summary>
    /// <remarks></remarks>
    public void Start() {
      scheduler_ = NonReentrantSchedule
        .Every(TimeSpan.FromSeconds(CheckIntervalInSeconds));
      scheduler_
        .Runnable(CheckForIdleTime);
    }

    void CheckForIdleTime() {
      IdleTimeInfo idle_time = Environment.GetSystemIdleTime();
      if (idle_time.LastInputTime <= last_idle_time_.LastInputTime) {
        last_idle_time_.MachineUpTime = idle_time.MachineUpTime;
      }

      if (idle_time.Seconds > IdleThresholdInSeconds) {
      }
    }

    /// <summary>
    /// Stops the idle time collecting process.
    /// </summary>
    /// <remarks>
    /// This methods blocks waiting the theread that started the
    /// monitor to finish.
    /// </remarks>
    public void Stop() {
      if (scheduler_ != null) {
        scheduler_.Stop().WaitOne();
      }
    }

    /// <summary>
    /// Gets or sets the interval between time checks.
    /// </summary>
    /// <remarks>
    /// This property should be set before start the monitor.
    /// </remarks>
    public long CheckIntervalInSeconds { get; set; }

    /// <summary>
    /// Gets or sets the amount of time in seconds during which no 
    /// user activity is detected before a user is considered idle.
    /// </summary>
    /// <remarks>
    /// This value should be set before start the monitor.
    /// </remarks>
    public long IdleThresholdInSeconds {
      get { return idle_threshold_; }
      set {
        if (value < 0) {
          throw new ArgumentOutOfRangeException("value", value,
            "IdleThresholdInSeconds cannot be negative.");
        }

        idle_threshold_ = value;
      }
    }
  }
}
