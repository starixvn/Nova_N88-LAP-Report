using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Common.Helpers
{
    public static class LockHelper
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _lockAsyncObjects = new ConcurrentDictionary<string, SemaphoreSlim>();

        public static async Task<TResult> LockAsync<TResult>(string key, Func<Task<TResult>> doing, int concurrent = 1)
           where TResult : class
        {
            var lockAsyncObject = _lockAsyncObjects.GetOrAdd(key, _ => new SemaphoreSlim(concurrent));
            await lockAsyncObject.WaitAsync();

            try
            {
                var result = await doing?.Invoke();
                return result;
            }
            finally
            {
                lockAsyncObject.Release();
            }
        }
    }
}