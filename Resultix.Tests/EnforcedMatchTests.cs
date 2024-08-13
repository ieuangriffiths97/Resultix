using System;

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

    #endregion

    // todo - just need to do a set of tests for task.
}