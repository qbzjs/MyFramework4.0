using MyFramework;
using UnityEngine;

public class AppMain : PrefabSingleton<AppMain>
{
    [SerializeField, MFWAttributeRename("App资源加载模式")]
    private AppResModel AppResModel;
    [SerializeField, MFWAttributeRename("App语言")]
    private Language AppLanguage;
    [SerializeField, MFWAttributeRename("发布目标平台")]
    private AppPlatform TargetPlatform;
    [SerializeField, MFWAttributeRename("Web服务器")]
    private WebRootAddress WebRoot;

    protected override void Init()
    {
        AppConfig.SetAppConfig(AppResModel, AppLanguage, TargetPlatform, WebRoot);
        //基础模块
        //Manager_ManagerModel.Instance.StartModel<TimerModel>();
        //Manager_ManagerModel.Instance.StartModel<CoroutineModel>();
        //Manager_ManagerModel.Instance.StartModel<LuaManagerModel>();
        //Manager_ManagerModel.Instance.StartModelForName("MyFramework", "LuaManagerModel");
        //Manager_ManagerModel.Instance.StartModel<WebServiceModel>();
        //Manager_ManagerModel.Instance.StartModel<DownloadModel>();
        //Manager_ManagerModel.Instance.StartModelForName("MyFramework", "LuaServiceModel", null,"127.0.0.1",1204);

        Manager_ManagerModel.Instance.StartModel<CoroutineModel>();
        Manager_ManagerModel.Instance.StartModel<ViewManagerModel>(null,new Vector2(1920,1080),1.0f);
        Manager_ManagerModel.Instance.StartModel<SceneModel>();
        Manager_ManagerModel.Instance.StartModel<ActorTestModel>();
    }
}
