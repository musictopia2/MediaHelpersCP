using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace MediaHelpersCP.BasicInterfaces
{
    public interface IBasicMediaPlayer
	{
		bool IsFinished();
		int Length();
		string Path { get; set; }
		void SetPathBinding(string path);
		int Volume { get; set; }
		int DefaultVolume { get; set; }
		int MaxVolume { get; }
		Task PlayAsync();
		Task PlayAsync(int position);
		Task PlayAsync(int length, int position);
		void Pause();
		void StopPlay(); //looks like i did need this as well.  i must have forgotten this.
		bool IsPaused();
		int TimeElapsedSeconds();
		event ErrorRaisedEventHandler ErrorRaised;
	}
}