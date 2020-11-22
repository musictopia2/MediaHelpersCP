using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using MediaHelpersCP.Video.TelevisionMiscClasses;
using System.Threading.Tasks;
namespace MediaHelpersCP.Video.Logic
{
    public class TelevisionListRerunLogic : ITelevisionListLogic
    {
        private readonly ITelevisionContext _data;

        public TelevisionListRerunLogic(ITelevisionContext data)
        {
            _data = data;
        }

        async Task<IEpisodeTable?> ITelevisionListLogic.GetNextEpisodeAsync(IShowTable selectedItem)
        {
            var episode = _data.GenerateNewRerunEpisode(selectedItem.ID);
            if (episode == null)
            {
                await UIPlatform.ShowMessageAsync($"There are no more episodes that can be chosen for {selectedItem.ShowName}");
                UIPlatform.ExitApp();
                return null;
            }
            return episode; //hopefully this simple.
        }

        async Task<CustomBasicList<IShowTable>> ITelevisionListLogic.GetShowListAsync()
        {
            await Task.CompletedTask;
            return _data.ListShows(EnumTelevisionCategory.Reruns);
            //i chose this implementation.
            //however, its possible to create another interface that does not do it.
        }
    }
}
