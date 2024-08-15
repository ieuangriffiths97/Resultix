# Resultix
 Stupidly simple result pattern for .NET

# Why use this library?
Many result pattern libraries have confusing configuration and lots of customisation options, which can be overwhelming. This library is for you if you want a barebones, easy to use and understand result pattern in .NET.

# Installation
Standard installation through NuGet.

NuGet CLI:
```
Install-Package Resultix
```

.NET core CLI
```
dotnet add package Resultix
```

# Quick start
## Returning a result from a function

You can use the out of the box `Result<T>` type, which by default has an error type of `System.Exception`. Or you can define your own error type, by using the generic overload `Result<TResult, TError>`.

Consider the following code. We have a simple class called `Movie` and a function `GetMovie()` which returns `Result<Movie>`.

``` csharp
using System;
using Resultix;

public class Movie
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class MovieService
{
    // demo class - inner workings omitted for brevity

    public Result<Movie> GetMovie(string movieName)
    {
        if (string.IsNullOrWhiteSpace(movieName))
            return new ArgumentException($"{nameof(movieName)} cannot be null or whitespace"); // notice Exception is being returned, not thrown

        var movie = _db.GetMovieByName(movieName);

        if (movie is null)
            return default;

        return movie;
    }
}
```

When returning types from a function with a signature of `Result`, implicit operators in the `Result` struct allow you to return the following types
+ Any instance of `TResult` - which indicates a success result
+ Any instance of `TError` or (`System.Exception` when using `Result<TResult>`) - which indicates a failure
+ `default` or `default(TResult)` - which indicates a "None" result.

## Resolving a result
Now you have a function returning a `Result`, you can call that function and perform different actions depending on the result. Consider the following code.

``` csharp
public IHttpActionResult GetMovie(string movieName)
{
    var result = GetMovie(movieName);

    return result.Match(movie => Ok(movie), // onSuccess
                        error => BadRequest(error.Message) // onError
                        () => NotFound()); //onNone
}
```

Calling the `Match()` function will call one of the 3 lambdas passed in, depending on the result. The first parameter, `onSuccess` gets executed when the `Result` is successful. The second parameter `onError` gets called when `TError` is returned from the `GetMovie` function. The 3rd and final parameter, `onNone` gets called when `default` is returned from `GetMovie()`.

The is an overload for `Match()` which takes in `System.Action` as the parameter types instead, for when you don't want to return a result from `Match()`. There are also async versions available, named `MatchAsync()`, which follow the same concept.

# Optional configuration options
There is currently only one configuration option. This property lives in the `ResultConfig` class and is called `EnforceMatches`. When set to true it will cause an exception to be thrown if null is passed in for any of the lambdas in the `Match()` and `MatchAsync()` functions. The default value, false, means that you can pass null into the `Match()` and `MatchAsync()` functions if you don't want to handle certain results. This setting was designed to be set once, at application start-up.