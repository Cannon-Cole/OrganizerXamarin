using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Organizer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddChunkPage : ContentPage
    {
        public AddChunkPage()
        {
            InitializeComponent();
        }

        private async void AddChunk(object sender, EventArgs e)
        {
            Organizer.Models.Chunk saveChunk = new Organizer.Models.Chunk();

            saveChunk.Name = ChunkName.Text;
            saveChunk.Note = ChunkNote.Text;
            saveChunk.Color = ChunkColor.SelectedColor.ToHex();
            saveChunk.StartTime = ChunkStartTime.Time;
            saveChunk.EndTime = ChunkEndTime.Time;

            ChunkButton.BackgroundColor = Color.FromRgb(ChunkColor.SelectedColor.R, ChunkColor.SelectedColor.G, ChunkColor.SelectedColor.B);

            await App.Database.SaveChunkAsync(saveChunk);

            Helper.addEventChunkListNeedsLoading = true;
            Helper.chunkViewNeedsLoading = true;

            List<Organizer.Models.Chunk> retrieveChunks = await App.Database.GetChunksAsync();
        }
    }
}