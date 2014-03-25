using System;
using System.Threading;
using Nohros.Concurrent;

namespace Nohros.Aion
{
  /// <summary>
  /// Monitors the current logged in user idle time.
  /// </summary>
  public class IdleTimeMonitor
  {
    long idle_threshold_;
    IdleTimeInfo last_idle_time_;
    readonly IProcessIdleTime idle_time_processor_;
    Thread monitor_thread_;
    readonly ManualResetEvent sync_;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdleTimeMonitor"/> class,
    /// using the default check interval and idle threshold.
    /// </summary>
    public IdleTimeMonitor(IProcessIdleTime idle_time_processor) {
      CheckIntervalInSeconds = 60;
      idle_threshold_ = 60;
      last_idle_time_ = Environment.GetSystemIdleTime();
      idle_time_processor_ = idle_time_processor;
      sync_ = new ManualResetEvent(false);
    }

    /// <summary>
    /// Starts collecting user idle time.
    /// </summary>
    /// <remarks></remarks>
    public void Start() {
      monitor_thread_ = new BackgroundThreadFactory()
        .CreateThread(Monitor);
      monitor_thread_.Start();
    }

    void Monitor() {
      long delay;
      do {

        IdleTimeInfo idle_time = Environment.GetSystemIdleTime();
        ProcessIdleTime(idle_time);

        delay = CheckIntervalInSeconds - idle_time.InSeconds();
        if (delay < 0) {
          delay = CheckIntervalInSeconds;
        }
      } while (!sync_.WaitOne(TimeSpan.FromSeconds(delay)));
    }

    void ProcessIdleTime(IdleTimeInfo idle_time) {
      if (idle_time.LastInputTime <= last_idle_time_.LastInputTime) {
        last_idle_time_.MachineUpTime = idle_time.MachineUpTime;
      }

      if (idle_time.InSeconds() > IdleThresholdInSeconds) {
        idle_time_processor_.Process(idle_time);
        last_idle_time_ = idle_time;
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
      if (monitor_thread_ != null) {
        sync_.Set();
        monitor_thread_.Join();
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
