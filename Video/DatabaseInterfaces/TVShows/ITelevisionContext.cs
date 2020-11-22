using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Video.TelevisionMiscClasses;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.DatabaseInterfaces.TVShows
{
	public interface ITelevisionContext
	{
		int Seconds { get; set; }
		IEpisodeTable CurrentEpisode { get; set; }
		CustomBasicList<IEpisodeTable> GetHolidayList(EnumTelevisionHoliday currentHoliday, int currentWeight);
		bool HadPreviousShow();
		Task AddFirstRunViewHistoryAsync();
		Task AddReRunViewHistory();
		CustomBasicList<IShowTable> ListShows(EnumTelevisionCategory televisionCategory);
		void LoadResumeTVEpisodeForReruns();
		Task UpdateEpisodeAsync(); //since i have the object, it will update it.
		Task FinishVideoFirstRunAsync(); //this will handle everything that is needed if its first run.
		IEpisodeTable GetNextFirstRunEpisode(int showID);
		IEpisodeTable GenerateNewRerunEpisode(int showID);
		//i think this is okay after all.
		/// <summary>
		/// this is needed so if you need to manually select an episode for testing or other purposes, that can be done.
		/// looks like needs showid.  otherwise, may be unable to get the full path.
		/// </summary>
		/// <param name="episodeID"></param>
		/// <returns></returns>
		IEpisodeTable GetManuelEpisode(int showID, int episodeID);

	}
}