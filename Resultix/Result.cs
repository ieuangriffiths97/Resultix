using System;
using System.Threading.Tasks;
using Resultix.Internal;

namespace Resultix
{
    public struct Result<TResult, TError> : IResult<TResult, TError>
    {
        private Internal.Result<TResult, TError> _inner;

        private Result(TResult result)
            => _inner = new Internal.Result<TResult, TError>(result);

        private Result(TError error)
            => _inner = new Internal.Result<TResult, TError>(error);

        private Internal.Result<TResult, TError> Inner
            => _inner ?? (_inner = new Internal.Result<TResult, TError>(default(TResult)));

        public void Match(Action<TResult> onSuccess, Action<TError> onError, Action onNone)
            => Inner.Match(onSuccess, onError, onNone);

        public TReturn Match<TReturn>(Func<TResult, TReturn> onSuccess, Func<TError, TReturn> onError, Func<TReturn> onNone)
            => Inner.Match(onSuccess, onError, onNone);

        public Task MatchAsync(Func<TResult, Task> onSuccess, Func<TError, Task> onError, Func<Task> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        public Task<TReturn> MatchAsync<TReturn>(Func<TResult, Task<TReturn>> onSuccess, Func<TError, Task<TReturn>> onError, Func<Task<TReturn>> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        public static implicit operator Result<TResult, TError>(TResult result)
            => new Result<TResult, TError>(result);

        public static implicit operator Result<TResult, TError>(TError error)
            => new Result<TResult, TError>(error);
    }

    public struct Result<TResult> : IResult<TResult, Exception>
    {
        private Internal.Result<TResult> _inner;

        private Result(TResult result)
            => _inner = new Internal.Result<TResult>(result);

        private Result(Exception error)
            => _inner = new Internal.Result<TResult>(error);

        private Internal.Result<TResult> Inner
            => _inner ?? (_inner = new Internal.Result<TResult>(default(TResult)));

        public void Match(Action<TResult> onSuccess, Action<Exception> onError, Action onNone)
            => Inner.Match(onSuccess, onError, onNone);

        public TReturn Match<TReturn>(Func<TResult, TReturn> onSuccess, Func<Exception, TReturn> onError, Func<TReturn> onNone)
            => Inner.Match(onSuccess, onError, onNone);

        public Task MatchAsync(Func<TResult, Task> onSuccess, Func<Exception, Task> onError, Func<Task> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        public Task<TReturn> MatchAsync<TReturn>(Func<TResult, Task<TReturn>> onSuccess, Func<Exception, Task<TReturn>> onError, Func<Task<TReturn>> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        public static implicit operator Result<TResult>(TResult result)
            => new Result<TResult>(result);

        public static implicit operator Result<TResult>(Exception error)
            => new Result<TResult>(error);
    }
}
