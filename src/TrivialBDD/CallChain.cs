using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TrivialBDD.Interfaces;

namespace TrivialBDD
{
    public class CallChain : ICallChain
    {
        private readonly List<CallInfo> calls = new List<CallInfo>();

        private Action<CallInfo> callback;

        private int callIndex = 0;

        private IDateTime dateTime;

        private CallChain(CallInfo initialCallStart, CallInfo initialCallEnd, Action<CallInfo> callback, IDateTime dateTime = null)
        {
            this.dateTime = dateTime ?? new Interfaces.DateTime();
            this.callback = callback;
            calls.Add(initialCallStart);
            calls.Add(initialCallEnd);
        }

        public CallInfo Current => calls[callIndex];

        object IEnumerator.Current => calls[callIndex];

        public TimeSpan? TotalElapsedTime { get => calls.Max(c => c.Start) - calls.Min(c => c.End); }

        public static CallChain Given(Action action, Action<CallInfo> callback = null)
        {
            var dateTime = new Interfaces.DateTime();
            return Given(action, callback, dateTime);
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

        internal static CallChain Given(Action action, Action<CallInfo> callback = null, IDateTime dateTime = null)
        {
            var sanitisedCallback = callback ?? ((ci) => { });
            var startAndEnd = CallTo(action, sanitisedCallback, dateTime);
            return new CallChain(startAndEnd.Item1, startAndEnd.Item2, sanitisedCallback, dateTime);
        }

        private static (CallInfo, CallInfo) CallTo(Action action, Action<CallInfo> callback, IDateTime dateTime)
        {
            var start = new CallInfo(action);
            callback(start);
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            var end = new CallInfo(initialInfo: start, end: dateTime.UtcNow, duration: sw.Elapsed);
            callback(end);

            return (start, end);
        }

        private CallChain CallTo(Action action)
        {
            var startAndEnd = CallChain.CallTo(action, callback, dateTime);
            calls.Add(startAndEnd.Item1);
            calls.Add(startAndEnd.Item2);
            return this;
        }
    }
}
