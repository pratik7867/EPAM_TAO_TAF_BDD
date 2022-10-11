using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowUIProject.Steps
{
    [Binding]
    public class BaseStepConfiguration
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private static readonly object syncLock = new object();
        private static BaseStepConfiguration _baseStepConfiguration = null;

        public readonly ScenarioContext scenarioContext;
        public IWebDriver driver { get; set; }

        BaseStepConfiguration(ScenarioContext _scenarioContext)
        {
            scenarioContext = _scenarioContext;
        }

        public static BaseStepConfiguration GetInstance(ScenarioContext _scenarioContext)
        {
            lock (syncLock)
            {
                if (_baseStepConfiguration == null)
                {
                    _baseStepConfiguration = new BaseStepConfiguration(_scenarioContext);
                }
                return _baseStepConfiguration;
            }
        }
    }
}
