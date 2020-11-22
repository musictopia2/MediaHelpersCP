using MediaHelpersCP.Music.DB.Models;
using System;
using System.Threading.Tasks;
namespace MediaHelpersCP.Music.Logic.PlaySongs
{
    //the songprogressviewmodel will usually call into it so it can invoke the action which tells the songlist to update its bindings.
    public interface IPrepareSong
    {
        /// <summary>
        /// the action sent is code that will run when this method invokes the action.
        /// usually will show that properties has changed so ui can do what it needs to do.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<bool> PrepareSongAsync(Action changePropertyMethod, IBaseSong currentSong, int resumeAt);
    }
}