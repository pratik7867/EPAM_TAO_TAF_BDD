﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace EPAM_TAO_TAF_BDD.UI_Helpers
{
    public class CommonUtilities
    {
        private static readonly object syncLock = new object();
        private static CommonUtilities _commonUtilities = null;

        private static Actions actions;

        private static string strParentDir, strSSDir, strPathToSSFile;

        CommonUtilities()
        {

        }

        public static CommonUtilities commonUtilities
        {
            get
            {
                lock(syncLock)
                {
                    if(_commonUtilities == null)
                    {
                        _commonUtilities = new CommonUtilities();
                    }
                    return _commonUtilities;
                }
            }
        }

        #region Navigation Utils

        public void NavigateToURL(IWebDriver driver, string strURL)
        {
            try
            {
                driver.Navigate().GoToUrl(strURL);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void MaximizeWindow(IWebDriver driver)
        {
            try
            {
                driver.Manage().Window.Maximize();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Wait Utils

        public void WaitForPageLoad(IWebDriver driver, int intWaitForNoOfSeconds)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(intWaitForNoOfSeconds));
                wait.Until(_driver => ExecuteJS(driver, "return document.readyState").Equals("complete"));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void WaitForAjaxToComplete(IWebDriver driver, int intWaitForNoOfSeconds)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(intWaitForNoOfSeconds));
                wait.Until(_driver => ExecuteJS(driver, "return window.jQuery && window.jQuery.active == 0"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IWebElement WaitForElementToBeVisible(IWebDriver driver, By locator, int intWaitForNoOfSeconds)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(intWaitForNoOfSeconds)).Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IList<IWebElement> WaitForElementsCollectionToBeVisible(IWebDriver driver, By locator, int intWaitForNoOfSeconds)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(intWaitForNoOfSeconds)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IWebElement WaitForElementToBeClickable(IWebDriver driver, By locator, int intWaitForNoOfSeconds)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(intWaitForNoOfSeconds)).Until(ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region JS Util

        public object ExecuteJS(IWebDriver driver, string strJS, IWebElement element = null)
        {
            try
            {
                if (element == null)
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript(strJS);
                }
                else
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript(strJS, element);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Action Utils

        public void MoveToElementAndClick(IWebDriver driver, IWebElement element)
        {
            try
            {
                actions = new Actions(driver);
                actions.MoveToElement(element).Click().Build().Perform();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DoubleClick(IWebDriver driver)
        {
            try
            {
                actions = new Actions(driver);
                actions.DoubleClick().Perform();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Additonal Utils

        public string TakeScreenshot(IWebDriver driver, string strSSFileName)
        {
            try
            {                
                strParentDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DateTime.Now.ToString("dd-MM-yyyy"));
                strSSDir = strParentDir + @"\" + "Screenshots";

                lock(syncLock)
                {
                    if(!Directory.Exists(strParentDir))
                    {
                        Directory.CreateDirectory(strParentDir);
                    }
                    else
                    {
                        if(!Directory.Exists(strSSDir))
                        {
                            Directory.CreateDirectory(strSSDir);
                        }
                    }
                }

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                Guid guid = Guid.NewGuid();
                strPathToSSFile = strSSDir + @"\" + strSSFileName + "_" + guid;
                screenshot.SaveAsFile(strPathToSSFile, ScreenshotImageFormat.Png);

                return strPathToSSFile;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public string GetPageTitle(IWebDriver driver)
        {
            try
            {
                return driver.Title;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

    }
}
