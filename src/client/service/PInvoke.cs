using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Nohros.Aion
{
  internal static class Environment
  {
    [StructLayout(LayoutKind.Sequential)]
    struct LastInputInfo
    {
      public static readonly int SizeOf = Marshal.SizeOf(typeof (LastInputInfo));

      [MarshalAs(UnmanagedType.U4)] public UInt32 cbSize;
      [MarshalAs(UnmanagedType.U4)] public UInt32 dwTime;
    }

    [DllImport("user32.dll")]
    static extern bool GetLastInputInfo(ref LastInputInfo plii);

    /// <summary>
    /// Gets time in seconds since the last user input.
    /// </summary>
    /// <returns>
    /// The time in seconds since the last user input.
    /// </returns>
    public static IdleTimeInfo GetSystemIdleTime() {
      var last_input_info = new LastInputInfo();
      last_input_info.cbSize = (uint) Marshal.SizeOf(last_input_info);
      last_input_info.dwTime = 0;

      uint last_input_tick = 0;
      var env_ticks = (uint) System.Environment.TickCount;
      if (GetLastInputInfo(ref last_input_info)) {
        last_input_tick = last_input_info.dwTime;
      }
      return new IdleTimeInfo(DateTime.Now, last_input_tick, env_ticks);
    }
  }
}
