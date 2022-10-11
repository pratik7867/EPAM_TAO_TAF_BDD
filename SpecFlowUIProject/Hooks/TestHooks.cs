using System;
using System.Reflection;
using System.Configuration;
using TechTalk.SpecFlow;
using EPAM_TAO_TAF_BDD.TestSetup;
using EPAM_TAO_TAF_BDD.UI_Helpers;
using AventStack.ExtentReports.Gherkin.Model;
using BoDi;

namespace SpecFlowUIProject.Hooks
{   
    [Binding]
    public sealed class TestHooks : TestHookup
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        public static readonly IObjectContainer objectContainer = new ObjectContainer();
        public string strBrowser { get { return ConfigurationManager.AppSettings["Browser"].ToString(); } }
        public string strSiteURL { get { return ConfigurationManager.AppSettings["SiteURL"].ToString(); } }
        public string strAUT { get { return ConfigurationManager.AppSettings["AUT"].ToString(); } }                

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            //TODO: implement logic that has to run before executing each scenario
            try
            {   
                driver = InitBrowser((BrowserType)Enum.Parse(typeof(BrowserType), strBrowser.ToUpper()));
                objectContainer.RegisterInstanceAs(driver);

                CommonUtilities.commonUtilities.NavigateToURL(driver, strSiteURL);
                CommonUtilities.commonUtilities.MaximizeWindow(driver);

                ExtentReportHelper.GetInstance(strAUT, driver).CreateTest(scenarioContext.ScenarioInfo.Title);
            }
            catch (Exception ex)
            {
                ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusFail($"<br>{ex.Message}<br>Stack Trace: <br>{ex.StackTrace}<br>");
            }
        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            try
            {
                var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
                PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);                                    

                if (scenarioContext.TestError == null)
                {
                    if (stepType == "Given")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodePassed(Given.GherkinName, ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "When")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodePassed(When.GherkinName, ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "Then")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodePassed(Then.GherkinName, ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "And")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodePassed(And.GherkinName, ScenarioStepContext.Current.StepInfo.Text);
                }
                if (scenarioContext.TestError != null)
                {
                    var stacktrace = scenarioContext.TestError.StackTrace;
                    var errorMessage = "<pre>" + scenarioContext.TestError.Message + "</pre>";
                    var failureMessage = $"<br>{errorMessage}<br>Stack Trace: <br>{stacktrace}<br>";

                    if (stepType == "Given")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodeFailed(Given.GherkinName, ScenarioStepContext.Current.StepInfo.Text, failureMessage, CommonUtilities.commonUtilities.TakeScreenshot(driver, ScenarioStepContext.Current.StepInfo.Text));
                    if (stepType == "When")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodeFailed(When.GherkinName, ScenarioStepContext.Current.StepInfo.Text, failureMessage, CommonUtilities.commonUtilities.TakeScreenshot(driver, ScenarioStepContext.Current.StepInfo.Text));
                    if (stepType == "Then")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodeFailed(Then.GherkinName, ScenarioStepContext.Current.StepInfo.Text, failureMessage, CommonUtilities.commonUtilities.TakeScreenshot(driver, ScenarioStepContext.Current.StepInfo.Text));
                    if (stepType == "And")
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestNodeFailed(And.GherkinName, ScenarioStepContext.Current.StepInfo.Text, failureMessage, CommonUtilities.commonUtilities.TakeScreenshot(driver, ScenarioStepContext.Current.StepInfo.Text));
                }
            }
            catch(Exception ex)
            {
                ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusFail($"<br>{ex.Message}<br>Stack Trace: <br>{ex.StackTrace}<br>");
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
            try
            {                
                ExtentReportHelper.GetInstance(strAUT, driver).CloseExtentReport();
                CloseBrowser();                
            }
            catch (Exception ex)
            {
                ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusFail($"<br>{ex.Message}<br>Stack Trace: <br>{ex.StackTrace}<br>");
            }            
        }
    }
}
