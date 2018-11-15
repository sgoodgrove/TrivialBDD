using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TrivialBDD
{
    public class CallChain : ICallChain
    {
        private readonly List<CallInfo> calls = new List<CallInfo>();

        private Action<CallInfo> callback;

        private int callIndex = 0;

        private CallChain(CallInfo initialCallStart, CallInfo initialCallEnd, Action<CallInfo> callback)
        {
            this.callback = callback;
            calls.Add(initialCallStart);
            calls.Add(initialCallEnd);
        }

        public CallInfo Current => calls[callIndex];

        object IEnumerator.Current => calls[callIndex];

        public TimeSpan? TotalElapsedTime { get => calls.Max(c => c.Start) - calls.Min(c => c.End); }

        public static CallChain Given(Action action, Action<CallInfo> callback = null)
        {
            var sanitisedCallback = callback ?? ((ci) => { });
            var startAndEnd = CallTo(action, sanitisedCallback);
            return new CallChain(startAndEnd.Item1, startAndEnd.Item2, sanitisedCallback);
        }

        public ICallChain And(Action action) => CallTo(action);

        public void Dispose()
        {
        }

        public IEnumerator<CallInfo> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return calls.GetEnumerator();
        }

        public bool MoveNext()
        {
            if (callIndex + 1 < calls.Count)
            {
                callIndex++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            callIndex = 0;
        }

        public ICallChain Then(Action action) => CallTo(action);

        public ICallChain When(Action action) => CallTo(action);

        private static (CallInfo, CallInfo) CallTo(Action action, Action<CallInfo> callback)
        {
            var start = new CallInfo(action);
            callback(start);
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            var end = new CallInfo(initialInfo: start, end: DateTime.UtcNow, duration: sw.Elapsed);
            callback(end);

            return (start, end);
        }

        private CallChain CallTo(Action action)
        {
            var startAndEnd = CallChain.CallTo(action, callback);
            calls.Add(startAndEnd.Item1);
            calls.Add(startAndEnd.Item2);
            return this;
        }
    }
}
