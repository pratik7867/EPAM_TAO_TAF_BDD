using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using EPAM_TAO_TAF_BDD.TestSetup;
using EPAM_TAO_TAF_BDD.UI_Helpers;

namespace SpecFlowUIProject.TestPages
{
    public class CheckoutPage : DriverPageContext
    {
        private static readonly object syncLock = new object();
        private static CheckoutPage _checkoutPage = null;

        IWebDriver driver { get; set; }

        CheckoutPage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }

        public static CheckoutPage GetInstance(IWebDriver _driver)
        {
            lock (syncLock)
            {
                if (_checkoutPage == null)
                {
                    _checkoutPage = new CheckoutPage(_driver);
                }
                return _checkoutPage;
            }
        }

        #region Elements/Locators
                
        [FindsBy(How = How.Id, Using = "first-name")]
        IWebElement txtFirstName { get; set; }
        
        [FindsBy(How = How.Id, Using = "last-name")]
        IWebElement txtLastName { get; set; }

        [FindsBy(How = How.Id, Using = "postal-code")]
        IWebElement txtPostalCode { get; set; }

        [FindsBy(How = How.Id, Using = "continue")]
        IWebElement btnContinue { get; set; }

        [FindsBy(How = How.ClassName, Using = "inventory_item_name")]
        IWebElement divProductName { get; set; }

        [FindsBy(How = How.ClassName, Using = "inventory_item_price")]
        IWebElement divProductPrice { get; set; }

        [FindsBy(How = How.Id, Using = "finish")]
        IWebElement btnFinish { get; set; }        

        #endregion

        #region Action Mehods

        public void FillUpCheckoutDetailsAndContiue(string strFirstName, string strLastName, string strPostalCode)
        {
            CommonUtilities.commonUtilities.WaitForPageLoad(driver, 10);

            txtFirstName.Clear();
            txtFirstName.SendKeys(strFirstName);

            txtLastName.Clear();
            txtLastName.SendKeys(strLastName);

            txtPostalCode.Clear();
            txtPostalCode.SendKeys(strPostalCode);

            btnContinue.Click();
        }

        public string GetProductName()
        {
            return divProductName.Text;
        }

        public string GetProductPrice()
        {
            return divProductPrice.Text;
        }

        public void ClickOnFinish()
        {
            btnFinish.Click();
        }

        #endregion
    }
}
