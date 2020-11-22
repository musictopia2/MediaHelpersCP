using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.Extensions;
using CommonBasicStandardLibraries.Exceptions;
using MediaHelpersCP.Music.DB.DataAccess;
using MediaHelpersCP.Music.DB.Models;
using System;
using static MediaHelpersCP.Music.BasicRandomizer.Globals;
using cs = CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses.ConditionOperators;
namespace MediaHelpersCP.Music.PlayListCreater
{
    public class PlayListBehaviors<S> where S : IBaseSong
    {
        public void FillInYears(BasicPlayListData currentObj, ref CustomBasicList<ICondition> tempList, ref bool hadOne,
            Action<CustomBasicList<ICondition>>? action = null) //i think needs to be here so its flexible of when its called.
        {
            if (currentObj.EarliestYear > 0)
            {
                if (currentObj.EarliestYear > currentObj.LatestYear && currentObj.LatestYear > 0)
                {
                    throw new BasicBlankException("The latest year must be greater or equal to the earliest year");
                }

                hadOne = true;
                if (currentObj.LatestYear > 0)
                {
                    tempList.AppendRangeCondition(nameof(IBaseSong.YearSong), currentObj.EarliestYear, currentObj.LatestYear);
                }
                else
                {
                    tempList.AppendCondition(nameof(IBaseSong.YearSong), cs.Equals, currentObj.EarliestYear);
                }

                if (action != null)
                {
                    action.Invoke(tempList);
                }
            }
        }
        public CustomBasicList<ICondition> GetStartingPoint(BasicPlayListData currentObj, IAppendTropicalAccess dats, bool songListCounts, bool anyChristmasCounts, out bool hadOne)
        {
            CustomBasicList<ICondition> tempList = new CustomBasicList<ICondition>(); //maybe no need because it will add to it anyways.
            hadOne = false;
            if (SongsChosen.Count > 0)
            {
                tempList.AppendsNot(SongsChosen);
            }

            if (currentObj.SongList.Count > 0) //if you need more than one song list, then needs to think about that possible issue.
            {
                tempList.AppendContains(currentObj.SongList);
                if (songListCounts == true)
                {
                    hadOne = true;
                }
            }
            if (currentObj.Christmas.HasValue == true)
            {
                if (anyChristmasCounts == true || currentObj.Christmas!.Value == true)
                {
                    hadOne = true;
                }

                tempList.AppendCondition(nameof(IBaseSong.Christmas), currentObj.Christmas!.Value);
            }
            if (currentObj.Artist > 0)
            {
                {
                    hadOne = true;
                    tempList.AppendCondition(nameof(IBaseSong.ArtistID), currentObj.Artist);
                }
            }
            if (currentObj.Romantic == false)
            {
                currentObj.Romantic = null; //if you want non romantic, then rethink
            }

            if (currentObj.Romantic == true)
            {
                hadOne = true;
                tempList.AppendCondition(nameof(IBaseSong.Romantic), true);
            }
            if (currentObj.Tropical == true)
            {
                hadOne = true;
                dats.AppendTropical(tempList);
            }
            if (currentObj.WorkOut == true)
            {
                hadOne = true;
                tempList.AppendCondition(nameof(IBaseSong.WorkOut), true);
            }
            if (currentObj.SpecializedFormat != "")
            {
                hadOne = true;
                if (currentObj.UseLikeInSpecializedFormat == true)
                {
                    tempList.AppendCondition(nameof(IBaseSong.SpecialFormat), cs.Like, currentObj.SpecializedFormat);
                }
                else
                {
                    tempList.AppendCondition(nameof(IBaseSong.SpecialFormat), currentObj.SpecializedFormat);
                }
            }
            if (currentObj.ShowType != "")
            {
                hadOne = true;
                tempList.AppendCondition(nameof(IBaseSong.ShowType), currentObj.ShowType);
            }
            return tempList;
        }
    }
}