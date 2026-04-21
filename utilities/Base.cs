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
        //public IWebDriver driver;
        public ThreadLocal<IWebDriver> driver = new();
        [SetUp]

        public void StartBrowser()

        {
            String browserName = ConfigurationManager.AppSettings["browser"];
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
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
      

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
            if (driver is IDisposable disposable)
            {
                getDriver().Quit();
                disposable.Dispose();
            }
        }

    }
}
