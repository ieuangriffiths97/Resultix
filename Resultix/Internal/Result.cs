using System;
using System.Threading.Tasks;

namespace Resultix.Internal
{
    internal class Result<TResult, TError> : IResult<TResult, TError>
    {
        private readonly TResult _result;
        private readonly TError _error;
        private readonly bool _faulted;

        internal Result(TResult result)
        {
            _result = result;
            _error = default;
            _faulted = false;
        }

        internal Result(TError error)
        {
            _error = error;
            _result = default;
            _faulted = true;
        }

        public void Match(Action<TResult> onSuccess, Action<TError> onError, Action onNone)
        {
            GuardDelegates(onSuccess, onError, onNone);

            if (_faulted)
                onError?.Invoke(_error);
            else if (_result != null)
                onSuccess?.Invoke(_result);
            else
                onNone?.Invoke();
        }

        public TReturn Match<TReturn>(Func<TResult, TReturn> onSuccess, Func<TError, TReturn> onError, Func<TReturn> onNone)
        {
            GuardDelegates(onSuccess, onError, onNone);

            if (_faulted)
                return onError is null ? default : onError(_error);
            else if (_result != null)
                return onSuccess is null ? default : onSuccess(_result);

            return onNone is null ? default : onNone();
        }

        public Task MatchAsync(Func<TResult, Task> onSuccess, Func<TError, Task> onError, Func<Task> onNone)
            => MatchAsync<Task>(onSuccess, onError, onNone) ?? Task.CompletedTask;

        public Task<TReturn> MatchAsync<TReturn>(Func<TResult, Task<TReturn>> onSuccess, Func<TError, Task<TReturn>> onError, Func<Task<TReturn>> onNone)
            => MatchAsync<Task<TReturn>>(onSuccess, onError, onNone) ?? Task.FromResult(default(TReturn));

        private TTask MatchAsync<TTask>(Func<TResult, TTask> onSuccess, Func<TError, TTask> onError, Func<TTask> onNone) where TTask : Task
        {
            GuardDelegates(onSuccess, onError, onNone);

            if (_faulted)
            {
                if (onError != null)
                    return onError(_error);
            }
            else if (_result != null)
            {
                if (onSuccess != null)
                    return onSuccess(_result);
            }
            else if (onNone != null)
                return onNone();

            return null;
        }

        private void GuardDelegates(Delegate onSuccess, Delegate onFailure, Delegate onNone)
        {
            if (ResultConfig.EnforceMatches)
            {
                if (onSuccess is null)
                    throw new ArgumentNullException(nameof(onSuccess));

                if (onFailure is null)
                    throw new ArgumentNullException(nameof(onFailure));

                if (onNone is null)
                    throw new ArgumentNullException(nameof(onNone));
            }
        }
    }

    internal class Result<TResult> : Result<TResult, Exception>
    {
        internal Result(TResult result) : base(result) { }

		internal Result(Exception error) : base(error) { }
    }
}