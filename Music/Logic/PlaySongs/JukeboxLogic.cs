using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.Extensions;
using CommonBasicStandardLibraries.Exceptions;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Music.DB.DataAccess;
using MediaHelpersCP.Music.DB.Models;
using MediaHelpersCP.Music.Helpers;
using MediaHelpersCP.Music.TemporaryModels;
using System.Linq;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses.StaticHelpers;
using cs = CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses.ConditionOperators;
namespace MediaHelpersCP.Music.Logic.PlaySongs
{
    //this will need to be singleton though.
    public class JukeboxLogic : IJukeboxLogic, IProgressMusicPlayer
    {
        private readonly IMP3Player _mp3;
        private readonly ISimpleMusicDataAccess _dats;
        public CustomBasicCollection<SongResult> SongsToPlay { get; set; } = new CustomBasicCollection<SongResult>(); //start with brand new.
        //bool IProgressMusicPlayer.NeedsToReportNextChangeWhenIsSongPlaying { get; }

        private IBaseSong? _currentSong;

        public JukeboxLogic(IMP3Player mp3, ISimpleMusicDataAccess dats)
        {
            _mp3 = mp3;
            _dats = dats;
        }

        async Task IJukeboxLogic.AddSongToListAsync(SongResult song)
        {
            SongsToPlay.Add(song);
            if (SongsToPlay.Count == 1)
            {
                _currentSong = _dats.GetSong(song.ID);
                await _currentSong.PlayNewSongAsync(0);
            }
        }

        CustomBasicCollection<ArtistResult> IJukeboxLogic.GetArtistList(bool isChristmas)
        {
            if (isChristmas == false)
            {
                var tempList = _dats.GetSortedArtistList();

                return tempList.Select
                    (Items => new ArtistResult { ArtistName = Items.ArtistName, ID = Items.ID }).ToCustomBasicCollection();
            }
            else
            {
                var starts = StartWithOneCondition(nameof(IBaseSong.Christmas), true);
                var firstList = _dats.GetCompleteSongList(starts);
                var nextList = firstList.Where(Items => Items.Christmas == true).ToCustomBasicList();
                var groups = nextList.GroupBy(Items => new { Items.ArtistName, Items.ArtistID })
                    .Select(Items => new ArtistResult { ArtistName = Items.Key.ArtistName, ID = Items.Key.ArtistID }).ToCustomBasicCollection();
                groups.Sort();
                return groups;
            }
        }

        CustomBasicCollection<SongResult> IJukeboxLogic.GetSongList(EnumJukeboxSearchOption searchOption, ArtistResult? artistChosen, bool isChristmas, string searchTerm)
        {
            if (searchOption == EnumJukeboxSearchOption.None || searchOption == EnumJukeboxSearchOption.Artist)
            {
                if (artistChosen == null)
                    return new CustomBasicCollection<SongResult>();
            }
            if (searchOption == EnumJukeboxSearchOption.Artist)
            {
                if (artistChosen == null)
                {
                    throw new BasicBlankException("Must have an artist if choosing by artist.  Otherwise, rethink");
                }
                CustomBasicList<ICondition> cList = StartWithOneCondition(nameof(IBaseSong.ArtistID), artistChosen.ID)
                    .AppendCondition(nameof(IBaseSong.Christmas), isChristmas);
                var firstList = _dats.GetCompleteSongList(cList, true); //still needs sorting because by song.

                return firstList.Select(items =>
                new SongResult { ID = items.ID, PlayListDisplay = items.GetSongArtistDisplay(), ResultDisplay = items.GetSongArtistDisplay() }).ToCustomBasicCollection();
            }
            CustomBasicList<IBaseSong> nextList;
            if (searchOption == EnumJukeboxSearchOption.SpecificWords)
            {
                CustomBasicList<ICondition> cList = StartWithOneCondition(nameof(IBaseSong.SongName), searchTerm)
                     .AppendCondition(nameof(IBaseSong.Christmas), isChristmas);
                var firstList = _dats.GetCompleteSongList(cList, true);
                nextList = firstList.ToCustomBasicList();
            }
            else
            {
                CustomBasicList<ICondition> conList = new CustomBasicList<ICondition>();
                if (isChristmas == true)
                    conList.AppendCondition(nameof(IBaseSong.Christmas), true);
                conList.AppendCondition(nameof(IBaseSong.SongName), cs.Like, searchTerm);
                var tempList = _dats.GetCompleteSongList(conList, true);
                nextList = tempList.ToCustomBasicList();
            }
            return nextList.OrderBy(items => items.SongName).Select(items =>
            new SongResult { ID = items.ID, PlayListDisplay = items.GetSongArtistDisplay(), ResultDisplay = items.GetSongArtistDisplay() }).ToCustomBasicCollection();
        }

        public async Task<bool> NextSongAsync()
        {
            _mp3.StopPlay();
            if (SongsToPlay.Count == 0)
            {
                throw new BasicBlankException("Cannot have 0 songs left to play");
            }
            SongsToPlay.RemoveFirstItem();
            if (SongsToPlay.Count == 0)
            {
                return false;
            }
            _currentSong = _dats.GetSong(SongsToPlay.First().ID);
            await _currentSong.PlayNewSongAsync(0); //something else handles actually playing song.
            return true;
        }

        async Task IJukeboxLogic.RemoveSongFromListAsync(SongResult song)
        {
            if (_currentSong == null)
            {
                throw new BasicBlankException("I think there should be a current song if removing song.  If I am wrong, rethink");
            }
            int id = song.ID;
            if (id == _currentSong.ID)
            {
                await NextSongAsync(); //hopefully this works.
                return;
            }
            SongsToPlay.RemoveSpecificItem(song);
        }

        Task IProgressMusicPlayer.SongInProgressAsync(int resumeAt)
        {
            return Task.CompletedTask; //we don't need to do anything (like save the progress of the song playing so upon autoresume, can resume where it left off.
            //if we decided to do this, rethink.
            //its now an option if necessary.
        }
    }
}