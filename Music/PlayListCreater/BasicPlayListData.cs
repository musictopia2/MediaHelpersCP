using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
namespace MediaHelpersCP.Music.PlayListCreater
{
    public class BasicPlayListData : ObservableObject
    {
        private int _artist;

        public int Artist
        {
            get { return _artist; }
            set
            {
                if (SetProperty(ref _artist, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private int _earliestYear;
        public int EarliestYear
        {
            get { return _earliestYear; }
            set
            {
                if (SetProperty(ref _earliestYear, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private int _latestYear;
        public int LatestYear
        {
            get { return _latestYear; }
            set
            {
                if (SetProperty(ref _latestYear, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private string _specializedFormat = "";
        public string SpecializedFormat
        {
            get { return _specializedFormat; }
            set
            {
                if (SetProperty(ref _specializedFormat, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private string _showType = "";
        public string ShowType
        {
            get { return _showType; }
            set
            {
                if (SetProperty(ref _showType, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private bool? _romantic;
        public bool? Romantic
        {
            get { return _romantic; }
            set
            {
                if (SetProperty(ref _romantic, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private bool? _tropical;
        public bool? Tropical
        {
            get { return _tropical; }
            set
            {
                if (SetProperty(ref _tropical, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private bool? _christmas;
        public bool? Christmas
        {
            get { return _christmas; }
            set
            {
                if (SetProperty(ref _christmas, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private bool? _workOut;
        public bool? WorkOut
        {
            get { return _workOut; }
            set
            {
                if (SetProperty(ref _workOut, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private bool _useLikeInSpecializedFormat;
        public bool UseLikeInSpecializedFormat
        {
            get { return _useLikeInSpecializedFormat; }
            set
            {
                if (SetProperty(ref _useLikeInSpecializedFormat, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private CustomBasicList<int> _songList = new CustomBasicList<int>();
        public CustomBasicList<int> SongList
        {
            get { return _songList; }
            set
            {
                if (SetProperty(ref _songList, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        protected bool CanResetChristmas = true;
        protected bool CanClearSongList = true;
        public virtual void SetDefaults()
        {
            if (CanResetChristmas == true)
            {
                Christmas = false;
            }

            Artist = 0;
            WorkOut = null;
            Tropical = null;
            if (CanClearSongList == true)
            {
                SongList.Clear();
            }

            Romantic = null;
            LatestYear = 0;
            EarliestYear = 0;
            ShowType = "";
            SpecializedFormat = "";
        }
    }
}