using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using EaFramework.Driver;
using Reqnroll;
using Reqnroll.Bindings;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EaSpecflowTests.Hooks;


[Binding]
public class Initialization
{
    private static ExtentReports _extentReports;
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;
    private readonly IDriverFixture _driverFixture;
    private ExtentTest _scenario;


    public Initialization(ScenarioContext scenarioContext, FeatureContext featureContext, IDriverFixture driverFixture)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
        _driverFixture = driverFixture;
    }

    [BeforeTestRun]
    public static void InitializeExtentReports()
    {
        var extentReport =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/extentreport.html";
        _extentReports = new ExtentReports();
        var spark = new ExtentSparkReporter(extentReport);
        _extentReports.AttachReporter(spark);
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var feature = _extentReports.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
        _scenario = feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
    }

    [AfterStep]
    public void AfterStep()
    {
        var fileName =
            $"{_featureContext.FeatureInfo.Title.Trim()}_{Regex.Replace(_scenarioContext.ScenarioInfo.Title, @"\s", "")}";


        if (_scenarioContext.TestError == null)
            switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    _scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.When:
                    _scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                    break;
                case StepDefinitionType.Then:
                    _scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        else
            switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
            {
                case StepDefinitionType.Given:
                    _scenario
                        .CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text)
                        .Fail(_scenarioContext.TestError.Message, new ScreenCapture()
                        {
                            Path = _driverFixture.TakeScreenshotAsPath(fileName),
                            Title = "Error screenshot"
                        });

                    break;
                case StepDefinitionType.When:
                    _scenario
                        .CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text)
                        .Fail(_scenarioContext.TestError.Message, new ScreenCapture()
                        {
                            Path = _driverFixture.TakeScreenshotAsPath(fileName),
                            Title = "Error screenshot"
                        });
                    break;
                case StepDefinitionType.Then:
                    _scenario
                        .CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text)
                        .Fail(_scenarioContext.TestError.Message, new ScreenCapture()
                        {
                            Path = _driverFixture.TakeScreenshotAsPath(fileName),
                            Title = "Error screenshot"
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }



    [AfterTestRun]
    public static void TearDownReport() => _extentReports.Flush();
}