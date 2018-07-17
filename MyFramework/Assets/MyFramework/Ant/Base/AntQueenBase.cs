using System.Collections.Generic;
/// <summary>
/// 蚁巢框架 蚁后
/// </summary>
namespace MyFramework
{
    public interface IAntQueenBase
    {
        void MakeAnt();
    }

    public abstract class AntQueenBase<A> : AntBase,IAntQueenBase where A: AntBase
    {
        private List<A> Childs;

        public AntQueenBase()
        {
            Childs = new List<A>();
        }

        public abstract void MakeAnt();
    }
}
