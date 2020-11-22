using CommonBasicStandardLibraries.Messenging;
using MediaHelpersCP.Music.DB.Models;
using MediaHelpersCP.Music.EventModels;
using MediaHelpersCP.Music.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MediaHelpersCP.Music.Helpers.GlobalStaticVariableClass;
namespace MediaHelpersCP.Music.Helpers
{
    public static class Extensions
    {
        public static Task PlayNewSongAsync(this IBaseSong song, int resumeAt)
        {
            return Aggregator!.PublishAsync(new NewSongEventModel(song, resumeAt));
        }
        public static Task ChangeSongAsync()
        {
            return Aggregator!.PublishAsync(new ChangeSongEventModel());
        }
        public static void JukeboxFocus(this EnumFocusCategory focus)
        {
            Aggregator!.Publish(focus);
        }

        //public static Task ShowCurrentSongPlayedAsync(this IBaseSong song)
        //{
        //    return Aggregator!.PublishAsync(song);
        //}

        //public static void ShowResumeAt(this int resumeAt)
        //{
        //    Aggregator!.Publish(resumeAt);
        //}

        //public static Task SelectCurrentSongAsync(this IEventAggregator aggregator, IBaseSong song)
        //{
        //    return aggregator.PublishAsync(song); //hopefully this simple.
        //}
    }
}
