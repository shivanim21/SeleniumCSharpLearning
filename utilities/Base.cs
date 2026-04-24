using AventStack.ExtentReports;
using AventStack.ExtentReports.Listener.Entity;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using WebDriverManager.DriverConfigs.Impl;

namespace CSharpSelFramework.utilities
{
    public class Base
    {
        public ExtentReports extent;
        public ExtentTest test;
        String browserName;
        //report file
        [OneTimeSetUp]
        public void SetUp()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string reportPath = projectDirectory + "//index.html";
            var htmlReporter = new ExtentSparkReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Host Name", "Local Host");
            extent.AddSystemInfo("Environment", "QA");
            extent.AddSystemInfo("Username", "Shivani");
        }

        //public IWebDriver driver;
        public ThreadLocal<IWebDriver> driver = new();
        [SetUp]

        public void StartBrowser()

        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            browserName = ConfigurationManager.AppSettings["browser"];
            InitBrowser(browserName);

            getDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            getDriver().Manage().Window.Maximize();
            getDriver().Url = "https://rahulshettyacademy.com/loginpagePractise/";

            //ChromeOptions options = new ChromeOptions();
            ////Window Behavior
            //options.AddArgument("start-maximized");

            ////Remove automation message
            //options.AddExcludedArgument("enable-automation");
            //options.AddAdditionalOption("useAutomationExtension", false);

            ////Disable notifications and password popups
            //options.AddArgument("--disable-notifications");
            //options.AddUserProfilePreference("credentials_enable_service", false);
            //options.AddUserProfilePreference("profile.password_manager_enabled", false);

           


        }

        public IWebDriver getDriver()
        {
            return driver.Value;
        }

        public void InitBrowser(String browserName)
        {
            switch (browserName)
            {
                case "Firefox":
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                    driver.Value = new FirefoxDriver();
                    break;

                case "Chrome":
                    //new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());


                    ChromeOptions options = new ChromeOptions();

                    options.AddArgument("start-maximized");
                    options.AddExcludedArgument("enable-automation");
                    options.AddAdditionalOption("useAutomationExtension", false);
                    options.AddArgument("--disable-notifications");
                    options.AddUserProfilePreference("credentials_enable_service", false);
                    options.AddUserProfilePreference("profile.password_manager_enabled", false);
                    options.AddArgument("--disable-features=PasswordLeakDetection");
                    options.AddUserProfilePreference("credentials_enable_service", false);
                    options.AddUserProfilePreference("profile.password_manager_enabled", false);
                    options.AddArgument("--disable-features=PasswordLeakDetection");
                    options.AddArgument("--incognito");

                    driver.Value = new ChromeDriver(options);
                    break;

                case "Edge":
                    driver.Value = new EdgeDriver();
                    break;

            }

        }

        public static JsonReader getDataParser()
        {
            return new JsonReader();
        }

        [TearDown]
        public void CloseBrowser()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            DateTime time = DateTime.Now;
            String fileName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
            if(status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                test.Fail("Test Failed", captureScreenShot(driver.Value, fileName));
                test.Log(Status.Fail, "test failed with logtrace" + stackTrace);
            }
            else if(status== NUnit.Framework.Interfaces.TestStatus.Passed)
            {

            }
         
            if (driver is IDisposable disposable)
            {
                getDriver().Quit();
                disposable.Dispose();
            }
        }

        public AventStack.ExtentReports.Model.Media captureScreenShot(IWebDriver driver,String screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot().AsBase64EncodedString;

            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();
        }

        [OneTimeTearDown]
        public void EndReport()
        {
            extent.Flush();
        }
    }
}
