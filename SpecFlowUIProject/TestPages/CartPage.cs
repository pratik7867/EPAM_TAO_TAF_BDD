using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using EPAM_TAO_TAF_BDD.TestSetup;
using EPAM_TAO_TAF_BDD.UI_Helpers;

namespace SpecFlowUIProject.TestPages
{
    public class CartPage : DriverPageContext
    {
        private static readonly object syncLock = new object();
        private static CartPage _cartPage = null;

        IWebDriver driver { get; set; }

        CartPage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }

        public static CartPage GetInstance(IWebDriver _driver)
        {
            lock (syncLock)
            {
                if (_cartPage == null)
                {
                    _cartPage = new CartPage(_driver);
                }
                return _cartPage;
            }
        }

        #region Elements/Locators        

        [FindsBy(How = How.ClassName, Using = "inventory_item_price")]
        IWebElement divProductPrice { get; set; }

        [FindsBy(How = How.Id, Using = "checkout")]
        IWebElement btnCheckout { get; set; }

        #endregion

        public string GetProductPrice()
        {
            CommonUtilities.commonUtilities.WaitForPageLoad(driver, 10);

            return divProductPrice.Text;
        }

        public CheckoutPage ClickOnCheckout()
        {
            btnCheckout.Click();

            return CheckoutPage.GetInstance(driver);
        }
    }
}
