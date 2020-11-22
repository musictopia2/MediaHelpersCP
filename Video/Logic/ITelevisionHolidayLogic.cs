using CommonBasicStandardLibraries.CollectionClasses;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.Logic
{
    public interface ITelevisionHolidayLogic
    {
        Task<CustomBasicList<IEpisodeTable>> GetHolidayEpisodeListAsync();
    }
}