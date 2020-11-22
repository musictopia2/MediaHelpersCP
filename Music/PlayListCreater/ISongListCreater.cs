using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
namespace MediaHelpersCP.Music.PlayListCreater
{
    public interface ISongListCreater
    {
        CustomBasicList<ICondition> GetMusicList(BasicPlayListData data); //thanks to dependency injection, for this one, i think the database should be newed up.
    }
}