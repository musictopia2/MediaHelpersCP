using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Music.Helpers;
using MediaHelpersCP.Music.TemporaryModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Music.Logic.PlaySongs
{
    public interface IJukeboxLogic
    {
        //this handles everything that is needed for the logic class for jukebox.

        CustomBasicCollection<ArtistResult> GetArtistList(bool isChristmas);
        CustomBasicCollection<SongResult> GetSongList(EnumJukeboxSearchOption searchOption, ArtistResult? artistChosen, bool isChristmas, string searchTerm);
        //i do like this being available to hook to.  is needed for the clients to access.
        CustomBasicCollection<SongResult> SongsToPlay { get; set; } //needs both so can use in binding.


        Task AddSongToListAsync(SongResult song);
        Task RemoveSongFromListAsync(SongResult song); //i think this is what will be removed.

        //i think this is everything having to do with jukebox.

    }
}
