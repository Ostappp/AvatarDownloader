using AvatarDownloader.Interfaces;
using AvatarDownloader.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AvatarDownloader.SocialsAvatars
{
    internal class FBook : ISocialService
    {
        public async Task<string> GetProfileLogoURL(string username, string password)
        {
            string profilePicUrl = string.Empty;
            string facebookUrl = Consts.SocialsURLs[Socials.Facebook];
            var uniqueProfileDir = $"/tmp/edge-profile-{Guid.NewGuid()}-{DateTime.Now.Ticks}";

            var options = new EdgeOptions();
            options.AddArgument("--no-sandbox"); 
            options.AddArgument("--disable-dev-shm-usage"); 
            options.AddArgument("--disable-gpu");
            options.AddArgument("--headless");
            //options.AddArgument($"--user-data-dir={uniqueProfileDir}");

            using (IWebDriver driver = new EdgeDriver(options))
            {
                try
                {
                    // Вхід у Facebook
                    Console.WriteLine("Moving into facebook login page...");
                    string facebookLoginUrl = $"{facebookUrl}login";
                    driver.Navigate().GoToUrl(facebookLoginUrl);
                    await PageLoaderHandler.WaitForPageToLoad(driver, Consts.WebRequestTimeout);

                    Console.WriteLine("Filling login fields...");
                    driver.FindElement(By.Id("email")).SendKeys(username);
                    driver.FindElement(By.Id("pass")).SendKeys(password);
                    driver.FindElement(By.Name("login")).Click();
                    await PageLoaderHandler.WaitForPageToLoad(driver, Consts.WebRequestTimeout);


                    // Отримання посилання на фото профілю
                    Console.WriteLine("Moving into profile page...");
                    string facebookProfileUrl = $"{facebookUrl}me";
                    driver.Navigate().GoToUrl(facebookProfileUrl);
                    await PageLoaderHandler.WaitForPageToLoad(driver, Consts.WebRequestTimeout);

                    Console.WriteLine("Searching for profile avatar element...");
                    var profilePic = driver.FindElement(By.CssSelector("img[alt='Profile picture']"));
                    profilePicUrl = profilePic.GetAttribute("src");
                    Console.WriteLine("Facebook Profile Picture URL: " + profilePicUrl);


                    return profilePicUrl;
                }
                catch (WebDriverTimeoutException ex)
                {
                    Console.WriteLine($"The page could not be loaded at the specified time. Error: {ex.Message}");
                    return profilePicUrl;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return profilePicUrl;
                }
                finally
                {
                    driver.Quit();
                }
            }
        }
    }
}
