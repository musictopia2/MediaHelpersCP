using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.Video.DatabaseInterfaces.Movies;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.Logic
{
    public class MovieLoaderLogic : IMovieLoaderLogic
    {
        private readonly IMovieContext _dats;

        public MovieLoaderLogic(IMovieContext dats)
        {
            _dats = dats;
        }

        Task IMovieLoaderLogic.AddToHistoryAsync(IMainMovieTable selectedMovie)
        {
            return _dats.AddHistoryAsync(selectedMovie);
        }

        async Task IMovieLoaderLogic.FinishMovieAsync(IMainMovieTable selectedMovie)
        {
            selectedMovie.ResumeAt = null;
            await UpdateMovieAsync(selectedMovie);
            UIPlatform.ExitApp();
        }
        private Task UpdateMovieAsync(IMainMovieTable selectedMovie)
        {
            return _dats.UpdateMovieAsync(selectedMovie);
        }
        Task IMovieLoaderLogic.UpdateMovieAsync(IMainMovieTable selectedMovie)
        {
            return UpdateMovieAsync(selectedMovie);
        }
    }
}