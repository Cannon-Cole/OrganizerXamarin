using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Organizer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailPage : ContentPage
    {
        int detailID;
        Organizer.Models.Event eventToDetail;
        public EventDetailPage()
        {
            InitializeComponent();
        }
        public EventDetailPage(int eventID)
        {
            InitializeComponent();
            this.detailID = eventID;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            string completion = "Something went wrong loading the completion state";

            eventToDetail = await App.Database.GetEventAsync(detailID);

            FlexLayout detailContainer = new FlexLayout
            {
                Direction = FlexDirection.Column,
                AlignItems = FlexAlignItems.Start,
                AlignContent = FlexAlignContent.Stretch
            };

            //Event NAME --------------------------------------------
            Frame eventNameContainer = new Frame
            {
                StyleClass = new List<string> { "DetailContainer" }
            };

            StackLayout eventNameLabelContainer = new StackLayout();

            Label eventNameHeading = new Label
            {
                Text = "Name",
                StyleClass = new List<string> { "Heading" }
            };

            Label eventName = new Label
            {
                Text = eventToDetail.Name,
                StyleClass = new List<string> { "Content" }
            };

            eventNameLabelContainer.Children.Add(eventNameHeading);
            eventNameLabelContainer.Children.Add(eventName);

            eventNameContainer.Content = eventNameLabelContainer;

            detailContainer.Children.Add(eventNameContainer);
            //Event NAME END --------------------------------------------

            //Event NOTE --------------------------------------------
            Frame eventNoteContainer = new Frame
            {
                StyleClass = new List<string> { "DetailContainer" }
            };

            StackLayout eventNoteLabelContainer = new StackLayout();

            Label eventNoteHeading = new Label
            {
                Text = "Note",
                StyleClass = new List<string> { "Heading" }
            };

            Label eventNote = new Label
            {
                Text = eventToDetail.Note,
                StyleClass = new List<string> { "Content" }
            };

            eventNoteLabelContainer.Children.Add(eventNoteHeading);
            eventNoteLabelContainer.Children.Add(eventNote);

            eventNoteContainer.Content = eventNoteLabelContainer;

            detailContainer.Children.Add(eventNoteContainer);
            //Event NOTE END --------------------------------------------

            //Event STARTDATE --------------------------------------------
            Frame eventStartDateContainer = new Frame
            {
                StyleClass = new List<string> { "DetailContainer" }
            };

            StackLayout eventStartDateLabelContainer = new StackLayout();

            Label eventStartDateHeading = new Label
            {
                Text = "Start Date",
                StyleClass = new List<string> { "Heading" }
            };

            Label eventStartDate = new Label
            {
                Text = eventToDetail.StartDate.ToString(),
                StyleClass = new List<string> { "Content" }
            };

            eventStartDateLabelContainer.Children.Add(eventStartDateHeading);
            eventStartDateLabelContainer.Children.Add(eventStartDate);

            eventStartDateContainer.Content = eventStartDateLabelContainer;

            detailContainer.Children.Add(eventStartDateContainer);
            //Event STARTDATE END --------------------------------------------

            //Event ENDDATE --------------------------------------------
            Frame eventEndDateContainer = new Frame
            {
                StyleClass = new List<string> { "DetailContainer" }
            };

            StackLayout eventEndDateLabelContainer = new StackLayout();

            Label eventEndDateHeading = new Label
            {
                Text = "End Date",
                StyleClass = new List<string> { "Heading" }
            };

            Label eventEndDate = new Label
            {
                Text = eventToDetail.EndDate.ToString(),
                StyleClass = new List<string> { "Content" }
            };

            eventEndDateLabelContainer.Children.Add(eventEndDateHeading);
            eventEndDateLabelContainer.Children.Add(eventEndDate);

            eventEndDateContainer.Content = eventEndDateLabelContainer;

            detailContainer.Children.Add(eventEndDateContainer);
            //Event ENDDATE END --------------------------------------------

            //Event STARTTIME --------------------------------------------
            Frame eventStartTimeContainer = new Frame
            {
                StyleClass = new List<string> { "DetailContainer" }
            };

            StackLayout eventStartTimeLabelContainer = new StackLayout();

            Label eventStartTimeHeading = new Label
            {
                Text = "Start Time",
                StyleClass = new List<string> { "Heading" }
            };

            Label eventStartTime = new Label
            {
                Text = Helper.TimeSpanToAMPM(eventToDetail.StartTime),
                StyleClass = new List<string> { "Content" }
            };

            eventStartTimeLabelContainer.Children.Add(eventStartTimeHeading);
            eventStartTimeLabelContainer.Children.Add(eventStartTime);

            eventStartTimeContainer.Content = eventStartTimeLabelContainer;

            detailContainer.Children.Add(eventStartTimeContainer);
            //Event STARTTIME END --------------------------------------------

            //Event ENDTIME --------------------------------------------
            Frame eventEndTimeContainer = new Frame
            {
                StyleClass = new List<string> { "DetailContainer" }
            };

            StackLayout eventEndTimeLabelContainer = new StackLayout();

            Label eventEndTimeHeading = new Label
            {
                Text = "End Time",
                StyleClass = new List<string> { "Heading" }
            };

            Label eventEndTime = new Label
            {
                Text = Helper.TimeSpanToAMPM(eventToDetail.EndTime),
                StyleClass = new List<string> { "Content" }
            };

            eventEndTimeLabelContainer.Children.Add(eventEndTimeHeading);
            eventEndTimeLabelContainer.Children.Add(eventEndTime);

            eventEndTimeContainer.Content = eventEndTimeLabelContainer;

            detailContainer.Children.Add(eventEndTimeContainer);
            //Event ENDTIME END --------------------------------------------

            //Event COMPLETE --------------------------------------------

            if (eventToDetail.Complete == 1)
            {
                completion = "Completed";
            }
            else
            {
                completion = "Not Completed";
            }

            Frame eventCompleteContainer = new Frame
            {
                StyleClass = new List<string> { "DetailContainer" }
            };

            StackLayout eventCompleteLabelContainer = new StackLayout();

            Label eventCompleteHeading = new Label
            {
                Text = completion,
                StyleClass = new List<string> { "Heading" }
            };

            Label eventComplete = new Label
            {
                Text = completion,
                StyleClass = new List<string> { "Content" }
            };

            eventCompleteLabelContainer.Children.Add(eventCompleteHeading);
            eventCompleteLabelContainer.Children.Add(eventComplete);

            eventCompleteContainer.Content = eventCompleteLabelContainer;

            detailContainer.Children.Add(eventCompleteContainer);
            //Event COMPLETE END --------------------------------------------

            detailLayout.Children.Add(detailContainer);

            Button editButton = new Button
            {
                Text = "Edit"
            };

            editButton.Clicked += EditEvent;

            Button deleteButton = new Button
            {
                Text = "Delete",
            };

            deleteButton.Clicked += DeleteEvent;

            detailLayout.Children.Add(editButton);
            detailLayout.Children.Add(deleteButton);

        }
        protected async void EditEvent(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditEventPage(eventToDetail));
        }
        protected async void DeleteEvent(object sender, EventArgs e)
        {
            await App.Database.DeleteEventAsync(eventToDetail);
            Helper.toDoNeedsLoading = true;
            Helper.monthNeedsLoading = true;
            await Application.Current.MainPage.Navigation.PopAsync();

        }
    }
}