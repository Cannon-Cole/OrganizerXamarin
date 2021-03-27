using Organizer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Organizer.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}