using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.Logic
{
    public interface ITelevisionShellLogic
    {
        Task<IEpisodeTable?> GetPreviousShowAsync(); //if it returns nothing, then there was no previous show. 
    }
}