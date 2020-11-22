using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Music.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Music.DB.DataAccess
{
    /// <summary>
    /// this is intended to be used for playlist processes.
    /// since this require extra functions.
    /// </summary>
    public interface IPlaylistMusicDataAccess : ISimpleMusicDataAccess
    {
        CustomBasicList<IPlayListMain> GetAllPlaylists();
        Task ErasePlayListAsync(int id);
        Task DeletePlayListAsync(int id);
        Task ClearSongsAsync(int id);

        IEnumerable<IPlayListSong> GetPlayListSongs(int playList); //has to be ienumerable unfortunatley.
        IEnumerable<IPlayListDetail> GetPlayListDetails(int playList); //we don't have iqueryable.
        bool HasPlayListCreated(int playList);
        int? CurrentPlayList { get; } //will be read only.
        Task SetCurrentPlayListAsync(int? playList);
        Task UpdatePlayListProgressAsync(int secs, int songNumber, int playList);
        IPlayListProgress GetSinglePlayListProgress(int playlistid);
        //this requires advanced music processes.
        //since the playlist songs requires extra code and its the same regardless of database used.
        //unfortunately, this implies that we are using a sql like database.  if we ever decided not to, will require lots of rethinking.
        Task PerformAdvancedMusicProcessAsync(Func<IDbConnection, IDbTransaction, Task> action);
        Task AddNewPlayListProgressAsync(IPlayListProgress progress, IDbConnection cons, IDbTransaction trans);
        Task AddSeveralPlayListSongsAsync(IEnumerable<IPlayListSong> songlist);
    }
}