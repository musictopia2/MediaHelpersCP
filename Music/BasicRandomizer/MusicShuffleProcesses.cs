using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using MediaHelpersCP.Music.DB.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MediaHelpersCP.Music.BasicRandomizer.Globals;
namespace MediaHelpersCP.Music.BasicRandomizer
{
    internal static class Globals
    {
        internal static CustomBasicList<int> SongsChosen { get; set; } = new CustomBasicList<int>(); //when i implement, should make sure that any ids on this list will not be included.
    }
    public class MusicShuffleProcesses : IDisposable
    {
        private readonly CustomBasicList<IBaseSong> _rList = new CustomBasicList<IBaseSong>();
        private CustomBasicList<IBaseSong>? _pList;
        private bool _alreadyHad;
        public int SongsSoFar()
        {
            return _rList.Count();
        }
        public MusicShuffleProcesses()
        {
            SongsChosen.Clear(); //i think
        }
        public CustomBasicList<IBaseSong> SongsChosenForPastSection()
        {
            return _pList!; //decided to taks a risk here.
        }
        public async Task AddSectionAsync(BasicSection thisSection) //now can be all async since the console supports async.
        {
            await Task.Run(() =>
            {
                _alreadyHad = true;
                var firstList = thisSection.SongList!.ToCustomBasicList();
                if (firstList.Count == 0)
                {
                    throw new BasicBlankException("When getting custom list, can't have 0 songs");
                }

                _pList = new CustomBasicList<IBaseSong>();
                var howManySongs = PrivateHowManySongs(thisSection, firstList.Count);
                if (howManySongs == 0)
                {
                    throw new BasicBlankException("PrivateHowManySongs Can't Return 0 Songs");
                }

                firstList.ShuffleList(howManySongs); //i think
                if (firstList.Count == 0)
                {
                    throw new BasicBlankException("Can't have 0 songs after shuffling.  Rethink");
                }

                firstList.ForEach(items =>
                {
                    SongsChosen.Add(items.ID); //i think
                });
                _pList = firstList;
                _rList.AddRange(firstList);
            });
        }
        private int PrivateHowManySongs(BasicSection thisS, int songCount)
        {
            int temps = songCount.MultiplyPercentage(thisS.Percent);
            if (thisS.HowManySongs == 0)
            {
                thisS.ChooseAll = true;
            }

            if (thisS.ChooseAll == true)
            {
                if (thisS.Percent == 0)
                {
                    return songCount;
                }

                return temps;
            }
            if (thisS.HowManySongs > temps)
            {
                return temps;
            }

            return thisS.HowManySongs;
        }
        public async Task<ICustomBasicList<IBaseSong>> GetRandomListAsync()
        {
            ICustomBasicList<IBaseSong>? tempList = null;
            await Task.Run(() =>
            {
                SongsChosen.Clear();
                if (_alreadyHad == true)
                {
                    _alreadyHad = false;
                    tempList = _rList.ToCustomBasicList(); //to get a new one.
                    return;
                }
                tempList = _rList.GetRandomList(); //i think this would work too.
                return;
            });
            return tempList!; //decided to risk it.
        }
        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
               

                _disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}