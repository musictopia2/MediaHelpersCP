namespace MediaHelpersCP.Music.DB.Models
{
	public interface IPlayListDetail
	{
		int ID { get; set; } //now we need this one too.
		string Description { get; set; } //we still needed those 2 because of bindings for views.
		string JsonData { get; set; }
	}
}