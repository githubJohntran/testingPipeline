using OpenQA.Selenium;

namespace StreetlightVision.Utilities
{
    public static class ElementUtility
    {
        public static bool IsDisplayed(By elementBy)
        {
            try
            {
                var element = WebDriverContext.CurrentDriver.FindElement(elementBy);
                return element.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}
