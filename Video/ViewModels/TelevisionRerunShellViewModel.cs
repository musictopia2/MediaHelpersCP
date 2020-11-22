using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using MediaHelpersCP.BasicInterfaces;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using MediaHelpersCP.Video.EventModels;
using MediaHelpersCP.Video.Logic;
using MediaHelpersCP.Video.TelevisionMiscClasses;
using System;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.ViewModels
{
    public class TelevisionRerunShellViewModel : VideoShellViewModel,
        IHandleAsync<TelevisionListViewModel>,
        IHandleAsync<IEpisodeTable>,
        IHandleAsync<LoadTelevisionListEventModel>
    {
        private readonly IVideoPlayer _player;
        private readonly ITelevisionShellLogic _logic;
        private readonly ITelevisionLoaderLogic _loader;


        private bool _hadPreviousShow;
        EnumTelevisionHoliday _currentHoliday; //this is important because it may load something else.

        public TelevisionRerunShellViewModel(IVideoPlayer player, ITelevisionShellLogic logic, ITelevisionLoaderLogic loader)
        {
            DisplayName = "Simple Reruns Version 6";
            _player = player;
            _logic = logic;
            _loader = loader;
        }
        private bool CanStartLoadingLists() //done i think.
        {
            if (_hadPreviousShow == true)
            {
                return false;
            }
            return _currentHoliday == EnumTelevisionHoliday.None;
        }

        private async Task LoadTelevisionListAsync()
        {
            var model = GetViewModel<TelevisionListViewModel>();
            await LoadScreenAsync(model);
        }

        protected override async Task ActivateAsync()
        {
            //this time, probably has to manually open the next view model.
            //because if there are other pieces of information needed, then
            //this needs to figure it out and pass it on.
            //could send a model as well.  well see what happens.
            //this is the start.
            //if there are any processes needed, will ask at the beginning.


            IEpisodeTable? previousEpisode = await _logic.GetPreviousShowAsync();

            _hadPreviousShow = previousEpisode != null;
            _currentHoliday = EnumTelevisionHoliday.None;
            if (_hadPreviousShow == false)
            {
                _currentHoliday = DateTime.Now.WhichHoliday();
            }
            if (CanStartLoadingLists())
            {
                await LoadTelevisionListAsync();
                return;
            }
            if (_hadPreviousShow)
            {
                if (previousEpisode == null)
                {
                    throw new BasicBlankException("Cannot load previous show because it was null.  Really rethink");
                }
                await LoadEpisodeAsync(previousEpisode);
                return;
            }
            var model = GetViewModel<TelevisionHolidayViewModel>();
            await LoadScreenAsync(model);
        }

        private async Task LoadEpisodeAsync(IEpisodeTable episode)
        {
            TelevisionLoaderViewModel model = new TelevisionLoaderViewModel(episode, _player, _loader); //we may need one more thing
            await LoadScreenAsync(model);
        }

        Task IHandleAsync<TelevisionListViewModel>.HandleAsync(TelevisionListViewModel message)
        {
            if (message.EpisodeChosen == null)
            {
                throw new BasicBlankException("No episode was chosen to load.  Rethink");
            }
            return LoadEpisodeAsync(message.EpisodeChosen);
        }

        Task IHandleAsync<IEpisodeTable>.HandleAsync(IEpisodeTable message)
        {
            return LoadEpisodeAsync(message);
        }

        //was going to have as a funct.  the only problem is can't call into the class
        //could set a private but i think this other way is fine too.

            

        Task IHandleAsync<LoadTelevisionListEventModel>.HandleAsync(LoadTelevisionListEventModel message)
        {
            return LoadTelevisionListAsync();
        }
    }
}