using System;
using System.Collections.Generic;

namespace TrivialBDD
{
    public interface ICallChain : IEnumerable<CallInfo>, IEnumerator<CallInfo>
    {
        TimeSpan? TotalElapsedTime { get; }

        ICallChain And(Action action);

        ICallChain Then(Action action);

        ICallChain When(Action action);
    }
}