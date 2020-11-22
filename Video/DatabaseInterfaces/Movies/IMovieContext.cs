using CommonBasicStandardLibraries.CollectionClasses;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.DatabaseInterfaces.Movies
{
    public interface IMovieContext
	{
		Task AddHistoryAsync(IMainMovieTable thisMovie); //this will need to auto update the movie but also add to history
		CustomBasicList<IMainMovieTable> GetMovieList(EnumMovieSelectionMode selectionMode, bool isChristmas);
		Task UpdateMovieAsync(IMainMovieTable ThisMovie);
	}
}