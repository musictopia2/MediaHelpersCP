using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using MediaHelpersCP.Video.Logic;
using MediaHelpersCP.Video.TelevisionMiscClasses;
using System;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.ViewModels
{
    public class TelevisionLoaderViewModel : VideoMainLoaderViewModel<IEpisodeTable>
    {
        private readonly ITelevisionLoaderLogic _logic;

        public TelevisionLoaderViewModel(IEpisodeTable selectedItem, IVideoPlayer player, ITelevisionLoaderLogic logic) : base(selectedItem, player)
        {
            _logic = logic;
        }

        public override Task SaveProgressAsync()
        {
            return _logic.UpdateTVShowProgressAsync(SelectedItem, VideoPosition);
        }

        public override Task VideoFinishedAsync()
        {
            return _logic.FinishTVEpisodeAsync(SelectedItem);
        }
        private bool _hasIntro;
        protected override async Task BeforePlayerInitAsync()
        {
            try
            {
                await base.BeforePlayerInitAsync();
                int secs = _logic.GetSeconds(SelectedItem);
                ResumeSecs = secs;
                VideoPosition = ResumeSecs;
                VideoPath = SelectedItem.FullPath();
                _hasIntro = SelectedItem.BeginAt > 0; //taking a chance by showing that if begin at is greater than 0 then show it has intro.
                //not sure if it will cause problems but its a chance i have to take.
                //good news is if i run into more problems can do a mock where it pretends it chose the episode for testing.
                //_hasIntro = SelectedItem.AlreadySkippedOpening == false && SelectedItem.BeginAt > 0;
                await _logic.AddToHistoryAsync(SelectedItem);
            }
            catch (Exception ex)
            {
                UIPlatform.ShowError(ex.Message);
            }
        }

        private (int startTime, int howLong) GetSkipData()
        {
            return (SelectedItem.BeginAt, SelectedItem.OpeningLength.Value);
        }

        protected override async Task AfterPlayerInitAsync()
        {
            try
            {
                if (_hasIntro)
                {
                    var (StartTime, HowLong) = GetSkipData();
                    SkipSceneClass ThisSkip = new SkipSceneClass()
                    {
                        StartTime = StartTime,
                        HowLong = HowLong
                    };
                    var ThisList = new CustomBasicList<SkipSceneClass> { ThisSkip };
                    Player.AddScenesToSkip(ThisList);
                }
                var tvLength = Player.Length();
                await CalculateDurationAsync(tvLength);
            }
            catch (Exception ex)
            {
                UIPlatform.ShowError(ex.Message);
            }
        }

        private async Task CalculateDurationAsync(int tvLength)
        {
            int newLength;
            TimeSpan thisSpan = TimeSpan.FromSeconds(tvLength);
            if (thisSpan.Minutes >= 20 && SelectedItem.ClosingLength.HasValue == true)
                newLength = tvLength - SelectedItem.ClosingLength!.Value;
            else
                newLength = tvLength;
            VideoLength = newLength;
            await ShowVideoLoadedAsync();
        }
    }
}