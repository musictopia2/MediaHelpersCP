using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Video.MiscClasses;
using System;
using System.Threading.Tasks; 
namespace MediaHelpersCP.Video.ViewModels
{
	public abstract class VideoMainLoaderViewModel<V> : Screen, IVideoPlayerViewModel
		where V : class
	{
		//this is responsible for loading the video.

		//for now, no remote control.
		//if we ever added it, then will be here as well.


		public VideoMainLoaderViewModel(V selectedItem, IVideoPlayer player)
		{
			SelectedItem = selectedItem;
			Player = player;
			VideoGlobal.CurrentVideoVM = this;
			//well see what else is needed
		}

		private int _videoLength;

		public int VideoLength
		{
			get { return _videoLength; }
			set
			{
				if (SetProperty(ref _videoLength, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private int _videoPosition;

		public int VideoPosition
		{
			get { return _videoPosition; }
			set
			{
				if (SetProperty(ref _videoPosition, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _videoPath = "";

		public string VideoPath
		{
			get { return _videoPath; }
			set
			{
				if (SetProperty(ref _videoPath, value))
				{
					//can decide what to do when property changes
					Player.Path = value;
				}

			}
		}

		private int _resumeSecs;

		public int ResumeSecs
		{
			get { return _resumeSecs; }
			set
			{
				if (SetProperty(ref _resumeSecs, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _fullScreen;

		public bool FullScreen
		{
			get { return _fullScreen; }
			set
			{
				if (VideoGlobal.IsTesting == true)
					value = false;
				if (SetProperty(ref _fullScreen, value))
				{
					//can decide what to do when property changes
					if (Player == null)
						return;
					Player.FullScreen = value;

				}

			}
		}
		//has to be public after all.  since that may have to be used to influence the display of the video like button 3.
		public V SelectedItem { get; protected set; } //inherited versions has to be able to set so they can load another video later like tim coreys videos.
		protected IVideoPlayer Player { get; }

		protected override async Task ActivateAsync()
		{
			await base.ActivateAsync();
			//needs to see what needs to be done.  probably an abstract class here.
			await BeforePlayerInitAsync();
			Player.Init();
			await AfterPlayerInitAsync();
		}
		protected virtual Task BeforePlayerInitAsync()
		{
			Player.ErrorRaised += ThisPlayer_ErrorRaised;
			Player.SaveResume += ThisPlayer_SaveResume;
			Player.Progress += ThisPlayer_Progress;
			Player.MediaEnded += ThisPlayer_MediaEnded;
			return Task.CompletedTask;
		}
		protected virtual Task AfterPlayerInitAsync() //this is after the player initialized.
		{
			return Task.CompletedTask;
		}
		protected async Task ShowVideoLoadedAsync()
		{
			if (VideoLength == 0)
				VideoLength = Player.Length();
			if (VideoLength == -1)
			{
				UIPlatform.ShowError("Movie Length can't be -1");
				return;
			}
			FullScreen = true;
			await Player.PlayAsync(VideoLength, VideoPosition);
		}


		private bool _playButtonVisible;
		public bool PlayButtonVisible
		{
			get { return _playButtonVisible; }
			set
			{
				if (SetProperty(ref _playButtonVisible, value))
				{

					NotifyOfCanExecuteChange(nameof(CanPlayPause)); //can has to be given if doing new snippet.
				}
			}
		}
		private bool _closeButtonVisible = true;
		public bool CloseButtonVisible
		{
			get { return _closeButtonVisible; }
			set
			{
				if (SetProperty(ref _closeButtonVisible, value))
				{
					NotifyOfCanExecuteChange(nameof(CanCloseScreen));
				}
			}
		}
		public bool CanPlayPause => PlayButtonVisible;
		public void PlayPause()
		{
			Player.Pause(); //i think.
		}
		public bool CanCloseScreen => CloseButtonVisible;
		public async Task CloseScreenAsync()
		{
			await SaveProgressAsync();
			UIPlatform.ExitApp();
		}

		public abstract Task SaveProgressAsync();
		public abstract Task VideoFinishedAsync();

		//not sure if endloaded was needed or not (?)
		//private bool _endLoaded;

		//public bool EndLoaded
		//{
		//	get { return _endLoaded; }
		//	set
		//	{
		//		if (SetProperty(ref _endLoaded, value))
		//		{
		//			//can decide what to do when property changes
		//		}

		//	}
		//}

		private async void ThisPlayer_MediaEnded()
		{
			await VideoFinishedAsync();
		}
		private int _attempts;
		private async void ThisPlayer_Progress(string timeElapsed, string totalTime)
		{
			try
			{
				int els = Player!.TimeElapsed();
				els += 3;
				if (els < ResumeSecs && ResumeSecs > 5 && els <= 5)
				{
					_attempts++;
					if (_attempts > 10)
					{
						UIPlatform.ShowError("Somehow; its failing to autoresume no matter what.  Already tried 10 times.  Therefore; its being closed out");
						return;
					}

					await Player.PlayAsync(VideoLength, ResumeSecs);
				}
				VideoPosition = Player.TimeElapsed();
				ProgressText = $"{timeElapsed}/{totalTime}";
			}
			catch (Exception ex)
			{
				UIPlatform.ShowError(ex.Message); //for now, if there is an error on reloading, will be forced to show error and close out to see what the problem could be.
			}
		}
		private async void ThisPlayer_SaveResume(int newSecs) //hopefully this is okay
		{
			VideoPosition = newSecs;
			await SaveProgressAsync();
		}
		private void ThisPlayer_ErrorRaised(string message)
		{
			UIPlatform.ShowError(message);
		}

		private string _progressText = "";

		public string ProgressText
		{
			get { return _progressText; }
			set
			{
				if (SetProperty(ref _progressText, value))
				{
					//can decide what to do when property changes
				}

			}
		}
	}
}
