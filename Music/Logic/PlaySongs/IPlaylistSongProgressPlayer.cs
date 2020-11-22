using MediaHelpersCP.Music.DB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Music.Logic.PlaySongs
{
    //TODO:  implement interface for the part that focuses on songprogressplayer.  most likely, will have another interface too (not sure though).
    public interface IPlaylistSongProgressPlayer : IProgressMusicPlayer
    {
        int UpTo { get; set; }
        int SongsLeft { get; set; }


        Action UpdateProgress { get; set; } //hopefully this works as well.

        Task StartUpAsync();


        //maybe these 2 won't be needed (not sure though).
        //for now, better be safe than sorry.
        //IPlayListMain MainPlayList { get; set; } //hopefully this is good as well.
        //Task StartPlayingAsync(); //since this probably has to be global, then this can set the mainplaylist and then call the StartPlayingAsync
    }
}