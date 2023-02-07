using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace SeleniumTests
{
    /*
    *� ������� ���������� "Selenium" � ��������� ���������� ("nUnit"/"xUnit") �������� �����, ����������� ��������� ��������� � ����� https://gemsdev.ru:
    *��� �������� ������ "��������" �� �������� ���������� ������� 
    *"GeoMeta", 
    *"��������������� ������� ����������� ����������������� ������������", 
    *"��������� ���������" � 
    *"������ ���� �������".
    *� ������� "��������������� ������� ����������� ����������������� ������������" ������������ ������ �� ���� "https://xn--c1aaceme9acfqh.xn--p1ai/"
    */
    public class SeleniumTests
    {
        [Fact]
        public void VerifySectionsName()
        {
            using IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://gemsdev.ru/");

            var productsButton = driver.FindElement(By.LinkText("��������"));
            productsButton.Click();

            var sections = driver.FindElements(By.XPath("//*[self::h1 or self::h2]"));
            var sectionsName = sections.Select(s => s.GetAttribute("textContent").Trim());

            Assert.Collection(sectionsName,
                name => Assert.Equal("Geometa", name),
                name => Assert.Equal("��������������� ������� ����������� ����������������� ������������", name),
                name => Assert.Equal("��������� ���������", name),
                name => Assert.Equal("������ ���� ��������", name)
            );
        }

        [Fact]
        public void VerifyElementLink()
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