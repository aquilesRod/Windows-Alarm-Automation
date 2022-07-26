using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MissaoRelatorioII
{
    public class Tests
    {
        public string appID = "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App";
        public string URL = "http://127.0.0.1:4723/";
        public WindowsDriver<WindowsElement> alarmSession;

        [SetUp]
        public void Setup()
        {
            if (alarmSession == null)
            {
                AppiumOptions appCapabilities = new AppiumOptions();
                appCapabilities.AddAdditionalCapability("platformName", "Windows");
                appCapabilities.AddAdditionalCapability("app", appID);
                appCapabilities.AddAdditionalCapability("deviceName", "WindowsPC");

                alarmSession = new WindowsDriver<WindowsElement>(new Uri(URL), appCapabilities);

                Assert.IsNotNull(alarmSession);
            }
        }

        [TearDown]
        public void CleanUP()
        {
            if (alarmSession != null)
            {
                alarmSession.Quit();
                alarmSession = null;
            }
        }

        [Test]
        public void CheckOptions()
        {
            //Verify if timer button is displayed
            var timerElement = alarmSession.FindElementByAccessibilityId("TimerButton");
            bool isDisplayed = timerElement.Displayed;
            Assert.AreEqual(true, isDisplayed, "Timer function is not present on app");

            //Verify if alarm button is displayed
            var alarmElement = alarmSession.FindElementByAccessibilityId("AlarmButton");
            isDisplayed = alarmElement.Displayed;
            Assert.AreEqual(true, isDisplayed, "Alarm function is not present on app");

            //Verify if world clock button is displayed
            var clockElement = alarmSession.FindElementByAccessibilityId("ClockButton");
            isDisplayed = clockElement.Displayed;
            Assert.AreEqual(true, isDisplayed, "World clock function is not present on app");

            //Verify if stopwatch button is displayed
            var stopWatchElement = alarmSession.FindElementByAccessibilityId("StopwatchButton");
            isDisplayed = stopWatchElement.Displayed;
            Assert.AreEqual(true, isDisplayed, "Stopwatch function is not present on app");
        }

        [Test]
        [TestCase("Sentinela", "Jingle", "Disabled")]
        public void setAlarm(string alarmName, string ringtone, string snoozeTime)
        {
            //Go to alarm page
            alarmSession.FindElementByAccessibilityId("AlarmButton").Click();

            //Add a new alarm
            #region
            alarmSession.FindElementByAccessibilityId("AddAlarmButton").Click();
            alarmSession.FindElementByName("Alarm name").SendKeys(alarmName);

            List<string> daysThatAlarmWillBeRepeats = new List<string>() { "Monday", "Wednesday", "Friday" };
            daysThatAlarmWillBeRepeats.ForEach(days => alarmSession.FindElementByName(days).Click());

            alarmSession.FindElementByAccessibilityId("ChimeComboBox").Click();
            alarmSession.FindElementByName(ringtone).Click();
            alarmSession.FindElementByAccessibilityId("SnoozeComboBox").Click();
            alarmSession.FindElementByName(snoozeTime).Click();

            //Save the new alarm
            alarmSession.FindElementByAccessibilityId("PrimaryButton").Click();
            #endregion

            //Verify if alarm is created
            Assert.IsTrue(alarmSession.FindElementByAccessibilityId("AlarmsScrollViewer").
                FindElementsByClassName("ToggleButton")[1].GetAttribute("Name").Contains(alarmName));


            //CODE THAT REMOVE TO APP MODIFICATIONS:
            #region
            alarmSession.FindElementByAccessibilityId("EditAlarmsButton").Click();
            alarmSession.FindElementByAccessibilityId("AlarmsScrollViewer").FindElementsByClassName("Button")[1].Click();
            #endregion
        }

        [Test]
        [TestCase("Lisbon, Portugal")]
        public void addClock(string city)
        {
            //Go to World clock page
            alarmSession.FindElementByAccessibilityId("ClockButton").Click();

            //Add a new clock
            #region
            alarmSession.FindElementByAccessibilityId("AddClockButton").Click();
            alarmSession.FindElementByAccessibilityId("TextBox").SendKeys(city);
            alarmSession.FindElementByAccessibilityId("SuggestionsList").FindElementByName(city).Click();
            //Save the new clock
            alarmSession.FindElementByAccessibilityId("PrimaryButton").Click();
            #endregion

            //Verify if clock is created
            var thisNewClockOnList = alarmSession.FindElementByAccessibilityId("ClockDetailListView").
                FindElementsByClassName("ListViewItem")[1];
            Assert.IsTrue(thisNewClockOnList.GetAttribute("Name").Contains(city));


            //CODE THAT REMOVE TO APP MODIFICATIONS:
            #region
            alarmSession.FindElementByName("Edit Clocks").Click();
            alarmSession.FindElementByAccessibilityId("ClockDetailListView").FindElementsByClassName("Button")[0].Click();
            #endregion
        }

        [Test]
        [TestCase("10")]
        public void testStopwatch(int time)
        {
            //Go to stopwatch page
            alarmSession.FindElementByAccessibilityId("StopwatchButton").Click();
            //Iniate stopwatch
            alarmSession.FindElementByAccessibilityId("StopwatchPlayPauseButton").Click();
            //Freeze the thread
            Thread.Sleep(time*1000);
            //Pause stopwatch
            alarmSession.FindElementByAccessibilityId("StopwatchPlayPauseButton").Click();

            //Verify if clock timer is equal to 10
            var stopWatchResult = alarmSession.FindElementByAccessibilityId("StopwatchTimerText").GetAttribute("Name");
            Assert.IsTrue(stopWatchResult.Contains($"{time} seconds"));


            //CODE THAT REMOVE TO APP MODIFICATIONS:

            alarmSession.FindElementByAccessibilityId("StopWatchResetButton").Click();
        }

    }
}