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
    public partial class AddEventPage : ContentPage
    {
        List<Organizer.Models.Chunk> selectedChunks = new List<Organizer.Models.Chunk>();
        Organizer.Models.Event temp;
        public AddEventPage()
        {
            InitializeComponent();
            Helper.addEventChunkListNeedsLoading = true;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Helper.addEventChunkListNeedsLoading)
            { 
                BuildChunkList(await App.Database.GetChunksAsync());
                Helper.addEventChunkListNeedsLoading = false;
            }
        }
        private async void AddEvent(object sender, EventArgs e)
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

            await App.Database.SaveEventAsync(saveEvent);
            List<Organizer.Models.Event> lastInsertedList = await App.Database.GetLastInsertedEvent();

            IList<View> selectedChunks = ChunksAvailable.Children;

            foreach (View selected in selectedChunks)
            {
                if(selected.BackgroundColor == (Color) Helper.eventChunkSelectedColor)
                {
                    Organizer.Models.Chunk convertedChunk = (Organizer.Models.Chunk) selected.BindingContext;
                    Organizer.Models.ChunkEvent addChunkEvent = new Organizer.Models.ChunkEvent();             

                    addChunkEvent.ChunkID = convertedChunk.ChunkID;
                   
                    Console.WriteLine(lastInsertedList.Count+"last inserted");
                    foreach(Organizer.Models.Event pls in lastInsertedList)
                    {
                        Console.WriteLine(pls.EventID + " this is a line\n");
                    }
                    addChunkEvent.EventID = lastInsertedList[0].EventID;

                    await App.Database.SaveChunkEventAsync(addChunkEvent);
                }
            }

            Helper.OutputEventToConsole(saveEvent);

            Helper.toDoNeedsLoading = true;
            Helper.monthNeedsLoading = true;
            Helper.scrollPosition = 0.0;

            BuildChunkList(await App.Database.GetChunksAsync());

            EventName.Text = null;
            EventNote.Text = null;
            EventStartDate.Date = DateTime.Now;
            EventEndDate.Date = DateTime.Now;
            EventStartTime.Time = new TimeSpan();
            EventEndTime.Time = new TimeSpan();
        }

        private void StartDateSelected(object sender, DateChangedEventArgs e)
        {
            if (EventStartDate.Date > EventEndDate.Date)
            {
                EventEndDate.Date = EventStartDate.Date;
            }

            IsMultiDay();
        }
        private void EndDateSelected(object sender, DateChangedEventArgs e)
        {
            if (!DateValidates())
            {
                EndDateIsGreaterThanStartDateWarning.IsVisible = true;
            }
            else
            {
                EndDateIsGreaterThanStartDateWarning.IsVisible = false;
            }

            IsMultiDay();
        }

        private void TimeSelected(object sender, FocusEventArgs e)
        {
            if (EventStartTime.Time > EventEndTime.Time)
            {
                EventEndTime.Time = EventStartTime.Time;
            }

            if (!TimeValidates())
            {
                EndTimeIsGreaterThanStartTimeWarning.IsVisible = true;
            }
            else
            {
                EndTimeIsGreaterThanStartTimeWarning.IsVisible = false;
            }

            Console.WriteLine("timeselected");
        }
        private bool DateValidates()
        {
            if (EventStartDate.Date > EventEndDate.Date)
            {
                return false;
            }

            return true;
        }
        private bool TimeValidates()
        {
            if (EventStartTime.Time > EventEndTime.Time)
            {
                return false;
            }

            return true;
        }
        private void IsMultiDay()
        {
            if (EventStartDate.Date != EventEndDate.Date)
            {
                StartTimeLabel.Text = "Event begins on start day at";
                EndTimeLabel.Text = "Event ends on end day at";
            }
            else
            {
                StartTimeLabel.Text = "Start Time";
                EndTimeLabel.Text = "End Time";
            }
        }

        private void BuildChunkList(List<Organizer.Models.Chunk> chunkList)
        {
            ClearChunkList();

            foreach (Organizer.Models.Chunk chunk in chunkList)
            {
                Frame chunkBox = new Frame
                {
                    BackgroundColor = Color.FromHex(chunk.Color)
                };

                chunkBox.BindingContext = chunk;

                TapGestureRecognizer selectToggleTap = new TapGestureRecognizer();
                selectToggleTap.Tapped += selectChunkForEvent;
                chunkBox.GestureRecognizers.Add(selectToggleTap);

                Label chunkName = new Label
                {
                    Text = chunk.Name,
                    TextColor = Color.Black
                };

                chunkBox.Content = chunkName;

                ChunksAvailable.Children.Add(chunkBox);
            }
        }
        private void ClearChunkList()
        {
            ChunksAvailable.Children.Clear();
        }

        private void selectChunkForEvent(object sender, EventArgs e)
        {
            Frame chunkToHighlight = (Frame)sender;
            Organizer.Models.Chunk chunkToSave = (Organizer.Models.Chunk)chunkToHighlight.BindingContext;
            if (chunkToHighlight.BackgroundColor == (Color) Helper.eventChunkSelectedColor)
            {
                chunkToHighlight.BackgroundColor = Color.FromHex(chunkToSave.Color);
                selectedChunks.Remove(chunkToSave);
            }
            else
            {
                chunkToHighlight.BackgroundColor = Helper.eventChunkSelectedColor;
                selectedChunks.Add(chunkToSave);
            }
            
            
            Console.WriteLine(chunkToSave.Name);
            Console.WriteLine(selectedChunks.Count);

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Hi");
            List <Organizer.Models.Chunk> testChunks = await App.Database.GetChunksByEvent(temp);

            foreach (Organizer.Models.Chunk chunk in testChunks)
            {
                Console.WriteLine(chunk.ChunkID);
            }
        }
    }
}