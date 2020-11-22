using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;

namespace MediaHelpersCP.Music.DB.DataAccess
{
    public interface IAppendTropicalAccess
    {
        void AppendTropical(CustomBasicList<ICondition> conditionList);
    }
}