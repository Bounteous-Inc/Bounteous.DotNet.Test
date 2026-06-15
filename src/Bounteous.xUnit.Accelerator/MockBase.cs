using System;
using Moq;

namespace Bounteous.xUnit.Accelerator
{
    public abstract class MockBase : IDisposable
    {
        private readonly MockRepository mocks;

        protected MockBase()
            => mocks = new MockRepository(MockBehavior.Strict);

        protected Mock<T> Create<T>(MockBehavior behavior = MockBehavior.Strict) where T : class
            => mocks.Create<T>(behavior);

        protected Mock<T> Strict<T>() where T : class
            => mocks.Create<T>(MockBehavior.Strict);

        protected Mock<T> Loose<T>() where T : class
            => mocks.Create<T>(MockBehavior.Loose);

        protected Mock<T> StrictPartial<T>(params object[] args) where T : class
            => mocks.Create<T>(MockBehavior.Strict, args);

        protected Mock<T> LoosePartial<T>(params object[] args) where T : class
            => mocks.Create<T>(MockBehavior.Loose, args);

        public virtual void Dispose() => mocks.VerifyAll();
    }
}