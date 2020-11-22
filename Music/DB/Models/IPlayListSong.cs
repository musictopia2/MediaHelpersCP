namespace MediaHelpersCP.Music.DB.Models
{
	public interface IPlayListSong
	{
		int ID { get; set; }
		int PlayList { get; set; }
		int SongID { get; set; }
		int SongNumber { get; set; }
		IBaseSong Song { get; } //i thought it was iffy but it somehow worked out
	}
}