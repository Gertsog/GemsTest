using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace SeleniumTests
{
    /*
    *С помощью библиотеки "Selenium" и тестового фреймворка ("nUnit"/"xUnit") создайте тесты, проверяющие следующие тебования к сайту https://gemsdev.ru:
    *При открытии ссылки "Продукты" на странице появляются разделы 
    *"GeoMeta", 
    *"Государственная система обеспечения градостроительной деятельности", 
    *"Городская аналитика" и 
    *"Другие наши проекты".
    *В разделе "Государственная система обеспечения градостроительной деятельности" присутствует ссылка на сайт "https://xn--c1aaceme9acfqh.xn--p1ai/"
    */
    public class SeleniumTests
    {
        [Fact]
        public void VerifySectionsName()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://gemsdev.ru/");

            var productsButton = driver.FindElement(By.LinkText("Продукты"));
            productsButton.Click();

            var sections = driver.FindElements(By.XPath("//*[self::h1 or self::h2]"));
            var sectionsName = sections.Select(s => s.GetAttribute("textContent").Trim());

            Assert.Collection(sectionsName,
                name => Assert.Equal("Geometa", name),
                name => Assert.Equal("Государственная система обеспечения градостроительной деятельности", name),
                name => Assert.Equal("Городская аналитика", name),
                name => Assert.Equal("Другие наши продукты", name)
            );
        }

        [Fact]
        public void VerifyElementLink()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://gemsdev.ru/");

            var productsButton = driver.FindElement(By.LinkText("Продукты"));
            productsButton.Click();

            var gosSystemSection = driver.FindElement(By.XPath("//h2[text()='Государственная система обеспечения градостроительной деятельности']"));
            var gosSystemLink = gosSystemSection.FindElement(By.XPath("../a"));

            Assert.Equal("https://xn--c1aaceme9acfqh.xn--p1ai/", gosSystemLink.GetAttribute("href"));
        }
    }
}