using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using MediaHelpersCP.Music.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaHelpersCP.Music.DB.DataAccess
{
    public interface ISimpleMusicDataAccess : IAppendTropicalAccess
    {
        //this is all the stuff for simple music data access.
        IBaseSong GetSong(int id); //this is fine

        
        IEnumerable<IBaseSong> GetCompleteSongList(CustomBasicList<ICondition> extraConditions, bool sortByAristSong = false);
        IEnumerable<IArtist> GetSortedArtistList();
        
    }
}