using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using MediaHelpersCP.Music.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace MediaHelpersCP.Music.ModelInterfaces
{


	//not sure what we will need.


	//public interface IMusicContext
	//{
	//	Task AddNewPlayListProgressAsync(IPlayListProgress thisProgress, IDbConnection cons, IDbTransaction trans); //you do have to send it in.
	//	Task AddSeveralPlayListSongsAsync(IEnumerable<IPlayListSong> songList); //will have several of them.
	//	IPlayListProgress GetSinglePlayListProgress(int playListID); //this is acceptable
	//	Task ErasePlayListAsync(int playListID); //this is acceptable
	//	IBaseSong GetSong(int id); //this is fine
	//	Task UpdatePlayListProgressAsync(int secs, int songNumber, int playList); //this is fine.
	//	IEnumerable<IPlayListSong> GetPlayListSongs(int playList); //has to be ienumerable unfortunatley.
	//	IEnumerable<IPlayListDetail> GetPlayListDetails(int playList); //we don't have iqueryable.
	//	bool HasPlayListCreated(int playList);
	//	int? CurrentPlayList { get; } //will be read only.
	//	Task SetCurrentPlayListAsync(int? playList);
	//	Task PerformAdvancedMusicProcessAsync(Func<IDbConnection, IDbTransaction, Task> action);
	//	IEnumerable<IBaseSong> GetCompleteSongList(CustomBasicList<ICondition> extraConditions, bool sortByAristSong = false); //i am guessing somebody else has to worry about the rest.
	//	IEnumerable<IArtist> GetSortedArtistList();
	//	void AppendTropical(CustomBasicList<ICondition> conditionList);
	//}
}