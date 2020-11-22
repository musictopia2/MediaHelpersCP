using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Video.Logic;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
//i think this is the most common things i like to do
namespace MediaHelpersCP.Video.ViewModels
{
    public class MovieShellViewModel : VideoShellViewModel, IHandleAsync<MovieListViewModel>
    {
        private readonly IVideoPlayer _player;
        private readonly IMovieLoaderLogic _loader;
        public MovieShellViewModel(IVideoPlayer player, IMovieLoaderLogic loader)
        {
            DisplayName = "Movie Player Version 7";
            _player = player;
            _loader = loader;
        }
        
        protected override async Task ActivateAsync()
        {
            await base.ActivateAsync();
            var model = GetViewModel<MovieListViewModel>();
            await LoadScreenAsync(model);
        }

        Task IHandleAsync<MovieListViewModel>.HandleAsync(MovieListViewModel message)
        {
            if (message.SelectedItem == null)
            {
                throw new BasicBlankException("Must have a selected item in order to load the loader for movie view model");
            }
            var model = new MovieLoaderViewModel(message.SelectedItem, _player, _loader);
            return LoadScreenAsync(model);
        }
    }
}