using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
//i think this is the most common things i like to do
namespace MediaHelpersCP.Video.ViewModels
{
    public abstract class MainVideoListViewModel<V> : Screen
        where V : class
    {


        public abstract string ListTitle { get; }

        public MainVideoListViewModel(IEventAggregator aggregator)
        {
            Aggregator = aggregator;
        }

        private V? _selectedItem;

        public V? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    //can decide what to do when property changes
                    SelectedItemChanged();
                }

            }
        }

        protected virtual void SelectedItemChanged() //i like this idea.
        {
            NotifyOfCanExecuteChange(nameof(CanChooseVideo));
        }

        //this only focuses on the video list.  something else handles the rest.



        private CustomBasicList<V> _videoList = new CustomBasicList<V>();

        public CustomBasicList<V> VideoList
        {
            get { return _videoList; }
            set
            {
                if (SetProperty(ref _videoList, value))
                {
                    //can decide what to do when property changes
                }

            }
        }
        /// <summary>
        /// this is when choosing manually.  if you choose a different way like autoresume last movie, then its another command.  something else
        /// is responsible for that process.
        /// </summary>
        /// <returns></returns>
        public abstract Task ChooseVideoAsync();

        public bool CanChooseVideo => SelectedItem != null;

        protected IEventAggregator Aggregator { get; }
    }
}
