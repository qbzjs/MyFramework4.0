using MyFramework;
using UnityEngine;

public class ActorTestModel : ManagerContorBase<ActorTestModel>
{
    private ActorTestSceneComp SceneComp;
    public override void Load(params object[] _Agr)
    {
        ResourceComp = AddComp<Model_ResourceComp>();
        SceneComp = AddComp<ActorTestSceneComp>();
        base.Load(_Agr);
    }
}
