﻿using System;
using System.IO;
using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace EPAM_TAO_TAF_BDD.UI_Helpers
{
    public class ExtentReportHelper
    {
        private static readonly object syncLock = new object();
        private static ExtentReportHelper _extentReportHelper = null;

        public ExtentReports extent { get; set; }
        public ExtentHtmlReporter reporter { get; set; }
        public ExtentTest test { get; set; }

        ExtentReportHelper(string strAUT, IWebDriver driver)
        {
            try
            {
                extent = new ExtentReports();

                reporter = new ExtentHtmlReporter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DateTime.Now.ToString("dd-MM-yyyy")) + @"\" + "TAF_Report.html");
                reporter.Config.DocumentTitle = "Automation Testing Report";
                reporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
                extent.AttachReporter(reporter);

                extent.AddSystemInfo("Application Under Test", strAUT);
                extent.AddSystemInfo("Environment", "QA");
                extent.AddSystemInfo("Machine", Environment.MachineName);
                extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);

                if (driver != null)
                {
                    ICapabilities browserCap = ((RemoteWebDriver)driver).Capabilities;

                    extent.AddSystemInfo("Browser", browserCap.BrowserName);
                    extent.AddSystemInfo("Browser Version", browserCap.Version);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }        

        public static ExtentReportHelper GetInstance(string strAUT, IWebDriver driver = null)
        {
            lock (syncLock)
            {
                if (_extentReportHelper == null)
                {
                    _extentReportHelper = new ExtentReportHelper(strAUT, driver);
                }
                return _extentReportHelper;
            }
        }

        public void CreateTest(string testName)
        {
            try
            {
                test = extent.CreateTest(testName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public void SetStepStatusPass(string stepDescription)
        {
            try
            {
                test.Log(Status.Pass, stepDescription);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void SetStepStatusWarning(string stepDescription)
        {
            try
            {
                test.Log(Status.Warning, stepDescription);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void SetTestStatusPass()
        {
            try
            {
                test.Pass("Test Executed Sucessfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void SetTestStatusFail(string message, string strPathToSSFile = null)
        {
            try
            {
                var printMessage = "<p><b>Test FAILED!</b></p>" + $"Message: <br>{message}<br>";
                test.Fail(printMessage);

                if (!string.IsNullOrEmpty(strPathToSSFile))
                {
                    test.AddScreenCaptureFromPath(strPathToSSFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }        
        public void SetTestStatusSkipped()
        {
            try
            {
                test.Skip("Test skipped!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SetTestNodePassed(string gherkinKeyword,string stepInfo)
        {
            try
            {
                test.CreateNode(gherkinKeyword, stepInfo).Pass("Step Executed Sucessfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SetTestNodeFailed(string gherkinKeyword, string stepInfo, string message, string strPathToSSFile = null)
        {
            try
            {
                var printMessage = "<p><b>Test FAILED!</b></p>" + $"Message: <br>{message}<br>";
                test.CreateNode(gherkinKeyword, stepInfo).Fail("Step Failed!");

                if (!string.IsNullOrEmpty(strPathToSSFile))
                {
                    test.AddScreenCaptureFromPath(strPathToSSFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseExtentReport()
        {
            try
            {
                extent.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
