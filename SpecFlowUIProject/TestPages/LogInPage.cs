using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using EPAM_TAO_TAF_BDD.TestSetup;
using EPAM_TAO_TAF_BDD.UI_Helpers;

namespace SpecFlowUIProject.TestPages
{
    public class LogInPage : DriverPageContext
    {
        private static readonly object syncLock = new object();
        private static LogInPage _loginPage = null;

        IWebDriver driver { get; set; }

        LogInPage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }

        public static LogInPage GetInstance(IWebDriver _driver)
        {
            lock (syncLock)
            {
                if (_loginPage == null)
                {
                    _loginPage = new LogInPage(_driver);
                }
                return _loginPage;
            }
        }

        #region Elements/Locators

        [FindsBy(How = How.Id, Using = "user-name")]
        IWebElement txtUserName { get; set; }

        [FindsBy(How = How.Id, Using = "password")]
        IWebElement txtPassword { get; set; }

        [FindsBy(How = How.Id, Using = "login-button")]
        IWebElement btnLogin { get; set; }

        #endregion

        #region Action Methods
        
        public ProductsPage LogIntoApplication(string strUserName, string strPassword)
        {
            CommonUtilities.commonUtilities.WaitForPageLoad(driver, 10);

            txtUserName.Clear();
            txtUserName.SendKeys(strUserName);

            txtPassword.Clear();
            txtPassword.SendKeys(strPassword);

            btnLogin.Click();

            return ProductsPage.GetInstance(driver);
        }

        #endregion
    }
}
