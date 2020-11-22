using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using MediaHelpersCP.Video.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.ViewModels
{
    public class TelevisionListViewModel : MainVideoListViewModel<IShowTable>
    {
        private readonly ITelevisionListLogic _logic;

        public TelevisionListViewModel(IEventAggregator aggregator, ITelevisionListLogic logic) : base(aggregator)
        {
            _logic = logic;
        }

        //this is for you to choose a show.

        //this is also responsible for figuring out what episode to watch as well.

        //has to figure out what part to use.
        //most likely will have one interface but 2 different implementations.
        //so one for reruns and another for firstruns.
        //can create others if you want as well.

        public IEpisodeTable? EpisodeChosen { get; private set; } //needs to be here.

        public override string ListTitle => "List Of Series (Reruns)";

        public override async Task ChooseVideoAsync()
        {
            if (SelectedItem == null)
            {
                throw new BasicBlankException("You never chose a show to watch.  Rethink");
            }
            EpisodeChosen = await _logic.GetNextEpisodeAsync(SelectedItem); //this is responsible for figuring out which episode it is which gets sent to one to actually watch
            if (EpisodeChosen == null)
            {
                return;
            }
            await Aggregator.PublishAsync(this);
        }
        protected override async Task ActivateAsync()
        {
            await base.ActivateAsync();
            //figure out the loading of the video list.
            VideoList = await _logic.GetShowListAsync();
            FocusOnFirst();
        }
    }
}