using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Numerics;
using System.Reflection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.TestFramework;

namespace AutomationTest
{ 
    internal class Program
    {
        static void Main(string[] args)
        {
           LabCorpJobSearchTest test = new LabCorpJobSearchTest();
            test.Setup();
            test.GivenIOpenTheLabCorpWebsite();
            test.WhenIClickOnTheCareersLink();
            test.WhenISelectAndBrowseToThePosition();
            test.ThenIConfirmTheJobDetails();
            test.WhenIClickApplyNow();
            test.WhenIClickReturnToJobSearch();
        }
    }

    [Binding]
    public class LabCorpJobSearchTest
    {
        private IWebDriver driver;

      
        [BeforeScenario]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }

        [Given(@"I open the LabCorp website")]
        public void GivenIOpenTheLabCorpWebsite()
        {
            driver.Navigate().GoToUrl("https://www.labcorp.com");
        }

        [When(@"I click on the Careers link")]
        public void WhenIClickOnTheCareersLink()
        {
            // Find and click Careers link
            IWebElement careersLink = driver.FindElement(By.LinkText("Careers"));
            careersLink.Click();
        }

        [When(@"I select and browse to the position")]
        public void WhenISelectAndBrowseToThePosition()
        {
            // Search for a specific position
            IWebElement jobSearch = driver.FindElement(By.LinkText("Job Search"));
            jobSearch.Click();

              // Select and browse to the position
            IWebElement jobTitleLink = driver.FindElement(By.XPath("//a[contains(@href, '/job/')]"));
            string jobTitle = jobTitleLink.Text;

            IWebElement jobLocationLink = driver.FindElement(By.XPath("//span[@class='job-location']"));
            string jobLocation = jobLocationLink.Text;

            IWebElement jobId = jobLocationLink.FindElement(By.XPath("//span[@class='au-target jobId']"));
            string jobIdText = jobId.Text;

        
            jobTitleLink.Click();

            // Confirm Job Title, Job Location, Job ID, and another text of choice to match the previous page
            Assert.AreEqual(jobTitle, driver.FindElement(By.XPath("//h1[@class='job-title']")).Text);
            Assert.AreEqual(jobLocation, driver.FindElement(By.XPath("//span[contains(@class,'job-location')]")).Text);
            string jobid = string.Join("", jobIdText.ToCharArray().Where(Char.IsDigit));
            string _jobid = string.Join("", driver.FindElement(By.XPath("//span[@class='au-target jobId']")).Text.ToCharArray().Where(Char.IsDigit));
            Assert.AreEqual(jobid, _jobid);
        }

        [Then(@"I confirm the job details")]
        public void ThenIConfirmTheJobDetails()
        {
            IWebElement firstsentence = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div/div[2]/div/div/div[1]/section[1]/div/div/div[2]/div[2]/ul/li[1]"));
            string firstTxt = firstsentence.Text;


            IWebElement secondsentence = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div/div[2]/div/div/div[1]/section[1]/div/div/div[2]/div[2]/ul/li[2]"));
            string secondTxt = secondsentence.Text;

            IWebElement thirdsentence = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div/div[2]/div/div/div[1]/section[1]/div/div/div[2]/div[2]/ul/li[3]"));
            string thirdTxt = thirdsentence.Text;

            // Example assertions:
            Assert.AreEqual("Undertakes primary medical review of cases, including medical assessment of the case for seriousness, listedness/labeling, causality, adverse event coding and narrative review.", firstTxt);
            Assert.AreEqual("Updates and documents daily case data, case-feedback in appropriate trackers/tools to facilitate tracking and workflow management", secondTxt);
            Assert.AreEqual("Assumes complete responsibility for all assigned deliverables in line with expected quality, compliance and productivity SLAs and KPIs", thirdTxt);

        }

        [When(@"I click Apply Now")]
        public void WhenIClickApplyNow()
        {
            // Click Apply Now
            IWebElement applyNowButton = driver.FindElement(By.LinkText("Apply Now"));
            applyNowButton.Click();
        }   

        [When(@"I click Return to Job Search")]
        public void WhenIClickReturnToJobSearch()
        {
            // Click to Return to Job Search
            IWebElement returnToJobSearchLink = driver.FindElement(By.LinkText("Back to search results"));
            returnToJobSearchLink.Click();
        }
    }
}
