using FakeItEasy;
using GistPaste.Desktop.WindowsHooks;
using System;
using Xunit;

namespace GistPaste.Desktop.UnitTests.WindowsHooks
{
    public class LowLevelKeyboardHookTests
    {
        [Fact]
        public void Constructor_DoesntCreateAHook()
        {
            var user32 = A.Fake<IUser32>();
            var kernel32 = A.Fake<IKernel32>();

            var sut = new LowLevelKeyboardHook(user32, kernel32);

            A.CallTo(user32).MustNotHaveHappened();
            A.CallTo(kernel32).MustNotHaveHappened();
        }

        [Fact]
        public void Subscribe_First_CreatesHook()
        {
            var user32 = A.Fake<IUser32>();
            var kernel32 = A.Fake<IKernel32>();
            var observer = A.Fake<IObserver<LowLevelKeyboardMessage>>();
            
            var sut = new LowLevelKeyboardHook(user32, kernel32);

            sut.Subscribe(observer);

            A.CallTo(() => user32.SetWindowsHookEx(A<int>._, A<LowLevelKeyboardProc>._, A<IntPtr>._, A<uint>._))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Subscribe_Multiple_CreatesOnlyOneHook()
        {
            var user32 = A.Fake<IUser32>();
            var kernel32 = A.Fake<IKernel32>();
            var observer = A.Fake<IObserver<LowLevelKeyboardMessage>>();

            var sut = new LowLevelKeyboardHook(user32, kernel32);

            sut.Subscribe(observer);
            sut.Subscribe(observer);
            sut.Subscribe(observer);

            A.CallTo(() => user32.SetWindowsHookEx(A<int>._, A<LowLevelKeyboardProc>._, A<IntPtr>._, A<uint>._))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Unsubscribe_Last_RemovesHook()
        {
            var user32 = A.Fake<IUser32>();
            var kernel32 = A.Fake<IKernel32>();
            var observer = A.Fake<IObserver<LowLevelKeyboardMessage>>();

            var sut = new LowLevelKeyboardHook(user32, kernel32);

            var subsciption = sut.Subscribe(observer);
            subsciption.Dispose();

            A.CallTo(() => user32.UnhookWindowsHookEx(A<IntPtr>._))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
        [Fact]
        public void Unsubscribe_NotLast_DoesntRemoveHook()
        {
            var user32 = A.Fake<IUser32>();
            var kernel32 = A.Fake<IKernel32>();
            var observer = A.Fake<IObserver<LowLevelKeyboardMessage>>();

            var sut = new LowLevelKeyboardHook(user32, kernel32);

            sut.Subscribe(observer);
            var subsciption = sut.Subscribe(observer);
            subsciption.Dispose();

            A.CallTo(() => user32.UnhookWindowsHookEx(A<IntPtr>._))
                .MustNotHaveHappened();
        }
    }
}
