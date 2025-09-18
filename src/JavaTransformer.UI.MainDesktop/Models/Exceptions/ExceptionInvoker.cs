using JavaTransformer.UI.MainDesktop.Models.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JavaTransformer.UI.MainDesktop.Models.Exceptions
{
    public sealed class ExceptionInvoker
    {
        private static readonly object _syncLock = new object();
        private static Exception _lastException;

        public static Exception LastException
        {
            get
            {
                lock (_syncLock)
                {
                    return _lastException;
                }
            }
        }

        public static event EventHandler<Exception> ExceptionOccurred;

        public static bool TryInvoke<TResult>(Func<TResult> action, out TResult result, out Exception error)
        {
            result = default;
            error = null;

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                result = action();
                return true;
            }
            catch (Exception ex)
            {
                CaptureException(ex);
                error = ex;
                return false;
            }
        }

        public static bool TryInvoke<TException>(Action action, out TException error) where TException : Exception
        {
            error = null;

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                action();
                return true;
            }
            catch (TException ex)
            {
                CaptureException(ex);
                error = ex;
                return false;
            }
        }

        public static bool TryInvoke<TException>(Action action) where TException : Exception
        {
            return TryInvoke(action, out TException _);
        }

        public static TResult ExecuteWithRetry<TResult, TException>(
            Func<TResult> action,
            int maxRetries = 3,
            int retryDelay = 100)
            where TException : Exception
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var exceptions = new List<Exception>();

            for (int attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return action();
                }
                catch (TException ex)
                {
                    exceptions.Add(ex);
                    CaptureException(ex);

                    if (attempt == maxRetries)
                        break;

                    Thread.Sleep(retryDelay);
                }
            }

            throw new AggregateException(
                $"Execution failed after {maxRetries + 1} attempts",
                exceptions);
        }

        public static void ClearLastException()
        {
            lock (_syncLock)
            {
                _lastException = null;
            }
        }

        private static void CaptureException(Exception exception)
        {
            if (exception == null)
                return;

            lock (_syncLock)
            {
                _lastException = exception;
            }

            RaiseExceptionOccurred(exception);
        }

        private static void RaiseExceptionOccurred(Exception exception)
        {
            var handlers = ExceptionOccurred;
            handlers?.Invoke(null, exception);
        }
    }
}