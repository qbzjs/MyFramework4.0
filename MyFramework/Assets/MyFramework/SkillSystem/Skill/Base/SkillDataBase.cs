using MyFramework;

namespace MyFramework.SkillSystem
{
    public interface ISkillDataBase { }

    public class SkillDataBase : ConfigDataBase, ISkillDataBase
    {
    }


    public class SkillDataBase<Id> : ConfigDataBase<Id>, ISkillDataBase
    {

    }

    public class SkillTableDataBase<Id, A> : ConfigTableDataBase<Id, A> where A : SkillDataBase<Id>
    {

    }
}
