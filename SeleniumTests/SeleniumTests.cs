using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace SeleniumTests
{
    public class SeleniumTests
    {
        [Fact]
        public void TestSections()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://gemsdev.ru/");

            var productsButton = driver.FindElement(By.LinkText("��������"));
            productsButton.Click();

            var sections = driver.FindElements(By.XPath("//*[self::h1 or self::h2]"));
            var sectionsName = sections.Select(s => s.GetAttribute("textContent").Trim());

            Assert.Collection(sectionsName,
                name => name.Equals("Geometa"),
                name => name.Equals("��������������� ������� ����������� ����������������� ������������"),
                name => name.Equals("��������� ���������"),
                name => name.Equals("������ ���� ��������")
            );
        }

        [Fact]
        public void TestLink()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://gemsdev.ru/");

            var productsButton = driver.FindElement(By.LinkText("��������"));
            productsButton.Click();

            var gosSystemSection = driver.FindElement(By.XPath("//h2[text()='��������������� ������� ����������� ����������������� ������������']"));
            var gosSystemLink = gosSystemSection.FindElement(By.XPath("../a"));

            Assert.Equal("https://xn--c1aaceme9acfqh.xn--p1ai/", gosSystemLink.GetAttribute("href"));
        }
    }
}