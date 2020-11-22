using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.Video.DatabaseInterfaces.Movies;
using MediaHelpersCP.Video.Logic;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.ViewModels
{
	public class MovieListViewModel : MainVideoListViewModel<IMainMovieTable>
	{
		private readonly IMovieListLogic _logic; //this means i can do a mock list even if i chose for movies.

		public MovieListViewModel(IEventAggregator aggregator, IMovieListLogic logic) : base(aggregator)
		{
			_logic = logic;
		}
		private EnumMovieSelectionMode _selectionMode = EnumMovieSelectionMode.NewMovies; //decided to default to new movies

		public EnumMovieSelectionMode SelectionMode
		{
			get { return _selectionMode; }
			set
			{
				if (SetProperty(ref _selectionMode, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private IMainMovieTable? _lastMovie;

		public override Task ChooseVideoAsync()
		{
			//this is for a movie.
			//will send a message with this view model.  since i don't think anything else would need it for this case.
			if (SelectedItem == null)
			{
				throw new BasicBlankException("You needed a selected movie in order to load video.  Rethink");
			}
			//no need for extenion that is only used once.
			return Aggregator.PublishAsync(this);
		}

		public async Task GetMovieListAsync()
		{
			VideoList = await _logic.GetMovieListAsync(SelectionMode);
			_lastMovie = _logic.GetLastMovie(VideoList);
			CanAutoResume = _lastMovie != null;
			NotifyOfCanExecuteChange(nameof(CanAutoResume));
			NotifyOfCanExecuteChange(nameof(CanShowLastWatched));
			NotifyOfCanExecuteChange(nameof(CanShowInfoLast));
			//since this is a command, then should be okay for the other functions.
			FocusOnFirst();
		}
		protected override async Task ActivateAsync()
		{
			await base.ActivateAsync();
			await GetMovieListAsync(); //i think same thing for tv shows.
		}
		protected override void SelectedItemChanged()
		{
			base.SelectedItemChanged();
			NotifyOfCanExecuteChange(nameof(CanShowLastWatched));
		}
        public bool CanAutoResume { get; private set; }

        public async Task AutoResumeAsync()
		{
			if (_lastMovie == null)
			{
				throw new BasicBlankException("Cannot autoresume movie because last movie was nothing.  Rethink");
			}
			SelectedItem = _lastMovie;
			await ChooseVideoAsync();
		}
		public bool CanShowLastWatched => CanChooseVideo;
		public async Task ShowLastWatchedAsync()
		{
			if (SelectedItem == null)
			{
				throw new BasicBlankException("You never selected the movie to show the details.  Rethink");
			}
			if (SelectedItem.LastWatched.HasValue == false)
			{
				await UIPlatform.ShowMessageAsync("You never watched this movie before");
				return;
			}
			await UIPlatform.ShowMessageAsync($"The last time you watched the movie was {SelectedItem.LastWatched!.Value}");
		}
		public bool CanShowInfoLast => CanAutoResume;

		public override string ListTitle => "List Of Movies";

		public async Task ShowInfoLastAsync()
		{
			if (_lastMovie == null)
			{
				throw new BasicBlankException("Cannot show last info because was nothing.  Rethink");
			}
			await UIPlatform.ShowMessageAsync($"The Last Movie You Need To Watch Was {_lastMovie.Title}");
		}
	}
}