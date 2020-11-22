using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Video.DatabaseInterfaces.Movies;
using MediaHelpersCP.Video.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.Logic
{
    public interface IMovieListLogic
    {
        //this is just the things needed for the movie list.
        //decided to risk not mocking christmas stuff.
        Task<CustomBasicList<IMainMovieTable>> GetMovieListAsync(EnumMovieSelectionMode selectionMode);

        //Task StartLoadingVideoAsync(IMainMovieTable selectedItem, MovieListViewModel model);
        //i think the logic one can go ahead and send to the shell view model to load the movie as well.
        /// <summary>
        /// this returns the last movie.  could even be null if there was no last movie watched.
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        IMainMovieTable? GetLastMovie(CustomBasicList<IMainMovieTable> movies);

    }
}
