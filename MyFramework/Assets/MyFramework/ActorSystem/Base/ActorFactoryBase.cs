using System;

namespace MyFramework.Actor
{
    public abstract class ActorFactoryBase<A> where A : ActorBase
    {
        public ActorFactoryBase()
        {

        }
        public abstract void Create(Action<A> CallBack);

        public abstract void Create(int Id, Action<A> CallBack);
    }
}
