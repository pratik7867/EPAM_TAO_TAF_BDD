using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace EPAM_TAO_TAF_BDD.TestSetup
{
    public abstract class DriverPageContext
    {
        public DriverPageContext(IWebDriver driver)
        {
            try
            {
                PageFactory.InitElements(driver, this);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
