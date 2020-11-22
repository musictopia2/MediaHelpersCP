namespace MediaHelpersCP.BasicInterfaces
{
    public interface IMP3Player : IBasicMediaPlayer
	{
		string TimeElapsedLabel();
		string TotalInLabel();
	}
}