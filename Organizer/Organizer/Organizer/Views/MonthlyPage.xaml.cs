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
    public partial class MonthlyPage : ContentPage
    {
        DateTime dateIndex = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public MonthlyPage()
        {
            InitializeComponent();
            Helper.monthNeedsLoading = true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine(dateIndex);
            if (Helper.monthNeedsLoading)
            {
                buildMonthView();
            }
        }

        async void buildMonthView()
        {
            int dayCounter = 1;
            Boolean monthIsNotBuilt = true;
            Boolean weekIsNotBuilt = true;

            DeleteAllMonthCalendar();
            DeleteAllMonthToDoEvents();

            MonthNameLabel.Text = dateIndex.ToString("MMMM") + " " + dateIndex.ToString("yyyy");

            while (monthIsNotBuilt)
            {
                if (dayCounter <= DateTime.DaysInMonth(dateIndex.Year, dateIndex.Month))
                {
                    weekIsNotBuilt = true;
                }

                FlexLayout monthRow = new FlexLayout
                {
                    Direction = FlexDirection.Row,
                    AlignItems = FlexAlignItems.Center,
                    AlignContent = FlexAlignContent.Stretch
                };

                while (weekIsNotBuilt)
                {
                    Frame dayBox = new Frame();
                    Label dayNumber = new Label();

                    //Add empty calendar days from previous months
                    if (dateIndex.DayOfWeek != DayOfWeek.Sunday && dayCounter == 1)
                    {

                        int dayOfWeek = (int)dateIndex.DayOfWeek;

                        for (int i = 0; i < dayOfWeek; i++)
                        {
                            dayBox = new Frame
                            {
                                BackgroundColor = Color.SkyBlue,
                                HorizontalOptions = LayoutOptions.Center,
                                Padding = 5
                            };

                            FlexLayout.SetBasis(dayBox, new FlexBasis(.142857142857f, true));
                            monthRow.Children.Add(dayBox);
                        }
                    }

                    //Add calendar days with numbers in them
                    if (dayCounter <= DateTime.DaysInMonth(dateIndex.Year, dateIndex.Month))
                    {

                        List<Organizer.Models.Event> dailyEvent = await App.Database.GetEventsForASpecificDay("\"" + dateIndex.Year + "-" + Helper.AddZeroToSingleDigit(dateIndex.Month) + "-" + Helper.AddZeroToSingleDigit(dayCounter) + "T00:00:00.000" + "\"");

                        dayBox = new Frame
                        {
                            BackgroundColor = dailyEvent.Count > 0 ? Color.IndianRed : Color.SkyBlue,
                            HorizontalOptions = LayoutOptions.Center,
                            Padding = 5,
                            HeightRequest = 60
                        };

                        dayNumber = new Label
                        {
                            Text = dayCounter.ToString(),
                            HorizontalTextAlignment = TextAlignment.Center
                        };
                    }

                    TapGestureRecognizer openEventDetailedView = new TapGestureRecognizer();
                    openEventDetailedView.Tapped += DayTapped;
                    dayBox.GestureRecognizers.Add(openEventDetailedView);

                    FlexLayout.SetBasis(dayBox, new FlexBasis(.142857142857f, true));
                    dayBox.Content = dayNumber;
                    monthRow.Children.Add(dayBox);


                    //Check if the end of the week has been reached meaning start a new row
                    if (new DateTime(dateIndex.Year, dateIndex.Month, dayCounter).DayOfWeek == DayOfWeek.Saturday)
                    {
                        weekIsNotBuilt = false;
                    }

                    dayCounter++;

                    //Check if all days are built
                    if (dayCounter > DateTime.DaysInMonth(dateIndex.Year, dateIndex.Month))
                    {
                        monthIsNotBuilt = false;
                        weekIsNotBuilt = false;
                        int daysRemaining = (int)DayOfWeek.Saturday - (int)new DateTime(dateIndex.Year, dateIndex.Month, DateTime.DaysInMonth(dateIndex.Year, dateIndex.Month)).DayOfWeek;
                        Console.WriteLine(daysRemaining);
                        for (int i = 0; i < daysRemaining; i++)
                        {
                            dayBox = new Frame
                            {
                                BackgroundColor = Color.SkyBlue,
                                HorizontalOptions = LayoutOptions.Center,
                                Padding = 5
                            };

                            FlexLayout.SetBasis(dayBox, new FlexBasis(.142857142857f, true));
                            monthRow.Children.Add(dayBox);

                            dayCounter++;
                        }

                        break;
                    }
                }
                monthContainer.Children.Add(monthRow);
                Helper.monthNeedsLoading = false;
            }
        }
        protected void DeleteAllMonthCalendar()
        {
            monthContainer.Children.Clear();
        }
        protected void DeleteAllMonthToDoEvents()
        {
            listContainer.Children.Clear();
        }
        private async void DayTapped(object sender, EventArgs e)
        {
            Frame dayFrame = (Frame)sender;
            Label dayLabel = (Label)dayFrame.Content;
            int dayNumber = Convert.ToInt32(dayLabel.Text);

            List<Organizer.Models.Event> eventsForDayTapped = await App.Database.GetEventsForASpecificDay(Helper.prepareDateForDB(new DateTime(dateIndex.Year, dateIndex.Month, dayNumber)));
            DeleteAllMonthToDoEvents();
            if (eventsForDayTapped.Count > 0)
            {
                foreach (Organizer.Models.Event monthEvent in eventsForDayTapped)
                {
                    AddToDoEventToView(monthEvent);
                }
            }

            //buildMonthView();
            //await Navigation.PushAsync(new EventDetailPage(new DateTime(DateTime.Now.Year, DateTime.Now.Month, int.Parse(testlabel.Text))));
        }
        protected void AddToDoEventToView(Organizer.Models.Event toAdd)
        {
            Frame EncapsulatingFrame = new Frame();
            EncapsulatingFrame.BindingContext = toAdd;
            EncapsulatingFrame.Padding = 5;
            EncapsulatingFrame.HorizontalOptions = LayoutOptions.FillAndExpand;
            EncapsulatingFrame.MinimumHeightRequest = 44;
            Console.WriteLine(toAdd.Complete);
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
                HorizontalTextAlignment = TextAlignment.Center
            };

            Label eventNote = new Label
            {
                Text = toAdd.Note,
                HorizontalTextAlignment = TextAlignment.Center
            };

            StackLayout labelStack = new StackLayout();

            labelStack.Children.Add(eventName);
            labelStack.Children.Add(eventNote);

            EncapsulatingFrame.Content = labelStack;
            listContainer.Children.Add(EncapsulatingFrame);
        }
        private async void LoadEventDetails(object sender, EventArgs e)
        {
            Frame eventCompleted = (Frame)sender;
            Organizer.Models.Event updatedEvent = (Models.Event)eventCompleted.BindingContext;

            await Navigation.PushAsync(new EventDetailPage(updatedEvent.EventID));
        }
        private async void EventCompletionToggle(object sender, EventArgs e)
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

            Organizer.Models.Event test;
            test = await App.Database.GetEventAsync(updatedEvent.EventID);
            Console.WriteLine(test.Complete);
            Helper.toDoNeedsLoading = true;
        }

        private void BackMonth(object sender, EventArgs e)
        {
            dateIndex = dateIndex.AddMonths(-1);
            buildMonthView();
        }

        private void ForwardMonth(object sender, EventArgs e)
        {
            dateIndex = dateIndex.AddMonths(1);
            buildMonthView();
        }
    }
}