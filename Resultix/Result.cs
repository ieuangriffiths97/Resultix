using System;
using System.Threading.Tasks;
using Resultix.Internal;

namespace Resultix
{
    /// <summary>
    /// Struct representing a result of an action
    /// </summary>
    /// <typeparam name="TResult">Type returned when this result is resolved as successful</typeparam>
    /// <typeparam name="TError">Type returned when this result is resolved as an error</typeparam>
    public struct Result<TResult, TError> : IResult<TResult, TError>
    {
        private Internal.Result<TResult, TError> _inner;

        private Result(TResult result)
            => _inner = new Internal.Result<TResult, TError>(result);

        private Result(TError error)
            => _inner = new Internal.Result<TResult, TError>(error);

        private Internal.Result<TResult, TError> Inner
            => _inner ?? (_inner = new Internal.Result<TResult, TError>(default(TResult)));

        /// <summary>
        /// Resolves a <see cref="Result{TResult, TError}"/>
        /// </summary>
        /// <param name="onSuccess">Action to perform when the result resolves as a success</param>
        /// <param name="onError">Action to perform when the result resolves as an error</param>
        /// <param name="onNone">Action to perform when the result resolves as none</param>
        public void Match(Action<TResult> onSuccess, Action<TError> onError, Action onNone)
            => Inner.Match(onSuccess, onError, onNone);

        /// <summary>
        /// <inheritdoc cref="Match(Action{TResult}, Action{TError}, Action)"/>
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="onSuccess">Function to perform when the result resolves as a success</param>
        /// <param name="onError">Function to perform when the result resolves as an error</param>
        /// <param name="onNone">Function to perform when the result resolves as none</param>
        /// <returns><typeparamref name="TReturn"/></returns>
        public TReturn Match<TReturn>(Func<TResult, TReturn> onSuccess, Func<TError, TReturn> onError, Func<TReturn> onNone)
            => Inner.Match(onSuccess, onError, onNone);

        /// <summary>
        /// <inheritdoc cref="Match(Action{TResult}, Action{TError}, Action)"/>
        /// </summary>
        /// <param name="onSuccess"><see cref="Task"/> to run when result resolves as a success</param>
        /// <param name="onError"><see cref="Task"/> to run when result resolves as an error</param>
        /// <param name="onNone"><see cref="Task"/> to run when result resolves as none</param>
        /// <returns><see cref="Task"/></returns>
        public Task MatchAsync(Func<TResult, Task> onSuccess, Func<TError, Task> onError, Func<Task> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        /// <inheritdoc cref="MatchAsync(Func{TResult, Task}, Func{TError, Task}, Func{Task})"/>
        public Task<TReturn> MatchAsync<TReturn>(Func<TResult, Task<TReturn>> onSuccess, Func<TError, Task<TReturn>> onError, Func<Task<TReturn>> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        /// <summary>
        /// implicit operator to allow conversion from <typeparamref name="TResult"/> to <see cref="Result{TResult, TError}"/>
        /// </summary>
        /// <param name="result">Instance of result object</param>
        public static implicit operator Result<TResult, TError>(TResult result)
            => new Result<TResult, TError>(result);

        /// <summary>
        /// implicit operator to allow conversion from <typeparamref name="TError"/> to <see cref="Result{TResult, TError}"/>
        /// </summary>
        /// <param name="error">Instance of error object</param>
        public static implicit operator Result<TResult, TError>(TError error)
            => new Result<TResult, TError>(error);
    }

    /// <inheritdoc cref="Result{TResult, TError}"/>
    public struct Result<TResult> : IResult<TResult, Exception>
    {
        private Internal.Result<TResult> _inner;

        private Result(TResult result)
            => _inner = new Internal.Result<TResult>(result);

        private Result(Exception error)
            => _inner = new Internal.Result<TResult>(error);

        private Internal.Result<TResult> Inner
            => _inner ?? (_inner = new Internal.Result<TResult>(default(TResult)));

        /// <inheritdoc cref="Result{TResult, TError}.Match(Action{TResult}, Action{TError}, Action)"/>
        public void Match(Action<TResult> onSuccess, Action<Exception> onError, Action onNone)
            => Inner.Match(onSuccess, onError, onNone);

        /// <inheritdoc cref="Result{TResult, TError}.Match{TReturn}(Func{TResult, TReturn}, Func{TError, TReturn}, Func{TReturn})"/>
        public TReturn Match<TReturn>(Func<TResult, TReturn> onSuccess, Func<Exception, TReturn> onError, Func<TReturn> onNone)
            => Inner.Match(onSuccess, onError, onNone);

        /// <inheritdoc cref="Result{TResult, TError}.MatchAsync(Func{TResult, Task}, Func{TError, Task}, Func{Task})"/>
        public Task MatchAsync(Func<TResult, Task> onSuccess, Func<Exception, Task> onError, Func<Task> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        /// <inheritdoc cref="MatchAsync{TReturn}(Func{TResult, Task{TReturn}}, Func{Exception, Task{TReturn}}, Func{Task{TReturn}})"/>
        public Task<TReturn> MatchAsync<TReturn>(Func<TResult, Task<TReturn>> onSuccess, Func<Exception, Task<TReturn>> onError, Func<Task<TReturn>> onNone)
            => Inner.MatchAsync(onSuccess, onError, onNone);

        /// <inheritdoc cref="Result{TResult, TError}.Result(TResult)"/>
        public static implicit operator Result<TResult>(TResult result)
            => new Result<TResult>(result);

        /// <inheritdoc cref="Result{TResult, TError}.Result(TError)"/>
        public static implicit operator Result<TResult>(Exception error)
            => new Result<TResult>(error);
    }
}
