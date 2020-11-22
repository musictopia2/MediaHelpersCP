using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.Logic
{
    public interface ITelevisionLoaderLogic
    {
        //anything needed here.
        //Task FinishMovieAsync(IMainMovieTable selectedMovie);
        //Task UpdateMovieAsync(IMainMovieTable selectedMovie);


        //figure out what is needed for the loader.
        /// <summary>
        /// this needs to not only do a simple function but also do other calculations.
        /// this is used to autoresume the video if needed.
        /// </summary>
        /// <param name="episode"></param>
        /// <returns></returns>
        int GetSeconds(IEpisodeTable episode); //best to have too much than not enough.
        Task UpdateTVShowProgressAsync(IEpisodeTable episode, int position); //hopefully this simple.
        /// <summary>
        /// this also needs to close out of the program as well.
        /// </summary>
        /// <param name="episode"></param>
        /// <returns></returns>
        Task FinishTVEpisodeAsync(IEpisodeTable episode);
        Task AddToHistoryAsync(IEpisodeTable episode);
    }
}
