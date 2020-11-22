using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.Conductors;
using MediaHelpersCP.Music.Helpers;
using MediaHelpersCP.Music.Logic.PlaySongs;
using MediaHelpersCP.Music.TemporaryModels;
using System;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace MediaHelpersCP.Music.ViewModels
{
    public class JukeboxViewModel : ConductorSingle<object>
    {

        //first step, figure out the properties, etc needed.
        //i think its okay this time to have the enum inside the class.
        //was going to make it stand alone but if somebody wants to add to it, they can.
        private readonly IJukeboxLogic _jukebox;

        public JukeboxViewModel(IJukeboxLogic jukebox)
        {
            _jukebox = jukebox;
            _songsToPlay = _jukebox.SongsToPlay;
            DisplayName = "Jukebox Version 1"; //this is a brand new jukebox.  hopefully now will actually have a title to it.
        }

        private EnumJukeboxSearchOption _searchOption; //we need after all since ui uses it for binding.

        public EnumJukeboxSearchOption SearchOption
        {
            get { return _searchOption; }
            set
            {
                if (SetProperty(ref _searchOption, value))
                    NotifyOfCanExecuteChange(nameof(CanSearchSongs));

            }
        }
        private string _searchTerm = "";

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (SetProperty(ref _searchTerm, value))
                    NotifyOfCanExecuteChange(nameof(CanSearchSongs));

            }
        }

        private bool _isChristmas;

        public bool IsChristmas
        {
            get { return _isChristmas; }
            set
            {
                if (SetProperty(ref _isChristmas, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        private ArtistResult? _artistChosen;

        public ArtistResult? ArtistChosen
        {
            get { return _artistChosen; }
            set
            {
                if (SetProperty(ref _artistChosen, value))
                    NotifyOfCanExecuteChange(nameof(CanChooseArtist));

            }
        }
        private SongResult? _songChosen;

        public SongResult? SongChosen
        {
            get { return _songChosen; }
            set
            {
                if (SetProperty(ref _songChosen, value))
                    NotifyOfCanExecuteChange(nameof(CanAddSongToList));

            }
        }
        private SongResult? _deleteChosen;
        public SongResult? DeleteChosen
        {
            get { return _deleteChosen; }
            set
            {
                if (SetProperty(ref _deleteChosen, value))
                    NotifyOfCanExecuteChange(nameof(DeleteChosen));

            }
        }

        private string _resultsText = "";

        public string ResultsText
        {
            get { return _resultsText; }
            set
            {
                if (SetProperty(ref _resultsText, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        //if we need to focus on specific one, a new event message will be sent.

        private CustomBasicCollection<SongResult> _resultList = new CustomBasicCollection<SongResult>();

        public CustomBasicCollection<SongResult> ResultList
        {
            get { return _resultList; }
            set
            {
                if (SetProperty(ref _resultList, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        private CustomBasicCollection<ArtistResult> _artists = new CustomBasicCollection<ArtistResult>();

        public CustomBasicCollection<ArtistResult> Artists
        {
            get { return _artists; }
            set
            {
                if (SetProperty(ref _artists, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        //this needs to be added to the songs to play.



        private CustomBasicCollection<SongResult> _songsToPlay;

        public CustomBasicCollection<SongResult> SongsToPlay
        {
            get { return _songsToPlay; }
            set
            {
                if (SetProperty(ref _songsToPlay, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        public bool CanChooseArtist => ArtistChosen != null;
        public void ChooseArtist()
        {
            ResultList = _jukebox.GetSongList(EnumJukeboxSearchOption.Artist, ArtistChosen, IsChristmas, SearchTerm);
            ResultsText = ResultList.Count.ToString();
            EnumFocusCategory.Results.JukeboxFocus();
        }
        public bool CanAddSongToList
        {
            get
            {
                if (SongChosen == null)
                    return false;
                if (SongsToPlay.Exists(x => x.ID == SongChosen.ID))
                    return false;
                return true;
            }
        }
        public async Task AddSongToListAsync()
        {
            if (SongChosen == null)
                throw new BasicBlankException("Cannot add to song list when its null");
            await _jukebox.AddSongToListAsync(SongChosen);
            SongChosen = null; //i think.
        }
        public bool CanSearchSongs
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SearchTerm) == true || SearchOption == EnumJukeboxSearchOption.Artist || SearchOption == EnumJukeboxSearchOption.None)
                    return false;
                return true;
            }
        }
        public void SearchSongs()
        {
            ResultList = _jukebox.GetSongList(SearchOption, ArtistChosen, IsChristmas, SearchTerm);
            ResultsText = ResultList.Count.ToString();
            EnumFocusCategory.Results.JukeboxFocus();
        }
        public bool CanRemoveSongFromList => DeleteChosen != null;
        public async Task RemoveSongFromListAsync()
        {
            if (DeleteChosen == null)
                throw new BasicBlankException("Cannot remove song from list when its null");
            await _jukebox.RemoveSongFromListAsync(DeleteChosen);
            DeleteChosen = null;
        }

        private bool CalculateChristmas()
        {
            if (DateTime.Now.Month == 12 && DateTime.Now.Day <= 25)
                return true;
            return false;
        }

        public void ChristmasToggle() //i do like the fact that if christmas is toggled, then the artistlist must be changed at that moment.
        {
            IsChristmas = !IsChristmas;
            LoadArtistList();
        }

        public void LoadArtistList()
        {
            Artists = _jukebox.GetArtistList(IsChristmas);
            EnumFocusCategory.Artist.JukeboxFocus();
        }

        protected override async Task ActivateAsync()
        {
            await base.ActivateAsync();
            //this is when you first load.
            IsChristmas = CalculateChristmas();
            LoadArtistList();
            var model = cons!.Resolve<BasicSongProgressViewModel>(); //this means i have to register the view model in the bootstrapper.
            await LoadScreenAsync(model); //this loads the portion to show the song played, actually playing song, etc.
        }
    }
}