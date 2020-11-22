using System.Threading.Tasks;
namespace MediaHelpersCP.Music.UIInterfaces
{
	public interface IValidationErrors //i think still needed
	{
		void ShowMessage(string thisMessage);
	}

	//lots of rethinking is required here now.

	//may not need the ijukebox.  rethinking may be required instead.

	//public interface IJukeBox
	//{
	//	void ArtistListChanged();
	//	void SongResultsChanged();
	//	void PlayListChanged();
	//	void ClearSongCombo();
	//}
	//public interface IPlayListNotifiers
	//{
	//	void MainListChanged();
	//	void FocusOnSection();
	//	void SubListChanged();
	//}
	//public interface ISongProgress
	//{
	//	Task ShowSongProgress();
	//	void MediaErrorMessage(string message);
	//}
	//public interface IHandPickers
	//{
	//	void GetNewLists();
	//	void Finished();
	//	void NoneToBeginWith();
	//}
	//public interface ISimplePlayListCreater
	//{
	//	void ScreenChanged();
	//	void LoadLists();
	//	void ListSongs();
	//	void PickerListChanged();
	//	void ListArtists();
	//	void ClearSelectedSong();
	//	void CreatedPickedLists();
	//}
	//public enum EnumFocusCategory
	//{
	//	Song = 1, Artist
	//}
	//public interface IImportUI
	//{
	//	void Focus(EnumFocusCategory category);
	//	void ShowLabel(string message);
	//}
}