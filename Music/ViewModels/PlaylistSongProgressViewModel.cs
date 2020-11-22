using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Music.Logic.PlaySongs;
using System.Threading.Tasks;
namespace MediaHelpersCP.Music.ViewModels
{
    public class PlaylistSongProgressViewModel : BasicSongProgressViewModel
    {

        private int _upTo;

        public int UpTo
        {
            get { return _upTo; }
            set
            {
                if (SetProperty(ref _upTo, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        private int _songsLeft;

        public int SongsLeft
        {
            get { return _songsLeft; }
            set
            {
                if (SetProperty(ref _songsLeft, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        //this will need to subscribe to a message to populate the songsleft and upto.
        //i propose a class that has these 2 properties.

        private readonly IPlaylistSongProgressPlayer _player;

        private void UpdateProgress()
        {
            Execute.OnUIThread(() =>
            {
                SongsLeft = _player.SongsLeft;
                UpTo = _player.UpTo;
            });
        }

        public PlaylistSongProgressViewModel(IMP3Player mp3, IPlaylistSongProgressPlayer player, IPrepareSong prepare) : base(mp3, player, prepare)
        {
            _player = player;
            _player.UpdateProgress = UpdateProgress;
        }

        protected override async Task ActivateAsync()
        {
            await base.ActivateAsync();
            await _player.StartUpAsync();
        }
    }
}