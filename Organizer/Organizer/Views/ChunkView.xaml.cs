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
    public partial class ChunkView : ContentPage
    {
        List<Label> ToDoLabels = new List<Label>();
        bool firstLoad = true;
        public ChunkView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await ToDoScroll.ScrollToAsync(0.0, Helper.scrollPosition, false);
            });

            if (Helper.toDoNeedsLoading)
            {
                Helper.toDoNeedsLoading = false;

                await BuildToDoListAsync();
            }
        }
        protected async System.Threading.Tasks.Task BuildToDoListAsync()
        {
            DeleteAllEventsToDoView();

            List<Organizer.Models.Event> datesOfIncompleteEvents = await App.Database.GetDatesWithIncompleteEventsAsync();

            datesOfIncompleteEvents.Sort(new Comparison<Organizer.Models.Event>((x, y) => DateTime.Compare(x.StartDate, y.StartDate)));

            foreach (Organizer.Models.Event dateToAdd in datesOfIncompleteEvents)
            {
                AddToDoDayLabelToView(dateToAdd.StartDate.Date);

                string singleDigitDay = Helper.AddZeroToSingleDigit(dateToAdd.StartDate.Date.Day);

                string singleDigitMonth = Helper.AddZeroToSingleDigit(dateToAdd.StartDate.Date.Month);

                List<Organizer.Models.Event> eventsForCurrentDay = await App.Database.GetEventsForASpecificDay("\"" + dateToAdd.StartDate.Year + "-" + singleDigitMonth + "-" + singleDigitDay + "T00:00:00.000" + "\"");

                Console.WriteLine("\"" + dateToAdd.StartDate.Year + "-" + singleDigitMonth + "-" + singleDigitDay + "T00:00:00.000" + "\"");

                foreach (Organizer.Models.Event currentEvent in eventsForCurrentDay)
                {
                    await AddToDoEventToViewAsync(currentEvent);
                }
            }
        }
        protected void DeleteAllEventsToDoView()
        {
            ToDo.Children.Clear();
            ToDoLabels.Clear();
        }

        protected async System.Threading.Tasks.Task AddToDoEventToViewAsync(Organizer.Models.Event toAdd)
        {
            Frame EncapsulatingFrame = new Frame();
            EncapsulatingFrame.BindingContext = toAdd;
            EncapsulatingFrame.Padding = 5;
            EncapsulatingFrame.HorizontalOptions = LayoutOptions.FillAndExpand;
            EncapsulatingFrame.MinimumHeightRequest = 44;

            if (toAdd.Complete == 1)
            {
                EncapsulatingFrame.BackgroundColor = Helper.complete;
            }
            else
            {
                EncapsulatingFrame.BackgroundColor = Helper.notComplete;
            }

            TapGestureRecognizer toggleTap = new TapGestureRecognizer();
            toggleTap.Tapped += LoadEventDetails;
            EncapsulatingFrame.GestureRecognizers.Add(toggleTap);

            SwipeGestureRecognizer toggleSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Right | SwipeDirection.Left };
            toggleSwipe.Swiped += EventCompletionToggle;
            toggleSwipe.Threshold = 15;
            EncapsulatingFrame.GestureRecognizers.Add(toggleSwipe);

            Label eventName = new Label
            {
                Text = toAdd.Name,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black
            };

            Label eventNote = new Label
            {
                Text = toAdd.Note,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.Black
            };

            StackLayout labelStack = new StackLayout();

            labelStack.Children.Add(eventName);
            labelStack.Children.Add(eventNote);

            List<Organizer.Models.Chunk> chunksForEvent = await App.Database.GetChunksByEvent(toAdd);

            FlexLayout horizontal = new FlexLayout();

            foreach (Organizer.Models.Chunk chunkToAdd in chunksForEvent)
            {

                horizontal.Direction = FlexDirection.Row;

                Frame chunkIndicator = new Frame();
                chunkIndicator.BackgroundColor = (Color)Color.FromHex(chunkToAdd.Color);
                chunkIndicator.HorizontalOptions = LayoutOptions.Start;

                horizontal.Children.Add(chunkIndicator);
            }

            labelStack.Children.Add(horizontal);

            EncapsulatingFrame.Content = labelStack;
            ToDo.Children.Add(EncapsulatingFrame);
        }
        private async void LoadEventDetails(object sender, EventArgs e)
        {
            Frame eventCompleted = (Frame)sender;
            Organizer.Models.Event updatedEvent = (Models.Event)eventCompleted.BindingContext;

            await Navigation.PushAsync(new EventDetailPage(updatedEvent.EventID));
        }
        private void EventCompletionToggle(object sender, EventArgs e)
        {
            Frame eventCompleted = (Frame)sender;
            Organizer.Models.Event updatedEvent = (Models.Event)eventCompleted.BindingContext;

            if (eventCompleted.BackgroundColor == (Color)Helper.notComplete)
            {
                eventCompleted.BackgroundColor = Helper.complete;
                updatedEvent.Complete = 1;
            }
            else
            {
                eventCompleted.BackgroundColor = Helper.notComplete;
                updatedEvent.Complete = 0;
            }

            App.Database.SaveEventAsync(updatedEvent);
        }
        protected void AddToDoDayLabelToView(DateTime sectionDate)
        {

            Label dayLabel = new Label
            {
                Text = sectionDate.Date.ToShortDateString(),
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                TextColor = Color.Black
            };
            ToDoLabels.Add(dayLabel);
            ToDo.Children.Add(dayLabel);
        }
    }
}