using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.Conductors;
using System;
using System.Collections.Generic;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace MediaHelpersCP.Video.ViewModels
{
    /// <summary>
    /// this is the starting point for the shell  everything needs to override from here.
    /// </summary>
    public abstract class VideoShellViewModel : ConductorCollectionSingleActive<object>
    {
        //i don't think that having 3 generic arguments make sense for this one.

        //protected IEventAggregator Aggregator;

        protected VM GetViewModel<VM>()
        {
            return cons!.Resolve<VM>();
        }

        public VideoShellViewModel()
        {
            IEventAggregator aggregator = cons!.Resolve<IEventAggregator>(); //so i don't need constructors.
            aggregator.Subscribe(this);
        }
    }
}
