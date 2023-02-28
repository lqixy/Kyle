using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim)
        {
            await semaphoreSlim.WaitAsync();
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
        {
            await semaphoreSlim.WaitAsync(millisecondsTimeout);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(millisecondsTimeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
        {
            await semaphoreSlim.WaitAsync(timeout);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(timeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim)
        {
            semaphoreSlim.Wait();
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
        {
            semaphoreSlim.Wait(cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
        {
            semaphoreSlim.Wait(millisecondsTimeout);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            semaphoreSlim.Wait(millisecondsTimeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
        {
            semaphoreSlim.Wait(timeout);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
        {
            semaphoreSlim.Wait(timeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        private static IDisposable GetDispose(this SemaphoreSlim semaphoreSlim)
        {
            return new DisposeAction(() =>
            {
                semaphoreSlim.Release();
            });
        }
    }
    /// <summary>
    /// This class can be used to provide an action when
    /// Dipose method is called.
    /// </summary>
    public class DisposeAction : IDisposable
    {
        public static readonly DisposeAction Empty = new DisposeAction(null);

        private Action _action;

        /// <summary>
        /// Creates a new <see cref="DisposeAction"/> object.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed.</param>
        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            // Interlocked prevents multiple execution of the _action.
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }

}
