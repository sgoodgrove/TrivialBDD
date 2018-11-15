using System;

namespace TrivialBDD
{
    public class CallInfo
    {
        public CallInfo(Action action)
        {
            this.MethodName = action.Method.Name;
            this.Start = DateTime.UtcNow;
        }

        public CallInfo(CallInfo initialInfo, string methodName = null, DateTime? start = null, DateTime? end = null, TimeSpan? duration = null)
        {
            this.Duration = initialInfo?.Duration ?? duration;
            this.End = initialInfo?.End ?? end;
            this.Start = initialInfo != null ? initialInfo.Start : (start.HasValue ? start.Value : DateTime.UtcNow);
            this.MethodName = initialInfo?.MethodName ?? methodName;
        }

        public TimeSpan? Duration { get; private set; }

        public DateTime? End { get; private set; }

        public string MethodName { get; private set; }

        public DateTime Start { get; private set; }
    }
}
