﻿using EaFramework.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.Extensions;
using System.Reflection;

namespace EaFramework.Driver;

public class DriverFixture : IDriverFixture, IDisposable
{
    private readonly TestSettings _testSettings;

    public IWebDriver Driver { get; }
    
    public DriverFixture(TestSettings testSettings)
    {
        _testSettings = testSettings;
        Driver = _testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        Driver.Navigate().GoToUrl(_testSettings.ApplicationUrl);
    }


    private IWebDriver GetWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            BrowserType.Safari => new SafariDriver(),
            _ => new ChromeDriver()
        };
    }

    private IWebDriver GetRemoteWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions()),
            BrowserType.Firefox => new RemoteWebDriver(_testSettings.GridUri, new FirefoxOptions()),
            BrowserType.Safari => new RemoteWebDriver(_testSettings.GridUri, new SafariOptions()),
            _ => new ChromeDriver()
        };
    }

    public string TakeScreenshotAsPath(string fileName)
    {
        var screenshot = Driver.TakeScreenshot();
        var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}//{fileName}.png";
        screenshot.SaveAsFile(path);
        return path;
    }

    public void Dispose()
    {
       Driver.Quit();
    }
}


public enum BrowserType
{
    Chrome,
    Firefox,
    Safari,
    EdgeChromium
}
