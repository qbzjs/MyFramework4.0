using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Actor
{
    public abstract class ActorCompBase
    {
        protected ActorBase Actor;

        public virtual void Load(ActorBase _Actor, params object[] _Agr)
        {
            Actor = _Actor;
        }

        public abstract void Init();
        public abstract void Destroy();
    }
    public abstract class ActorCompBase<A> : ActorCompBase where A : ActorBase
    {
        protected new A Actor;

        public override void Load(ActorBase _Actor, params object[] _Agr)
        {
            base.Load(Actor, _Agr);
            Actor = _Actor as A;
        }
    }
}
