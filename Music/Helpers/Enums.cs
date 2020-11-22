using System;
using System.Collections.Generic;
using System.Text;
namespace MediaHelpersCP.Music.Helpers
{
    public enum EnumJukeboxSearchOption
    {
        None, Artist, KeyWords, SpecificWords
    }
    //we can try enum portion (?)
    public enum EnumFocusCategory
    {
        Artist, Results
    }
    public enum EnumPlayListOption
    {
        PlaySongsDefault = 1,
        ClearPlayLists,
        DeletePlayLists
    }
}