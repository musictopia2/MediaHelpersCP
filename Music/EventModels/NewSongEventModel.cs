using MediaHelpersCP.Music.DB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaHelpersCP.Music.EventModels
{
    public class NewSongEventModel
    {
        public IBaseSong CurrentSong { get; }
        public int ResumeAt { get; }
        public NewSongEventModel(IBaseSong currentSong, int resumeAt)
        {
            CurrentSong = currentSong;
            ResumeAt = resumeAt;
        }
    }
}
