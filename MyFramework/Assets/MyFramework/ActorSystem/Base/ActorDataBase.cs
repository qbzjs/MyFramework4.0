using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Actor
{
    public interface IActorDataBase { }

    public class ActorDataBase : ConfigDataBase, IActorDataBase
    {

    }

    public class ActorDataBase<Id> : ConfigDataBase<Id>, IActorDataBase
    {

    }

    public class ActorTableDataBase<Id,A> : ConfigTableDataBase<Id, A> where A : ActorDataBase<Id>
    {
        
    }
}
