using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaHelpersCP.Music.TemporaryModels
{
    public class JukeboxModel
    {
        public EnumSearchOption SearchOption { get; set; }
        public string SearchTerm { get; set; } = "";
        public bool IsChristmas { get; set; }
    }
}
