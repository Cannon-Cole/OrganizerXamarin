using Organizer.Data;
using Organizer.Services;
using Organizer.Views;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Organizer
{
    public partial class App : Application
    {
        static OrganizerDatabase _database;
      
        public static OrganizerDatabase Database
        {
            get
            {
                if (_database == null)
                {      
                        _database = new OrganizerDatabase(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "OrganizerDB.db3"));
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
