using CSharpSelFramework.pageObjects;
using CSharpSelFramework.utilities;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace CSharpSelFramework.tests
{
    //class level parallel - [Parallelizable(ParallelScope.Children)]
    // parallel with other class - [Parallelizable(ParallelScope.Self)]
    public class E2ETest : Base
    {
        
        
        [Test,TestCaseSource("AddTestDataConfig")]
        //[TestCase("rahulshettyacademy", "Learning@830$3mK2")]
        //[TestCase("rahulshettyacademy", "Learning")]

        //run parallel test - all data sets of test method
        [Parallelizable(ParallelScope.All)]
        
        //all test files in parallel



        public void EndToEndFlow(String username, String password, String[] expectedProducts)

        {

            //String[] expectedProducts = { "iphone X", "Blackberry" };
            String[] actualProducts = new string[2];
            LoginPage loginPage = new LoginPage(getDriver());
            ProductsPage productPage=loginPage.validLogin(username,password);
            productPage.waitForPageDisplay();
            //loginPage.getUserName().SendKeys("rahulshettyacademy");
            //getDriver().FindElement(By.Name("password")).SendKeys("Learning@830$3mK2");
            //getDriver().FindElement(By.XPath("//div[@class='form-group'][5]/label/span/input")).Click();
            //getDriver().FindElement(By.XPath("//input[@value='Sign In']")).Click();
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.PartialLinkText("Checkout")));

            IList<IWebElement> products = productPage.getCards();
            //getDriver().FindElements(By.TagName("app-card")); replaced by productPage.getCards();

            foreach (IWebElement product in products)
            {

                if (expectedProducts.Contains(product.FindElement(productPage.getCardTitle()).Text))

                {
                    product.FindElement(productPage.addToCartButton()).Click();
                }
                //TestContext.Progress.WriteLine(product.FindElement(By.CssSelector(".card-title a")).Text);

            }
            //getDriver().FindElement(By.PartialLinkText("Checkout")).Click();
            CheckoutPage checkoutPage = productPage.checkout();
            /*IList<IWebElement> checkoutCards = checkoutPage.getCards();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => checkoutCards.Count > 0);
            checkoutCards.ToList();
            for (int i = 0; i < checkoutCards.Count; i++)

            {
                actualProducts[i] = checkoutCards[i].Text;



            }
            Assert.Equals(expectedProducts, actualProducts);*/
            IList<IWebElement> checkoutCards = checkoutPage.getCards();

            var cards = checkoutCards.ToList();

            for (int i = 0; i < cards.Count; i++)
            {
                actualProducts[i] = cards[i].Text;
            }

            CollectionAssert.AreEqual(expectedProducts, actualProducts);

            //getDriver().FindElement(By.CssSelector(".btn-success")).Click();
            checkoutPage.checkOut();

            driver.Value.FindElement(By.Id("country")).SendKeys("ind");

            WebDriverWait wait = new WebDriverWait(getDriver(), TimeSpan.FromSeconds(8));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.LinkText("India")));
            driver.Value.FindElement(By.LinkText("India")).Click();


            driver.Value.FindElement(By.CssSelector("label[for*='checkbox2']")).Click();
            driver.Value.FindElement(By.CssSelector("[value='Purchase']")).Click();
            String confirText = driver.Value.FindElement(By.CssSelector(".alert-success")).Text;

            Assert.That(confirText, Does.Contain("Success"));

        }

        public static IEnumerable<TestCaseData> AddTestDataConfig()
        {
            yield return new TestCaseData(getDataParser().extractData("username"), getDataParser().extractData("password"), getDataParser().extractDataArray("products"));
            yield return new TestCaseData(getDataParser().extractData("username"), getDataParser().extractData("password"), getDataParser().extractDataArray("products"));
            yield return new TestCaseData(getDataParser().extractData("username_wrong"), getDataParser().extractData("password_wrong"), getDataParser().extractDataArray("products"));
          

        }
    }

}
