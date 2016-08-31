using FakeItEasy;
using GistPaste.Desktop.WindowsHooks;
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
    }
}
