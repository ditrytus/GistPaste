using System;

namespace GistPaste.Desktop
{
    class LowLevelKeyboardHook : IObservable<LowLevelKeyboardMessage>
    {
        public IDisposable Subscribe(IObserver<LowLevelKeyboardMessage> observer)
        {
            return new LowLevelKeyboardHookSubscription(observer);
        }
    }
}
