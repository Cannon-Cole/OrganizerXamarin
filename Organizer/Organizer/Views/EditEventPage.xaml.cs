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
    public partial class EditEventPage : ContentPage
    {
        Organizer.Models.Event eventToEdit;
        public EditEventPage()
        {
            InitializeComponent();
        }
        public EditEventPage(Organizer.Models.Event eventToEdit)
        {
            InitializeComponent();
            this.eventToEdit = eventToEdit;
            LoadForm();
        }
        private void LoadForm()
        {
            EventName.Text = eventToEdit.Name;
            EventNote.Text = eventToEdit.Note;
            EventStartDate.Date = eventToEdit.StartDate.Date;
            EventEndDate.Date = eventToEdit.EndDate.Date;
            EventStartTime.Time = eventToEdit.StartTime;
            EventEndTime.Time = eventToEdit.EndTime;
        }
        private async void EditEvent(object sender, EventArgs e)
        {

            string singleDigitMonth = "";
            string singleDigitDay = "";

            if (EventStartDate.Date.Month < 10)
            {
                singleDigitMonth = "0" + EventStartDate.Date.Month;
            }

            if (EventStartDate.Date.Month < 10)
            {
                singleDigitDay = "0" + EventStartDate.Date.Day;
            }

            Organizer.Models.Event saveEvent = new Organizer.Models.Event();
            saveEvent.Name = EventName.Text;
            saveEvent.Note = EventNote.Text;
            saveEvent.StartDate = EventStartDate.Date;
            saveEvent.EndDate = EventEndDate.Date;
            saveEvent.StartTime = EventStartTime.Time;
            saveEvent.EndTime = EventEndTime.Time;
            saveEvent.Complete = 0;

            Helper.OutputEventToConsole(saveEvent);

            Helper.toDoNeedsLoading = true;
            Helper.monthNeedsLoading = true;

            App.Database.SaveEventAsync(saveEvent);

            await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        //<StackLayout>

        //    <Label Text = "Add Event" HorizontalOptions="CenterAndExpand" />

        //    <Entry x:Name="EventName" Placeholder="Event Name" />

        //    <Editor x:Name="EventNote" Placeholder="Notes" />

        //    <Label Text = "Start Day" />
        //    < DatePicker x:Name="EventStartDate" Format="D"/>

        //    <Label Text = "End Day" />
        //    < DatePicker x:Name="EventEndDate" Format="D" />

        //    <Label Text = "Start Time" />
        //    < TimePicker x:Name="EventStartTime"/>

        //    <Label Text = "End Time" />
        //    < TimePicker x:Name="EventEndTime"/>

        //    <Button Text = "Add Event"
        //        HorizontalOptions="Center"
        //        Clicked="AddEvent"/>

        //</StackLayout>
    }
}