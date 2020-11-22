using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Video.TelevisionMiscClasses;
using System;
namespace MediaHelpersCP.BasicInterfaces
{
	public interface IVideoPlayer : IBasicMediaPlayer
	{
		int HowLongBeforeRemovingCursor { get; set; }
		void Init();

		//we may not need to set the visible binding anymore.
		//especially if doing via view model first approach.


		//void SetVisibleBinding(string propertyName);
		bool IsCursorVisible();
		event Action MediaEnded;
		bool ProcessingBeginning();
		event Action<string, string> Progress; //hopefully this risk pays off.
		event Action<int> SaveResume;
		int TimeElapsed();
		bool PossibleSkips { get; set; }
		TimeSpan TimeLimit { get; set; }
		double SpeedRatio { get; set; }
		void AddScenesToSkip(CustomBasicList<SkipSceneClass> SkipList);
		bool FullScreen { get; set; }
	}
}