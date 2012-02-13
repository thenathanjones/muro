using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Muro.Tests.Features.Steps
{
    [Binding]
    public class ProjectStatusSteps
    {
        [Given(@"I have a successful build")]
        public void GivenIHaveASuccessfulBuild()
        {
            ScenarioContext.Current.Pending();
        }


        [When(@"I load the status page")]
        public void WhenILoadTheStatusPage()
        {
            ScenarioContext.Current.Pending();
        }


        [Then(@"I should see a successful build")]
        public void ThenIShouldSeeASuccessfulBuild()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
