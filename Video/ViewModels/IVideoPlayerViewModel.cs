using System;
using System.Collections.Generic;
using System.Text;

namespace MediaHelpersCP.Video.ViewModels
{
    public interface IVideoPlayerViewModel
    {
        bool PlayButtonVisible { get; set; }
        bool CloseButtonVisible { get; set; }
        bool FullScreen { get; set; }
    }
}
