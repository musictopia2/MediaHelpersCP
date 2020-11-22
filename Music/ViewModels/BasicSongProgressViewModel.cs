using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Music.DB.Models;
using MediaHelpersCP.Music.EventModels;
using MediaHelpersCP.Music.Helpers;
using MediaHelpersCP.Music.Logic.PlaySongs;
using System.Threading.Tasks;
using System.Timers;
using static MediaHelpersCP.Music.Helpers.GlobalStaticVariableClass;
namespace MediaHelpersCP.Music.ViewModels
{

    //we were going to decide to not allow extending.
    //however, we need to for the playlistsongs which needs extra properties.
    //otherwise, that would have been sealed.



    public class BasicSongProgressViewModel : Screen, IHandleAsync<NewSongEventModel>
    {
        private readonly IMP3Player _mp3;
        //the only problem is when song is over, the ui needs to know.

        private readonly IProgressMusicPlayer _player;
        private readonly IPrepareSong _prepare;

        public BasicSongProgressViewModel(IMP3Player mp3, IProgressMusicPlayer player, IPrepareSong prepare)
        {
            _mp3 = mp3;
            _player = player;
            _prepare = prepare;
            _timer = new Timer(1000); //i think that every second is fine.
            if (Aggregator == null)
            {
                throw new BasicBlankException("Needs event aggregator before doing the progress");
            }
            //was not fine because something else needed it before this point.
            Aggregator.Subscribe(this);
        }

        private bool _isSongPlaying; //if false, then even though it was loaded, can't do anything.

        public bool IsSongPlaying
        {
            get { return _isSongPlaying; }
            set
            {
                if (SetProperty(ref _isSongPlaying, value))
                {
                    //can decide what to do when property changes
                    if (value == false)
                        CurrentSong = null;
                    NotifyOfCanExecuteChange(nameof(CanPlayPause));
                    NotifyOfCanExecuteChange(nameof(CanNextSong));
                    PrivateChangeSongAsync();
                }
            }
        }

        private async void PrivateChangeSongAsync()
        {
            await Extensions.ChangeSongAsync();
        }

        private IBaseSong? _currentSong;

        public IBaseSong? CurrentSong
        {
            get { return _currentSong; }
            set
            {
                if (SetProperty(ref _currentSong, value))
                {
                    //can decide what to do when property changes
                    NotifyOfCanExecuteChange(nameof(CanPlayPause));
                    if (_currentSong != null)
                        IsSongPlaying = true;
                }
            }
        }

        private void MiddlePrepareSong()
        {
            OnPropertyChanged(nameof(SongName));
            OnPropertyChanged(nameof(ArtistName));
        }
        public string SongName
        {
            get
            {
                if (CurrentSong == null)
                    return "";
                return CurrentSong.SongName;
            }
        }
        public string ArtistName
        {
            get
            {
                if (CurrentSong == null)
                    return "";
                return CurrentSong.ArtistName;
            }
        }

        public int SongLength
        {
            get
            {
                if (CurrentSong == null)
                    return 0;
                return CurrentSong.Length;
            }
        }

        private int _resumeAt;

        public int ResumeAt
        {
            get { return _resumeAt; }
            set
            {
                if (SetProperty(ref _resumeAt, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        private string _progressText = "";

        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                if (SetProperty(ref _progressText, value))
                {
                    //can decide what to do when property changes
                }

            }
        }


        private readonly Timer _timer;
        public bool CanNextSong()
        {
            //needs an interface for this part.
            return CanPlayPause;
            //if i am wrong, can rethink.
        }
        public async Task NextSongAsync()
        {
            IsSongPlaying = await _player.NextSongAsync();
        }

        public bool CanPlayPause => !(CurrentSong == null);

        public void PlayPause()
        {
            _mp3.Pause();
        }
        protected override async Task ActivateAsync()
        {
            await base.ActivateAsync();
            _mp3.ErrorRaised += MP3_ErrorRaised;
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = false;
            _timer.Start();
        }
        protected override async Task TryCloseAsync()
        {
            _timer.Stop();
            _mp3.ErrorRaised -= MP3_ErrorRaised;

            await base.TryCloseAsync();
        }
        //public override Task CancelAsync()
        //{
        //    _timer.Stop();
        //    _mp3.ErrorRaised -= MP3_ErrorRaised;
        //    return base.CancelAsync();
        //}
        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            await FirstShowProgressAsync();
            //_timer.Start(); //go ahead each time.
        }

        private async Task FirstShowProgressAsync()
        {
            if (IsSongPlaying == false)
                return;            
            if (_mp3.IsFinished())
            {
                IsSongPlaying = await _player.NextSongAsync();
                return;
            }
            ResumeAt = _mp3.TimeElapsedSeconds();
            //Execute.OnUIThread(() => ResumeAt = _mp3.TimeElapsedSeconds());
            //ResumeAt = _mp3.TimeElapsedSeconds();
            ProgressText = ResumeAt.MusicProgressStringFromSeconds(SongLength);
            await _player.SongInProgressAsync(ResumeAt);
            _timer.Start();
        }

        private void MP3_ErrorRaised(string message)
        {
            UIPlatform.ShowError(message);
        }

        async Task IHandleAsync<NewSongEventModel>.HandleAsync(NewSongEventModel message)
        {
            CurrentSong = message.CurrentSong;
            ResumeAt = message.ResumeAt;
            IsSongPlaying = await _prepare.PrepareSongAsync(MiddlePrepareSong, CurrentSong, ResumeAt);
            _timer.Start(); //start up again.

            //only catch is something needs to know when song is playing.

        }
        //maybe this does not care about when it plays a song.
        //just needs to send the message so ui knows.

    }
}
