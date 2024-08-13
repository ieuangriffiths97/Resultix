using System;
using System.Threading.Tasks;

namespace Resultix.Tests;

[TestClass]
public class UnenforcedMatchTests
{
    [ClassInitialize]
    public static void Init(TestContext ctx)
        => ResultConfig.EnforceMatches = false;

    #region config

    public void AssertConfig()
        => Assert.IsFalse(ResultConfig.EnforceMatches);

    #endregion

    #region match void tests

    [TestMethod]
    public void MatchVoid_Success_NotImplemented()
    {
        Result<int> result = 1;

        result.Match(null,
                     _ => throw new Exception("onError should not be called"),
                     () => throw new Exception("onNone should not be called"));
    }

    [TestMethod]
    public void MatchVoid_Error_NotImplemented()
    {
        Result<string> result = new Exception("Test");

        result.Match(_ => throw new Exception("onSuccess should not be called"),
                     null,
                     () => throw new Exception("onNone should not be called"));
    }

    [TestMethod]
    public void MatchVoid_None_NotImplemented()
    {
        Result<string?> result = default(string);

        result.Match(_ => throw new Exception("onSuccess should not be called"),
                     _ => throw new Exception("onError should not be called"),
                     null);
    }

    [TestMethod]
    public void MatchVoid_Success_Implemented()
    {
        const int num = 1;
        Result<int> result = num;
        var wasCalled = false;

        result.Match(r =>
            {
                Assert.AreEqual(num, r);
                wasCalled = true;
            },
            ex => throw new Exception("onError should not be called"),
            () => throw new Exception("onNone should not be called"));

        Assert.IsTrue(wasCalled);
    }

    [TestMethod]
    public void MatchVoid_Error_Implemented()
    {
        var ex = new Exception("Test");
        Result<int> result = ex;
        var wasCalled = false;

        result.Match(_ => throw new Exception("onSuccess should not be called"),
                     error =>
                     {
                         Assert.AreEqual(ex, error);
                         wasCalled = true;
                     },
                     () => throw new Exception("onNone should not be called"));

        Assert.IsTrue(wasCalled);
    }

    [TestMethod]
    public void MatchVoid_None_Implemented()
    {
        Result<string?> result = default(string);

        var wasCalled = false;

        result.Match(_ => throw new Exception("onSuccess should not be called"),
                     _ => throw new Exception("onError should not be called"),
                     () => wasCalled = true);

        Assert.IsTrue(wasCalled);
    }

    #endregion

    #region async Task match tests

    [TestMethod]
    public async Task MatchTask_Success_NotImplemented()
    {
        const int num = 2;
        Result<int> result = num;

        await result.MatchAsync(null,
                                _ => Task.Run(() => throw new Exception("onError should not be called")),
                                () => Task.Run(() => throw new Exception("onNone should not be called")));
    }

    [TestMethod]
    public async Task MatchTask_Error_NotImplemented()
    {
        var ex = new Exception("Test");
        Result<int> result = ex;

        await result.MatchAsync(_ => Task.Run(() => throw new Exception("onSuccess should not be called")),
                                null,
                                () => Task.Run(() => throw new Exception("onNone should not be called")));
    }

    [TestMethod]
    public async Task MatchTask_None_NotImplemented()
    {
        Result<int?> result = default(int?);

        await result.MatchAsync(_ => Task.Run(() => throw new Exception("onSuccess should not be called")),
                                _ => Task.Run(() => throw new Exception("onError should not be called")),
                                null);
    }

    [TestMethod]
    public async Task MatchTask_Success_Implemented()
    {
        const int num = 1;
        Result<int> result = num;

        var resolved = await result.MatchAsync(r => Task.FromResult(r),
                                               _ => Task.Run(() => ThrowIntEx("onError should not be called")),
                                               () => Task.Run(() => ThrowIntEx("onNone should not be called")));

        Assert.AreEqual(num, resolved);
    }

    [TestMethod]
    public async Task MatchTask_Error_Implemented()
    {
        var ex = new Exception("Test");
        Result<int> result = ex;

        var resolved = await result.MatchAsync(_ => Task.Run(() => ThrowIntEx("onSuccess should not be called")),
                                               _ => Task.Run(() => 2),
                                               () => Task.Run(() => ThrowIntEx("onNone should not be called")));

        Assert.AreEqual(2, resolved);
    }

    [TestMethod]
    public async Task MatchTask_None_Implemented()
    {
        Result<string?> result = default(string);

        var resolved = await result.MatchAsync(_ => Task.Run(() => ThrowStringEx("onSuccess should not be called")),
                                               _ => Task.Run(() => ThrowStringEx("onError should not be called")),
                                               () => Task.Run(() => "Test!"));

        Assert.AreEqual("Test!", resolved);
    }

    #endregion


    private string ThrowStringEx(string message)
        => throw new Exception(message);

    private int ThrowIntEx(string message)
        => throw new Exception(message);
}