using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace AvatarDownloader.Services
{
    internal class PageLoaderHandler
    {
        internal static async Task WaitForPageToLoad(IWebDriver driver, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            wait.Until(driver =>
            {
                return ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");
            });
        }
    }
}
