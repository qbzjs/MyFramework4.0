using System.Collections;
using MyFramework;

public class ActorTestSceneComp : ModelCompBase<ActorTestModel>, ISceneLoadCompBase
{
    public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
    {
        base.Load(_ModelContorl);
        //MyCentorl.LoadBundle("Scene/MainCityScene");
        SceneModel.Instance.ChangeScene(this);
    }

    /// <summary>
    /// 加载进度
    /// </summary>
    private float Process;
    /// <summary>
    ///获取场景切换进入
    /// </summary>
    /// <returns></returns>
    public virtual float GetProcess()
    {
        return Process;
    }
    /// <summary>
    /// 获取场景名称
    /// </summary>
    /// <returns></returns>
    public virtual string GetSceneName()
    {
        return "ActorTestScene";
    }
    /// <summary>
    /// 场景加载
    /// </summary>
    public virtual IEnumerator LoadScene()
    {
        yield return 1;
        Process = 1;
        base.LoadEnd();
    }
    /// <summary>
    /// 场景卸载
    /// </summary>
    public virtual IEnumerator UnloadScene()
    {
        yield return 1;
        Process = 1;
    }
}

