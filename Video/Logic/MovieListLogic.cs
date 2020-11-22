using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Video.DatabaseInterfaces.Movies;
using MediaHelpersCP.Video.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.Logic
{
    public class MovieListLogic : IMovieListLogic
    {
        private readonly IMovieContext _dats;

        public MovieListLogic(IMovieContext dats)
        {
            _dats = dats;
        }

        IMainMovieTable? IMovieListLogic.GetLastMovie(CustomBasicList<IMainMovieTable> movies)
        {
            var tempList = movies.GetConditionalItems(Items => Items.LastWatched.HasValue == true);
            //PrivateList.RemoveAllOnly(Items => Items.LastWatched.HasValue == false);
            tempList.Sort(comparison: (xx, yy) =>
            {
                return yy.LastWatched!.Value.CompareTo(xx.LastWatched!.Value);
            });
            IMainMovieTable output = tempList.Find(Items => Items.ResumeAt.HasValue == true
            || Items.Opening.HasValue == false && Items.Closing.HasValue == false);
            return output;
        }

        Task<CustomBasicList<IMainMovieTable>> IMovieListLogic.GetMovieListAsync(EnumMovieSelectionMode selectionMode)
        {
            //has to figure out christmas.
            DateTime thisDate = DateTime.Now;
            bool isChristmas = thisDate.IsBetweenThanksgivingAndChristmas();
            return Task.FromResult(_dats.GetMovieList(selectionMode, isChristmas));
        }
        //i think the view model should be responsible for sending to new list after all.

        //Task IMovieListLogic.StartLoadingVideoAsync(IMainMovieTable selectedItem, MovieListViewModel model)
        //{
        //    //this is the movie chosen.  will send a message 

        //}
    }
}
