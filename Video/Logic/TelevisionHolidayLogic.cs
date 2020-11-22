using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using MediaHelpersCP.Video.TelevisionMiscClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.Logic
{
    public class TelevisionHolidayLogic : ITelevisionHolidayLogic
    {
        private readonly ITelevisionContext _dats;

        public TelevisionHolidayLogic(ITelevisionContext dats)
        {
            _dats = dats;
        }
        async Task<CustomBasicList<IEpisodeTable>> ITelevisionHolidayLogic.GetHolidayEpisodeListAsync()
        {
            int currentWeight;
            var currentHoliday = DateTime.Now.WhichHoliday();
            await Task.CompletedTask;
            if (currentHoliday == EnumTelevisionHoliday.Christmas)
            {
                currentWeight = 6;
            }
            else
            {
                currentWeight = 4;
            }
            return _dats.GetHolidayList(currentHoliday, currentWeight); //something else has the try catch statement.
        }
    }
}
