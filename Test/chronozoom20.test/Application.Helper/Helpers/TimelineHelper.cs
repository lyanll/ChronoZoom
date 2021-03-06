﻿using System;
using Application.Driver;
using Application.Helper.Constants;
using Application.Helper.Entities;
using Application.Helper.UserActions;
using OpenQA.Selenium;

namespace Application.Helper.Helpers
{
    public class TimelineHelper : DependentActions
    {
        public void AddTimeline(Timeline timeline)
        {
            Logger.Log("<- timeline: " + timeline);
            InitTimelineCreationMode();
            DrawTimeline();
            SetTimelineName(timeline.Title);
            CreateTimeline();
            WaitAjaxComplete(60);
            Logger.Log("->");
        }

        public void AddTimelineWithDateMode(Timeline timeline)
        {
            Logger.Log("<- timeline: " + timeline);
            InitTimelineCreationMode();
            DrawTimeline();
            SetTimelineName(timeline.Title);
            SetDateMode();
            CreateTimeline();
            WaitAjaxComplete(60);
            Logger.Log("->");
        }

        public Timeline GetLastTimeline()
        {
            Logger.Log("<-");
            var timeline = new Timeline();
            const string script = Javascripts.LastCanvasElement;
            timeline.Title = GetJavaScriptExecutionResult(script + ".title");
            timeline.TimelineId = GetJavaScriptExecutionResult(script + ".id");
            Logger.Log("-> " + timeline);
            return timeline;
        }

        public void DeleteTimeline(Timeline timeline)
        {
            Logger.Log("<- timeline: " + timeline);
            NavigateToTimeLine(timeline);
            WaitAnimation();
            InitEditForm();
            ClickDelete();
            ConfirmDeletion();
            Logger.Log("->");
        }

        public void DeleteTimelineByJavaScript(Timeline timeline)
        {
            Logger.Log("<-");
            ExecuteJavaScript(string.Format("CZ.Authoring.removeTimeline({0})", Javascripts.LastCanvasElement));
            Logger.Log("->");
        }

        public bool IsTimelineFound(Timeline newTimeline)
        {
            Logger.Log("<- timeline: " + newTimeline);
            try
            {
                ExecuteJavaScript(string.Format("CZ.Search.goToSearchResult('{0}', 'timeline')", newTimeline.TimelineId));
                Logger.Log("-> true");
                return true;
            }
            catch (Exception)
            {
                Logger.Log("-> false");
                return false;
            }
        }

        public void OpenLifeTimeline()
        {
            Logger.Log("<-");
            HelperManager<NavigationHelper>.Instance.OpenLifePage();
            WaitAnimation();
            WaitForElementIsDisplayed(By.XPath("//*[@id='breadcrumbs-table']//*[text()='Life']"));
            Logger.Log("->");
        }

        public void OpenHumanityTimeline()
        {
            Logger.Log("<-");
            HelperManager<NavigationHelper>.Instance.OpenHumanityPage();
            WaitForElementIsDisplayed(By.XPath("//*[@id='breadcrumbs-table']//*[text()='Humanity']"));
            WaitAnimation();
            Logger.Log("->");
        }


        public void OpenCosmosTimeline()
        {
            Logger.Log("<-");
            HelperManager<NavigationHelper>.Instance.NavigateToCosmos();
            WaitCondition(() => GetItemsCount(By.CssSelector("#breadcrumbs-table td")) == 1, Configuration.ImplicitWait);
            Logger.Log("->");
        }

        public void OpenBceCeArea()
        {
            Logger.Log("<-");
            NavigateBceToCeEra();
            WaitForElementIsDisplayed(By.XPath("//*[@id='breadcrumbs-table']//*[text()='Geologic Time Scale']"));
            WaitForElementIsDisplayed(By.XPath("//*[@class='cz-timescale-label' and contains(@style,'display: block;') and text()='1 BCE']"));
            Logger.Log("->");
        }

        public void OpenRomanHistoryTimeline()
        {
            Logger.Log("<-");
            HelperManager<NavigationHelper>.Instance.NavigateToRomanHistoryTimeline();
            WaitForElementIsDisplayed(By.XPath("//*[@id='breadcrumbs-table']//*[text()='Roman History']"));
            WaitAnimation();
            Logger.Log("->");
        }

        public void OpenHistoryOfScienceTimeline()
        {
            Logger.Log("<-");
            HelperManager<NavigationHelper>.Instance.NavigateToHistoryOfScienceTimeline();
            WaitForElementIsDisplayed(By.XPath("//*[@id='breadcrumbs-table']//*[text()='History of Science']"));
            WaitAnimation();
            Logger.Log("->");
        }

        private void ConfirmDeletion()
        {
            Logger.Log("<-", LogType.MessageWithoutScreenshot);
            AcceptAlert();
            MoveToElementAndClick(By.ClassName("virtualCanvasLayerCanvas"));
            Logger.Log("->");
        }

        private void ClickDelete()
        {
            Logger.Log("<-");
            Click(By.CssSelector("#auth-edit-timeline-form .cz-form-delete.cz-button"));
            Logger.Log("->", LogType.MessageWithoutScreenshot);
        }

        private void InitEditForm()
        {
            Logger.Log("<-");
            ExecuteJavaScript("CZ.Authoring.isActive = true");
            ExecuteJavaScript("CZ.Authoring.mode = 'editTimeline'");
            ExecuteJavaScript("CZ.Authoring.showEditTimelineForm(CZ.Authoring.selectedTimeline)");
            Logger.Log("->");
        }

        private void NavigateToTimeLine(Timeline timeline)
        {
            Logger.Log("<-");
            ExecuteJavaScript(string.Format("CZ.Search.goToSearchResult('{0}')", timeline.TimelineId));
            Logger.Log("->");
        }

        private void CreateTimeline()
        {
            Logger.Log("<-");
            Click(By.CssSelector("#auth-edit-timeline-form .cz-form-save.cz-button"));
            Logger.Log("->");
        }

        private void SetTimelineName(string timelineName)
        {
            Logger.Log("<- timeline: " + timelineName);
            WaitForElementIsDisplayed(By.Id("auth-edit-timeline-form"));
            By title = By.CssSelector("#auth-edit-timeline-form .cz-form-item-title.cz-input");
            WaitForElementIsDisplayed(title);
            WaitForElementEnabled(title);
            TypeText(title, timelineName);
            Logger.Log("->");
        }

        private void DrawTimeline()
        {
            Logger.Log("<-");
            MoveToElementAndDrugAndDrop(By.ClassName("virtualCanvasLayerCanvas"), 50, 50);
            Logger.Log("->");
        }

        private void InitTimelineCreationMode()
        {
            Logger.Log("<-");
            MoveToElementAndClick(By.CssSelector("[title='Create Your Events']"));
            MoveToElementAndClick(By.CssSelector("button.cz-form-create-timeline.cz-button"));
            Logger.Log("->");
        }

        private void SetDateMode()
        {
            Logger.Log("<-");
            SelectByText(By.CssSelector(".cz-form-time-start.cz-datepicker .cz-datepicker-mode.cz-input"), "Date");
            Logger.Log("->");
        }
    }
}