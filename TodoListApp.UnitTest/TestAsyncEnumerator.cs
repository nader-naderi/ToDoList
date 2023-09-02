namespace TodoListApp.UnitTest
{
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _innerEnumerator;

        public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
        {
            _innerEnumerator = innerEnumerator;
        }

        public T Current => _innerEnumerator.Current;

        public async ValueTask<bool> MoveNextAsync()
        {
            await Task.Delay(1); // Simulate asynchronous operation
            return _innerEnumerator.MoveNext();
        }

        public ValueTask DisposeAsync()
        {
            _innerEnumerator.Dispose();
            return new ValueTask();
        }
    }
}
