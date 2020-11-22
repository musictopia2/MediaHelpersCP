using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings; //just in case i need those 2.
using MediaHelpersCP.Music.DB.Models;
//i think this is the most common things i like to do
namespace MediaHelpersCP.Music.Logic.PlaySongs
{
    //TODO:  create the code necessary for the music player's progress.  this time, has to do the parts that specialize in playlist songs.
    public interface IProgressMusicPlayer
    {
        //this is everything that must happen to be a basic music player.
        /// <summary>
        /// this is everything that needs to happen for next song.
        /// if it returns false, then song is no longer playing.
        /// so jukebox can return false
        /// </summary>
        /// <returns></returns>
        Task<bool> NextSongAsync();

        
        
        /// <summary>
        /// something will send in the resumeat which this part needs to decide how it will handle it.
        /// if the interface does not need it, they can just ignore it.
        /// </summary>
        /// <param name="resumeAt"></param>
        /// <returns></returns>
        Task SongInProgressAsync(int resumeAt); //not sure if we need current song or not (?)
        /// <summary>
        /// mostly will be false.  however, for playlist songs, can be true.
        /// </summary>
        




    }
}
