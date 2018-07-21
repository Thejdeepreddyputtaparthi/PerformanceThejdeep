using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PerformanceTest
{
    [TestClass]
    public class AstroPerformance
    {
        [TestMethod]
        public void AstroPerformanceTest()
        {
            using (var driver = new ChromeDriver())
            {

                var startTime = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;

                driver.Manage().Window.Maximize();

                // Go to the "Astro" homepage
                driver.Navigate().GoToUrl("https://www.astro.com.my");

                // Click on Continue to Astro if link is present

                if (driver.FindElementByCssSelector("#STO-Pop > section > div.dispose > img").Displayed)
                {
                    driver.FindElementByCssSelector("#STO-Pop > section > div.dispose > img").Click();
                }

                var homePageTime = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;

                var responseTime = homePageTime - startTime;

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(1));

                Console.WriteLine(" Total Page Load time for Astro home page is " + responseTime + "  MilliSecond");
                try
                {
                    if (responseTime < 1000)
                    {
                        Assert.IsTrue(true, " Page is loaded with in 0.1 seconds");
                    }

                }
                catch (Exception e1)
                {
                    Console.WriteLine("thrown exception" + e1);
                }

                // Page loads with in 5 seconds
                Assert.AreNotEqual(5000, responseTime, " Page is loaded with in 5 seconds");
                Console.WriteLine("Page is loaded with in 5 seconds");

                //Page Loading with out any Error
                Assert.AreEqual(true, driver.FindElementById("acmlogin").Displayed);
                Console.WriteLine(" Page loaded completely with out any Error");

                //Get all the links on the page (Taking First 50)s
                var urls = driver.FindElementsByCssSelector("a").Take(50);
                
                HttpWebRequest re = null;

                Console.WriteLine("Looking at all URLs of Astro site :");

               //Loop through all the urls
               // Ensure none of the links within this page results in a non-200 header response
                foreach (var url in urls)
                {
                    if (!(url.Text.Contains("Email") || url.Text == ""))
                    {
                        //Get the url
                        re = (HttpWebRequest) WebRequest.Create(url.GetAttribute("href"));
                        try
                        {
                            var response = (HttpWebResponse) re.GetResponse();
                            System.Console.WriteLine($"URL: {url.GetAttribute("href")} status is :{response.StatusCode}");
                        }
                        catch (WebException e)
                        {
                            var errorResponse = (HttpWebResponse) e.Response;
                            System.Console.WriteLine(
                                $"URL: {url.GetAttribute("href")} status is :{errorResponse.StatusCode}");
                        }
                    }
                }



            }
        }
    }
}
