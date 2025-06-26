using EaFramework.Driver;

namespace EaFramework.Config;

public class TestSettings
{
    public BrowserType BrowserType { get; set; }
    public Uri ApplicationUrl { get; set; }
    public float? TimeoutInterval { get; set; }
    public TestRunType TestRunType { get; set; }
    public Uri GridUri { get; set; }
}

public enum TestRunType
{
    Local,
    Grid
}