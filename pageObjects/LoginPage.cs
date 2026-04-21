using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpSelFramework.pageObjects
{
    public class LoginPage
    {
        private IWebDriver driver;
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);

        }

        //Pageobject factory

        //Encapsulation
        [FindsBy(How = How.Id, Using = "username")]
        private IWebElement username;

        [FindsBy(How = How.Name, Using = "password")]
        private IWebElement password;

        [FindsBy(How = How.XPath, Using = "//div[@class='form-group'][5]/label/span/input")]
        private IWebElement checkBox;

        [FindsBy(How = How.XPath, Using = "//input[@value='Sign In']")]
        private IWebElement signInButton;


        public ProductsPage validLogin(string user, string pass)
        {
            username.SendKeys(user);
            password.SendKeys(pass);
            checkBox.Click();
            signInButton.Click();
            return new ProductsPage(driver);
        }



        public IWebElement getUserName()
        {
            return username;
        }
    }
}
