using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.Conductors;
using MediaHelpersCP.Music.EventModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using static MediaHelpersCP.Music.Helpers.GlobalStaticVariableClass;

namespace MediaHelpersCP.Music.ViewModels
{
    public class PlaylistShellViewModel : ConductorCollectionSingleActive<object>,
		IHandleAsync<PlayListSongBuilderEventModel>,
		IHandleAsync<StartPlayingPlayListEventModel>,
		IHandle<OpenSubPlaylistEventModel>
    {


		public PlaylistShellViewModel()
		{
			if (Aggregator == null)
			{
				throw new BasicBlankException("Needs event aggregator before doing the progress");
			}
			//was not fine because something else needed it before this point.
			Aggregator.Subscribe(this);
			//this will be doing both playlist songs and creating a brand new playlist.

			DisplayName = "Complete Playlist App";
		}

		public bool CanOpenPlayListSongs => !IsOpened(typeof(PlaylistSongDashboardViewModel));


		public async Task OpenPlayListSongsAsync()
		{
			var model = cons!.Resolve<PlaylistSongDashboardViewModel>();
			await LoadScreenAsync(model);
			TryOpenCreater();
		}
        public bool CanOpenPlayListCreater { get; private set; }
        public async Task OpenPlayListCreaterAsync()
		{
			if (CanOpenPlayListCreater == false)
			{
				throw new BasicBlankException("Should not have been able to open creater because it was false.  Rethink");
			}
			var model = cons!.Resolve<IShellPlaylistCreaterViewModel>();
			await LoadScreenAsync(model);
			CanOpenPlayListCreater = false;
			NotifyOfCanExecuteChange(nameof(NotifyOfCanExecuteChange)); //i think.
		}

		protected override async Task ActivateAsync()
		{
			await base.ActivateAsync();
			await OpenPlayListSongsAsync(); //default to opening the playlistsongs.
			TryOpenCreater();
		}

		async Task IHandleAsync<PlayListSongBuilderEventModel>.HandleAsync(PlayListSongBuilderEventModel message)
		{
			//this means i have to register 2 more view models.
			//this could be a good time to autoregister.  i already do for the views.
			//might as well for the view models as well.  view models has to always be instance.
			//has to decide whether i do or not (could be an option to autoregister as well).
			var model = cons!.Resolve<PlaylistSongBuilderViewModel>();
			await LoadScreenAsync(model);
		}

		async Task IHandleAsync<StartPlayingPlayListEventModel>.HandleAsync(StartPlayingPlayListEventModel message)
		{
			var model = cons!.Resolve<PlaylistSongProgressViewModel>();
			await LoadScreenAsync(model);
			//hopefully we don't need the playlist main anymore (?)



			//var model = new PlaylistSongProgressViewModel()
		}

		private void TryOpenCreater()
		{
			CanOpenPlayListCreater = cons!.RegistrationExist<IShellPlaylistCreaterViewModel>();
		}

		void IHandle<OpenSubPlaylistEventModel>.Handle(OpenSubPlaylistEventModel message)
		{
			TryOpenCreater();
		}

		//lots of communication goes to the shell so the shell can decide what else to open.
		//this means for this version, you do have to open the progress view model.
		//this would also be responsible for opening all the screens for playlist songs.
		//that way when on any screen, can easily go back to the main page.




	}
}
