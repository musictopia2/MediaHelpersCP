using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.Logic
{
    public class TelevisionRerunsLoaderLogic : ITelevisionLoaderLogic
    {
        private readonly ITelevisionContext _data;

        public TelevisionRerunsLoaderLogic(ITelevisionContext data)
        {
            _data = data;
        }

        Task ITelevisionLoaderLogic.AddToHistoryAsync(IEpisodeTable episode)
        {
            _data.CurrentEpisode = episode;
            _data.AddReRunViewHistory();
            return Task.CompletedTask;
        }

        async Task ITelevisionLoaderLogic.FinishTVEpisodeAsync(IEpisodeTable episode)
        {
            episode.ResumeAt = null;
            _data.CurrentEpisode = episode;
            await _data.UpdateEpisodeAsync();
            UIPlatform.ExitApp();
        }

        int ITelevisionLoaderLogic.GetSeconds(IEpisodeTable episode)
        {
            _data.CurrentEpisode = episode;
            int secs = _data.Seconds;

            if (secs == 0 && _data.CurrentEpisode.AlwaysSkipBeginning == true && _data.CurrentEpisode.OpeningLength > 0)
            {
                secs = _data.CurrentEpisode.OpeningLength.Value;
                if (_data.CurrentEpisode.StartAt.HasValue == true)
                    secs += _data.CurrentEpisode.StartAt!.Value;
                _data.Seconds = secs;
            }
            else if (secs == 0 && _data.CurrentEpisode.StartAt.HasValue == true)
            {
                secs = _data.CurrentEpisode.StartAt!.Value;
                _data.Seconds = secs;
            }
            return secs;
        }

        Task ITelevisionLoaderLogic.UpdateTVShowProgressAsync(IEpisodeTable episode, int position)
        {
            _data.CurrentEpisode = episode;
            _data.Seconds = position;
            return Task.CompletedTask;
        }
    }
}
