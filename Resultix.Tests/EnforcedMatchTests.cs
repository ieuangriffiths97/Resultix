using System;
using System.Threading.Tasks;

namespace Resultix.Tests;

[TestClass]
public class EnforcedMatchTests
{
    [ClassInitialize]
    public static void Init(TestContext ctx)
        => ResultConfig.EnforceMatches = true;

    #region config

    public void AssertConfig()
        => Assert.IsTrue(ResultConfig.EnforceMatches);

    #endregion

    #region match void tests

    [TestMethod]
    public void Match_Success_NotImplemented()
    {
        const int num = 1234;
        Result<int> result = num;

        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            result.Match(null,
                         _ => throw new NotImplementedException(),
                         () => throw new NotImplementedException());
        });
    }

    [TestMethod]
    public void Match_Error_NotImplemented()
    {
        var ex = new Exception("Test");
        Result<int> result = ex;

        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            result.Match(_ => throw new NotImplementedException(),
                         null,
                         () => throw new NotImplementedException());
        });
    }

    [TestMethod]
    public void Match_None_NotImplemented()
    {
        Result<int?> result = default(int);

        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            result.Match(_ => throw new NotImplementedException(),
                         _ => throw new NotImplementedException(),
                         null);
        });
    }

    [TestMethod]
    public void Match_None_Default()
    {
        Result<string> result = default;
        var wasCalled = false;

        result.Match(_ => throw new NotImplementedException(),
                     _ => throw new NotImplementedException(),
                     () => wasCalled = true);

        Assert.IsTrue(wasCalled);
    }

    [TestMethod]
    public async Task MatchAsync_None_Default()
    {
        Result<string> result = default;

        var resolved = await result.MatchAsync(_ => Task.Run(() => ThrowExBool()),
                                               _ => Task.Run(() => ThrowExBool()),
                                              () => Task.Run(() => true));

        Assert.IsTrue(resolved);
    }

    [TestMethod]
    public async Task MatchAsync_None_Default_NoReturn()
    {
        Result<string> result = default;
        var wasCalled = false;

        await result.MatchAsync(_ => Task.Run(() => throw new NotImplementedException()),
                                _ => Task.Run(() => throw new NotImplementedException()),
                                () => Task.Run(() => wasCalled = true));

        Assert.IsTrue(wasCalled);
        
    }

    #endregion

    private bool ThrowExBool()
        => throw new NotImplementedException();
}