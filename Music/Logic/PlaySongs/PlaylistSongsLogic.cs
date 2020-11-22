using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Music.BasicRandomizer;
using MediaHelpersCP.Music.DB.DataAccess;
using MediaHelpersCP.Music.DB.Models;
using MediaHelpersCP.Music.Helpers;
using MediaHelpersCP.Music.PlayListCreater;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings;

namespace MediaHelpersCP.Music.Logic.PlaySongs
{
    public class PlaylistSongsLogic<P> : IPlaylistSongMainLogic, IPlaylistSongProgressPlayer
        where P: BasicPlayListData, new()
    {
        private readonly IPlaylistMusicDataAccess _data;
        private readonly IMP3Player _mp3;

        private IBaseSong? _currentSong;
        private MusicShuffleProcesses? _ras;

        public PlaylistSongsLogic(IPlaylistMusicDataAccess data, IMP3Player mp3)
        {
            _data = data;
            _mp3 = mp3;
        }

        public int UpTo { get; set; }
        public int SongsLeft { get; set; }
        public Action UpdateProgress { get; set; } = (() => { }); //default is do nothing.  so if not given, just do nothing in this case.

        private int? _playlistId;

        private CustomBasicList<IBaseSong> _songs = new CustomBasicList<IBaseSong>(); //iffy for now.

        Task IPlaylistSongMainLogic.ClearSongsAsync(int playlist)
        {
            return _data.ClearSongsAsync(playlist);
        }

        Task IPlaylistSongMainLogic.DeleteCurrentPlayListAsync(int playlist)
        {
            return _data.DeletePlayListAsync(playlist);//hopefully this simple.
        }

        async Task<CustomBasicList<IPlayListMain>> IPlaylistSongMainLogic.GetMainPlaylistsAsync()
        {
            await Task.CompletedTask;
            return _data.GetAllPlaylists();
        }

        async Task<int?> IPlaylistSongMainLogic.GetMostRecentPlaylistAsync()
        {
            await Task.CompletedTask;
            return _data.CurrentPlayList;
        }

        async Task<CustomBasicCollection<IPlayListDetail>> IPlaylistSongMainLogic.GetPlaylistDetailsAsync()
        {
            if (_playlistId == null)
            {
                throw new BasicBlankException("Never saved the playlistid for the playlist used.  Rethink");
            }
            await Task.CompletedTask;
            _ras = new MusicShuffleProcesses(); //i think this is when we need a brand new one (?)
            return _data.GetPlayListDetails(_playlistId.Value).ToCustomBasicCollection();
        }


        async Task<bool> IPlaylistSongMainLogic.HasPlaylistCreatedAsync(int playlist)
        {
            await Task.CompletedTask;
            return _data.HasPlayListCreated(playlist);
        }

        async Task IPlaylistSongMainLogic.SetMainPlaylistAsync(int id) //hopefully this simple (?)
        {
            await _data.SetCurrentPlayListAsync(id);
            _playlistId = id;
        }

        public Task SongInProgressAsync(int resumeAt)
        {
            if (_songs.Count == 0)
            {
                throw new BasicBlankException("There are no songs.  Rethink");
            }
            if (UpTo == 0)
            {
                throw new BasicBlankException("Upto must be at least one.  Otherwise, rethink");
            }
            if (_playlistId == null)
            {
                throw new BasicBlankException("Must have a playlist associated.  Otherwise, can't show song progress");
            }
            _currentSong = _songs[UpTo - 1];
            //decided to take some risks.
            //i already had a possible situation where i went back in and missed part of the song.

            //if (_startedUp)
            //{
            //    _startedUp = false;
            //}
            //else
            //{
                //if (resumeAt > 0 && resumeAt < _lastResumeAt)
                //{

                //    throw new BasicBlankException("Rethinking may be required because the resumeat is hosed.  Maybe can stop but not sure");
                //}
                //_lastResumeAt = resumeAt; //try this as well.
            //}
            return _data.UpdatePlayListProgressAsync(resumeAt, _currentSong.SongNumber, _playlistId.Value);
        }
        async Task<int> IPlaylistSongMainLogic.ChooseSongsAsync(IPlayListDetail detail, int percentage, int howmanySongs)
        {
            if (_ras == null)
            {
                throw new BasicBlankException("Never created the random function before choosing songs.  Rethink");
            }
            BasicSection section = new BasicSection()
            {
                Percent = percentage,
                HowManySongs = howmanySongs
            };
            P data = await fs.DeserializeObjectAsync<P>(detail.JsonData);

            ISongListCreater query = Resolve<ISongListCreater>();
            CustomBasicList<ICondition> firstList = query.GetMusicList(data);
            section.SongList = _data.GetCompleteSongList(firstList);
            if (section.SongList.Count() == 0)
            {
                throw new BasicBlankException("I know this play list has more than 0 songs");
            }
            await _ras.AddSectionAsync(section); //problem is here
            int sofar = _ras.SongsSoFar();
            if (sofar == 0)
            {
                throw new BasicBlankException("Can't actually choose 0 songs");
            }
            return sofar; //hopefully works.
        }

        async Task IPlaylistSongMainLogic.CreatePlaylistSongsAsync()
        {
            if (_ras == null)
            {
                throw new BasicBlankException("Never created random function for creating playlist songs.  Rethink");
            }
            if (_playlistId == null)
            {
                throw new BasicBlankException("Never created playlist id.  Rethink");
            }
            var list = await _ras.GetRandomListAsync();
            var newList = new CustomBasicList<IPlayListSong>();
            list.ForEach(song =>
            {
                IPlayListSong fins = Resolve<IPlayListSong>();
                if (fins.ID != 0)
                {
                    throw new BasicBlankException("Did not use new object");
                }
                fins.SongNumber = list.IndexOf(song) + 1;
                fins.PlayList = _playlistId.Value;
                fins.SongID = song.ID;
                newList.Add(fins);
            });
            await _data.AddSeveralPlayListSongsAsync(newList);
        }

        async Task<bool> IProgressMusicPlayer.NextSongAsync()
        {
            if (_playlistId == null)
            {
                throw new BasicBlankException("Can't go to next song because we don't have a playlist id.  rethink");
            }
            _mp3.StopPlay();
            if (SongsLeft == 0)
            {
                //we needed to clear songs, not delete songs.
                await _data.ErasePlayListAsync(_playlistId.Value);
                UIPlatform.ExitApp(); //go ahead and exit at this point.
                return false; //i think.  even this can't hurt.
            }
            UpTo++;
            SongsLeft--;
            //_startedUp = true;
            //_lastResumeAt = 0; //to reset to 0 again.
            await SongInProgressAsync(0); //does not hurt to have this too.
            UpdateProgress.Invoke(); //to let them know something changed.
            if (_currentSong == null)
            {
                throw new BasicBlankException("No current song was sent.  Rethink");
            }
            await _currentSong.PlayNewSongAsync(0); //something else handles actually playing song.
            return true;
        }
        //private bool _startedUp;
        //private int _lastResumeAt;
        async Task IPlaylistSongProgressPlayer.StartUpAsync()
        {
            //_startedUp = true;
            if (_playlistId == null)
            {
                throw new BasicBlankException("There was no playlist id.  Therefore, can't even start up.  Rethink");
            }
            var progress = _data.GetSinglePlayListProgress(_playlistId.Value);
            var tempsongs = _data.GetPlayListSongs(_playlistId.Value);
            int resumeat = 0;
            await _data.PerformAdvancedMusicProcessAsync(async (cons, trans) =>
            {
                if (progress == null)
                {
                    progress = Resolve<IPlayListProgress>();
                    progress.PlayList = _playlistId.Value;
                    progress.ResumeAt = 0;
                    progress.SongNumber = 1;
                    UpTo = 1;
                    resumeat = 0;

                    await _data.AddNewPlayListProgressAsync(progress, cons, trans);
                }
                else
                {
                    resumeat = progress.ResumeAt;
                    UpTo = progress.SongNumber;
                }
                _songs = new CustomBasicList<IBaseSong>();
                foreach (IPlayListSong thisTemp in tempsongs)
                {
                    if (thisTemp.Song == null)
                    {
                        UIPlatform.ShowError("The Song From PlayList Song Was Nothing.  Rethink");
                        break;
                    }
                    thisTemp.Song.SongNumber = thisTemp.SongNumber;
                    _songs.Add(thisTemp.Song);
                }
                trans.Commit();
            });
            SongsLeft = _songs.Count - UpTo; //maybe no need for minus 1 (?)
            try
            {
                _currentSong = _songs[UpTo - 1];
            }
            catch (Exception ex)
            {
                UIPlatform.ShowError(ex.Message);
                return;
            }
            //_lastResumeAt = resumeat;
            UpdateProgress.Invoke(); //i think that this should to this too (?)
            await _currentSong.PlayNewSongAsync(resumeat); //hopefully this simple (?)
        }
    }
}