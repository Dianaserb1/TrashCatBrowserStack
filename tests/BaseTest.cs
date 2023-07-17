using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using BrowserStack;
using System.Net;
using OpenQA.Selenium.Appium.iOS;

namespace alttrashcat_tests_csharp.tests
{
    public class BaseTest
    {
        // IOSDriver<IOSElement> appiumDriver;
        AndroidDriver<AndroidElement> appiumDriver;
        Local browserStackLocal;

        [OneTimeSetUp]
        public void SetupAppium()
        {
            String BROWSERSTACK_USERNAME = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            String BROWSERSTACK_ACCESSKEY = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESSKEY");
            String BROWSERSTACK_APP_ID_SDK_201 = Environment.GetEnvironmentVariable("BROWSERSTACK_APP_ID_SDK_201");

            // Use dot net bindings v4.0.0 or above
            AppiumOptions capabilities = new AppiumOptions();
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("projectName", "TrashCat");
            browserstackOptions.Add("buildName", "TrashCat201Android");
            browserstackOptions.Add("sessionName", "tests - " + DateTime.Now.ToString("MMMM dd - HH:mm"));
            browserstackOptions.Add("local", "true");
            browserstackOptions.Add("idleTimeout", "300");
            browserstackOptions.Add("userName", BROWSERSTACK_USERNAME);
            browserstackOptions.Add("accessKey", BROWSERSTACK_ACCESSKEY);
            capabilities.AddAdditionalCapability("bstack:options", browserstackOptions);
            capabilities.AddAdditionalCapability("platformName", "android");
            capabilities.AddAdditionalCapability("platformVersion", "11.0");
            capabilities.AddAdditionalCapability("appium:deviceName", "Samsung Galaxy S21");
            // capabilities.AddAdditionalCapability("platformName", "ios");
            // capabilities.AddAdditionalCapability("platformVersion", "16");
            // capabilities.AddAdditionalCapability("appium:deviceName", "iPhone 14");
            capabilities.AddAdditionalCapability("appium:app", BROWSERSTACK_APP_ID_SDK_201);

            Console.WriteLine("WebDriver request initiated. Waiting for response, this typically takes 2-3 mins");

            browserStackLocal = new Local();
            List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("key", BROWSERSTACK_ACCESSKEY)
                };
            browserStackLocal.start(bsLocalArgs);

            appiumDriver = new AndroidDriver<AndroidElement>(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capabilities);
            // appiumDriver = new IOSDriver<IOSElement>(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capabilities);
            Thread.Sleep(30000);
            Console.WriteLine("Appium driver started");
            // SetupPortForwarding();
            // IOSElement allowButton = appiumDriver.FindElementByAccessibilityId();
        }

        //browserstack has an idle timeout of max 300 seconds
        //so we need to do something with the appium driver
        //to keep it alive
        [TearDown]
        public void KeepAppiumAlive()
        {
            appiumDriver.GetDisplayDensity(); //android
            // appiumDriver.GetClipboardText(); //ios
        }

        [OneTimeTearDown]
        public void DisposeAppium()
        {
            Console.WriteLine("Ending");
            appiumDriver.Quit();
            if (browserStackLocal != null)
            {
                browserStackLocal.stop();
            }
        }

        void SetupPortForwarding()
        {
            AltReversePortForwarding.RemoveReversePortForwardingAndroid();
            AltReversePortForwarding.ReversePortForwardingAndroid();
            Console.WriteLine("Port forwarded (Android).");
        }
    }
}