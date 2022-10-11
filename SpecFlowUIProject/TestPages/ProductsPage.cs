using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using EPAM_TAO_TAF_BDD.TestSetup;
using EPAM_TAO_TAF_BDD.UI_Helpers;

namespace SpecFlowUIProject.TestPages
{
    public class ProductsPage : DriverPageContext
    {
        private static readonly object syncLock = new object();
        private static ProductsPage _productsPage = null;

        IWebDriver driver { get; set; }

        ProductsPage(IWebDriver _driver) : base(_driver)
        {
            driver = _driver;
        }

        public static ProductsPage GetInstance(IWebDriver _driver)
        {
            lock (syncLock)
            {
                if (_productsPage == null)
                {
                    _productsPage = new ProductsPage(_driver);
                }
                return _productsPage;
            }
        }

        #region Elements/Locators
        
        string strDivProductPriceLocator = ".//button[@id='add-to-cart-{0}']/preceding::div[@class='inventory_item_price']";
        string strBtnAddToCartLocator = "add-to-cart-{0}";

        [FindsBy(How = How.Id, Using= "shopping_cart_container")]
        IWebElement btnShoppingCart { get; set; }        

        #endregion

        #region Action Methods

        public string getProductPrice(string strProductName)
        {
            CommonUtilities.commonUtilities.WaitForPageLoad(driver, 10);            

            return CommonUtilities.commonUtilities.WaitForElementToBeVisible(driver, By.XPath(String.Format(strDivProductPriceLocator, strProductName.Replace(" ", "-").ToLower())), 5).Text;
        }

        public void AddToCart(string strProductName)
        {
            CommonUtilities.commonUtilities.WaitForElementToBeVisible(driver, By.Id(String.Format(strBtnAddToCartLocator, strProductName.Replace(" ", "-").ToLower())), 5).Click();
        }

        public CartPage ClickOnShoppingCart()
        {
            btnShoppingCart.Click();

            return CartPage.GetInstance(driver);
        }

        #endregion
    }
}
