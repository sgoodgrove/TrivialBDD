using System;

namespace TrivialBDD
{
    public static class Given
    {
        public static ICallChain That(Action action, Action<CallInfo> callback = null)
        {
            return CallChain.Given(action, callback);
        }
    }
}
