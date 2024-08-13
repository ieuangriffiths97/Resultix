using System;
using System.Threading.Tasks;

namespace Resultix
{
    internal interface IResult<TResult, TError>
    {
        void Match(Action<TResult> onSuccess, Action<TError> onError, Action onNone);
        TReturn Match<TReturn>(Func<TResult, TReturn> onSuccess, Func<TError, TReturn> onError, Func<TReturn> onNone);
        Task MatchAsync(Func<TResult, Task> onSuccess, Func<TError, Task> onError, Func<Task> onNone);
        Task<TReturn> MatchAsync<TReturn>(Func<TResult, Task<TReturn>> onSuccess, Func<TError, Task<TReturn>> onError, Func<Task<TReturn>> onNone);
    }
}