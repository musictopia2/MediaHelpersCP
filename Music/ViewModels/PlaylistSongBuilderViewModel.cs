using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using MediaHelpersCP.Music.DB.Models;
using MediaHelpersCP.Music.EventModels;
using MediaHelpersCP.Music.Logic.PlaySongs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaHelpersCP.Music.Helpers.GlobalStaticVariableClass;

namespace MediaHelpersCP.Music.ViewModels
{
    /// <summary>
    /// the purpose of this class is to build that playlist (choosing either how many songs or the percent of songs to choose).
    /// </summary>
    public class PlaylistSongBuilderViewModel : Screen
    {
        private readonly IPlaylistSongMainLogic _logic;



        public PlaylistSongBuilderViewModel(IPlaylistSongMainLogic logic)
        {
            _logic = logic;
        }
        protected override async Task ActivateAsync()
        {
            await base.ActivateAsync();
            SubLists = await _logic.GetPlaylistDetailsAsync();
            FocusOnFirst();
        }

        private CustomBasicCollection<IPlayListDetail> _subLists = new CustomBasicCollection<IPlayListDetail>();

        public CustomBasicCollection<IPlayListDetail> SubLists
        {
            get { return _subLists; }
            set
            {
                if (SetProperty(ref _subLists, value))
                {
                    //can decide what to do when property changes
                }

            }
        }


        public void FocusOnSongs()
        {
            FocusOnFirst();
        }

        private int _songsActuallyChosen;

        public int SongsActuallyChosen
        {
            get { return _songsActuallyChosen; }
            set
            {
                if (SetProperty(ref _songsActuallyChosen, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private int _percentage = 90;

        public int Percentage
        {
            get { return _percentage; }
            set
            {
                if (SetProperty(ref _percentage, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private int _howManySongsWanted;

        public int HowManySongsWanted
        {
            get { return _howManySongsWanted; }
            set
            {
                if (SetProperty(ref _howManySongsWanted, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        private void ClearSelections()
        {
            HowManySongsWanted = 0;
            Percentage = 90;
        }
        public async Task ChooseSongsAsync()
        {
            if (SubLists.Count == 0)
            {
                throw new BasicBlankException("Can't have 0 songs.  Otherwise, rethinking is required");
            }
            int actuallyChosen = await _logic.ChooseSongsAsync(SubLists.First(), Percentage, HowManySongsWanted);
            SongsActuallyChosen += actuallyChosen;
            SubLists.RemoveFirstItem();
            if (SubLists.Count == 0)
            {
                await _logic.CreatePlaylistSongsAsync();
                var model = new StartPlayingPlayListEventModel();
                await Aggregator!.PublishAsync(model);
                return;
            }
            ClearSelections();
            FocusOnFirst();
        }


    }
}
