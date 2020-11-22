using MediaHelpersCP.Video.ViewModels;

namespace MediaHelpersCP.Video.MiscClasses
{
    public static class VideoGlobal
    {
        //because i think whether its testing or not belongs to the global (bootstrapper will figure that part out).

        public static bool IsTesting { get; set; }
        public static IVideoPlayerViewModel? CurrentVideoVM { get; set; }
    }
}
