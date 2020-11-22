using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using MediaHelpersCP.Music.DB.Models;
using MediaHelpersCP.Music.EventModels;
using MediaHelpersCP.Music.Helpers;
using MediaHelpersCP.Music.Logic.PlaySongs;
using System.Threading.Tasks;
using static MediaHelpersCP.Music.Helpers.GlobalStaticVariableClass;
namespace MediaHelpersCP.Music.ViewModels
{
    /// <summary>
    /// this class should give options like choose a playlist, delete, or erase songs from the playlist
    /// for now, not even doing phone playlists.
    /// if i ever enabled it again, then rethinking will be required.
    /// 
    /// </summary>
    public class PlaylistSongDashboardViewModel : Screen
    {
        //this focuses on desktop part of it.
        //not sure how i would eventually integrate the phone portion.
        //for now, don't worry about it.  because has to first make sure i have something.  can alway redo parts later if necessary.

        public PlaylistSongDashboardViewModel(IPlaylistSongMainLogic logic)
        {
            _logic = logic;
            if (Aggregator == null)
            {
                throw new BasicBlankException("Needs aggregator in order to use the dashboard view model");
            }
        }
        protected override async Task ActivateAsync()
        {
            await base.ActivateAsync();
            Playlists = await _logic.GetMainPlaylistsAsync();
            _recentPlaylist = await _logic.GetMostRecentPlaylistAsync();
            //PlayListOption = EnumPlayListOption.DeletePlayLists;
            //NotifyOfPropertyChange(() => PlayListOption);
            NotifyOfCanExecuteChange(nameof(CanChooseRecentPlayList));
            FocusOnFirst();
        }
        private EnumPlayListOption _playListOption = EnumPlayListOption.PlaySongsDefault;
        private readonly IPlaylistSongMainLogic _logic;
        private int? _recentPlaylist;
        public EnumPlayListOption PlayListOption
        {
            get { return _playListOption; }
            set
            {
                if (SetProperty(ref _playListOption, value))
                {
                    //can decide what to do when property changes
                }

            }
        }


        private IPlayListMain? _chosenPlayList;

        public IPlayListMain? ChosenPlayList
        {
            get { return _chosenPlayList; }
            set
            {
                if (SetProperty(ref _chosenPlayList, value))
                {
                    NotifyOfCanExecuteChange(nameof(CanSmartChooseOption));
                }

            }
        }

        //so i can do via hotkeys as well
        public void ChangePlaylistOption(EnumPlayListOption option)
        {
            PlayListOption = option;
        }


        private CustomBasicList<IPlayListMain> _playlists = new CustomBasicList<IPlayListMain>();

        public CustomBasicList<IPlayListMain> Playlists
        {
            get { return _playlists; }
            set
            {
                if (SetProperty(ref _playlists, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        public bool CanSmartChooseOption => ChosenPlayList != null;

        private async Task StartChoosingSongsAsync(int id)
        {
            //var playlist = await _logic.GetPlayListMainAsync(id);
            await _logic.SetMainPlaylistAsync(id); //so it gets set in whoever is responsible for handling this part.
            var model = new PlayListSongBuilderEventModel();
            await Aggregator!.PublishAsync(model);
        }

        public async Task SmartChooseOptionAsync()
        {
            if (ChosenPlayList == null)
            {
                throw new BasicBlankException("Needs to have a chosen playlist in order to use smart playlist options.  Rethink");
            }
            switch (PlayListOption)
            {
                case EnumPlayListOption.PlaySongsDefault:
                    //this will play songs.  if there is none, has to do something else.
                    await PlayChosenPlaylistAsync(ChosenPlayList.ID);
                    break;
                case EnumPlayListOption.ClearPlayLists:
                    //this will clear the songs in the existing playlist
                    await _logic.ClearSongsAsync(ChosenPlayList.ID);
                    await StartChoosingSongsAsync(ChosenPlayList.ID);
                    break;
                case EnumPlayListOption.DeletePlayLists:
                    //this will delete the song in the playlist.
                    await DeletePlayListAsync(ChosenPlayList.ID);
                    break;
                default:
                    throw new BasicBlankException("Option not supported for smart playlist option");
            }
            PlayListOption = EnumPlayListOption.PlaySongsDefault; //back to this one.
        }

        private async Task PlayChosenPlaylistAsync(int id)
        {
            if (await _logic.HasPlaylistCreatedAsync(id) == false)
            {
                await StartChoosingSongsAsync(id);
                return;
            }
            //this means can open the playlist.
            //var playlist = await _logic.GetPlayListMainAsync(id);
            await _logic.SetMainPlaylistAsync(id);
            var model = new StartPlayingPlayListEventModel();
            await Aggregator!.PublishAsync(model);
        }
        private async Task DeletePlayListAsync(int id)
        {
            await _logic.DeleteCurrentPlayListAsync(id);
            ChosenPlayList = null;
            Playlists = await _logic.GetMainPlaylistsAsync();
            FocusOnFirst(); //i think this should focus on first again.
        }

        public bool CanChooseRecentPlayList => _recentPlaylist != null;
        
        public async Task ChooseRecentPlayListAsync()
        {
            if (_recentPlaylist == null)
            {
                throw new BasicBlankException("Can't choose recent playlist because there was none");
            }
            await PlayChosenPlaylistAsync(_recentPlaylist.Value);
        }
    }
}