using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Music.DB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Music.Logic.PlaySongs
{
    /// <summary>
    /// this is all the logic parts for playlist songs that is not quite related to actually playing the song.
    /// however, one class can implement both.
    /// </summary>
    public interface IPlaylistSongMainLogic
    {

        //TODO:  implement the business logic for playlist songs.

        //this may have other interfaces required to implement other parts.

        //if so, create other interfaces and those has to be separate.

        //can't be just andy since this is cross platform and intended to be very generic.

        //the parts that can't be generic, other interfaces has to be implemented.

        

        //int? CurrentPlaylist { get; } //not sure if we can set or not (?)

        Task<int?> GetMostRecentPlaylistAsync();

        Task<CustomBasicList<IPlayListMain>> GetMainPlaylistsAsync();
        Task ClearSongsAsync(int playlist);
        Task DeleteCurrentPlayListAsync(int playlist);
        Task<bool> HasPlaylistCreatedAsync(int playlist);
        //Task<IPlayListMain> GetPlayListMainAsync(int playlist);
        Task SetMainPlaylistAsync(int id);
        Task<CustomBasicCollection<IPlayListDetail>> GetPlaylistDetailsAsync(); //this should already know what the id is at this point.


        Task<int> ChooseSongsAsync(IPlayListDetail detail, int percentage, int howmanySongs);

        Task CreatePlaylistSongsAsync(); //hopefully this simple.

    }
}
