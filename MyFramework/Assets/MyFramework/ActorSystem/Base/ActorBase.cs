using System.Collections.Generic;

namespace MyFramework.Actor
{
    public interface IActorBase
    {
        void Init();
        CP AddComp<CP>(params object[] _Agr) where CP : ActorCompBase, new();
        void RemoveComp(ActorCompBase Comp);
        void Destroy();
    }

    public abstract class ActorBase : IActorBase
    {
        protected IActorDataBase Config;
        protected List<ActorCompBase> MyComps = new List<ActorCompBase>();

        public ActorBase(IActorDataBase _Config)
        {
            Config = _Config;
        }

        public virtual void Init()
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Init();
            }
        }
        public virtual CP AddComp<CP>(params object[] _Agr) where CP : ActorCompBase, new()
        {
            CP Comp = new CP();
            Comp.Load(this, _Agr);
            MyComps.Add(Comp);
            return Comp;
        }
        public virtual void RemoveComp(ActorCompBase Comp)
        {
            MyComps.Remove(Comp);
            Comp.Destroy();
        }
        public void Destroy()
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Destroy();
            }
        }
    }

    public abstract class ActorBase<D> : ActorBase where D: IActorDataBase
    {
        protected new D Config;

        public ActorBase(D _Config)
            :base(_Config)
        {
            Config = _Config;
        }

    }

}