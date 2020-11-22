using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Video.DatabaseInterfaces.Movies;
using MediaHelpersCP.Video.Logic;
using System;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
//i think this is the most common things i like to do
namespace MediaHelpersCP.Video.ViewModels
{
    public class MovieLoaderViewModel : VideoMainLoaderViewModel<IMainMovieTable>
    {
        private readonly IMovieLoaderLogic _loader;

        public MovieLoaderViewModel(IMainMovieTable selectedItem, IVideoPlayer player, IMovieLoaderLogic loader) : base(selectedItem, player)
        {
            _loader = loader;
            //has to have other functions as well.

        }

        private string _button3Text = "";

        public string Button3Text
        {
            get { return _button3Text; }
            set
            {
                if (SetProperty(ref _button3Text, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private bool _button3Visible;

        public bool Button3Visible
        {
            get { return _button3Visible; }
            set
            {
                if (SetProperty(ref _button3Visible, value))
                {
                    //can decide what to do when property changes
                    NotifyOfCanExecuteChange(nameof(CanButton3Process));
                }

            }
        }
        public override async Task SaveProgressAsync()
        {
            if (SelectedItem.ResumeAt.HasValue == false || SelectedItem.ResumeAt!.Value < VideoPosition)
                SelectedItem.ResumeAt = VideoPosition;
            await _loader.UpdateMovieAsync(SelectedItem);
        }

        public override Task VideoFinishedAsync()
        {
            return _loader.FinishMovieAsync(SelectedItem);
        }
        private async Task<int> ResumeAtAsync()
        {
            if (SelectedItem.ResumeAt.HasValue == false)
                return 0;
            if (SelectedItem.ResumeAt!.Value == -1)
            {
                SelectedItem.ResumeAt = 0;
                await _loader.UpdateMovieAsync(SelectedItem);
                return 0;
            }
            return SelectedItem.ResumeAt.Value;
        }

        private int _secs;

        protected override async Task BeforePlayerInitAsync()
        {
            try
            {
                await base.BeforePlayerInitAsync(); //needs this for sure.
                _secs = await ResumeAtAsync();
                if (_secs == 0 && SelectedItem.Opening.HasValue == true)
                {
                    _secs = SelectedItem.Opening!.Value;
                }
                VideoPath = SelectedItem.Path;
                SelectedItem.LastWatched = DateTime.Now;
                if (SelectedItem.ResumeAt.HasValue == false)
                {
                    await _loader.UpdateMovieAsync(SelectedItem);
                }
                else
                {
                    await _loader.AddToHistoryAsync(SelectedItem);
                }
            }
            catch (Exception ex)
            {
                UIPlatform.ShowError(ex.Message);
            }
        }
        protected override async Task AfterPlayerInitAsync()
        {
            try
            {
                if (SelectedItem.Opening.HasValue == false)
                {
                    Button3Text = "Movie Started";
                    VideoLength = 0;
                }
                else if (SelectedItem.Closing.HasValue == false)
                {
                    Button3Text = "Movie Ended";
                    VideoLength = 0;
                }
                else
                {
                    VideoLength = SelectedItem.Closing!.Value;
                }
                ResumeSecs = _secs;
                VideoPosition = _secs;
                await ShowVideoLoadedAsync();
            }
            catch (Exception ex)
            {
                UIPlatform.ShowError(ex.Message);
            }
        }
        public bool CanButton3Process => Button3Visible;
        public async Task Button3ProcessAsync()
        {
            //this is the misc one.  i liked calling it button3.
            if (Button3Text == "Movie Started")
                await StartMovieAsync();
            else if (Button3Text == "Movie Ended")
                await EndMovieAsync();
        }

        private async Task StartMovieAsync()
        {
            if (VideoPosition > 0)
            {
                SelectedItem.Opening = VideoPosition;
                await _loader.UpdateMovieAsync(SelectedItem);
            }
            Button3Visible = true;
            Button3Text = "Movie Ended";
            await Task.Delay(2000);
            Button3Visible = false;
        }

        private async Task EndMovieAsync()
        {
            //because fat albert was less than 20 minutes.
            //may do something else eventually.
            if (VideoLength < 1200)
            {
                if (VideoPosition + 120 < VideoLength)
                {
                    return;
                }
            }
            else if (VideoPosition < 1200)
            {
                return;
            }
            SelectedItem!.Closing = VideoPosition;
            await VideoFinishedAsync();
        }
    }
}