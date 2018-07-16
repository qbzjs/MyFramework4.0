using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Tools
{
    public class AssetBundleInfo : EditorFileinfo
    {
        public string AssetBundleName;
        public bool IsMergerAssetBundle = true;
        public bool IsSelectAssetBundle = true;

        public AssetCheckMode CheckMode = AssetCheckMode.AppStartCheck;

        public AssetBundleInfo(string _Path, AssetBundleInfo _Parent, bool _IsRootNode = false)
            :base(_Path, _Parent, _IsRootNode)
        {
            AssetBundleName = "";
        }


        /// <summary>
        /// 设置文件树打包信息
        /// </summary>
        /// <param name="_AssetBundleName"></param>
        public void SetAssetBundleName(string _AssetBundleName, AssetBundleInfo rootinfo, ref Dictionary<string, ResBuileInfo> Builder)
        {
            if (FileType != FileType.Folder)
            {
                if (((AssetBundleInfo)Parent).IsMergerAssetBundle)
                {
                    IsSelectAssetBundle = false;
                    AssetBundleName = _AssetBundleName;
                    string AssetPath = Path;
                    if (!Builder.ContainsKey(AssetBundleName.ToLower()))
                    {
                        Builder[AssetBundleName.ToLower()] = new ResBuileInfo();
                        Builder[AssetBundleName.ToLower()].Id = AssetBundleName.ToLower();
                        Builder[AssetBundleName.ToLower()].CheckModel = CheckMode;
                        Builder[AssetBundleName.ToLower()].Assets = new List<string>();
                    }
                    Builder[AssetBundleName.ToLower()].Assets.Add(AssetPath.Substring(AppPathConfig.PlatformRoot.Length + 1));
                }
                else
                {
                    AssetBundleName = Path.Substring(rootinfo.Path.Length + 1);
                    AssetBundleName = AssetBundleName.Substring(0, AssetBundleName.IndexOf(".")) + AppConfig.ResFileSuffix;
                    if (!Builder.ContainsKey(AssetBundleName.ToLower()))
                    {
                        Builder[AssetBundleName.ToLower()] = new ResBuileInfo();
                        Builder[AssetBundleName.ToLower()].Id = AssetBundleName.ToLower();
                        Builder[AssetBundleName.ToLower()].CheckModel = CheckMode;
                        Builder[AssetBundleName.ToLower()].Assets = new List<string>();
                        Builder[AssetBundleName.ToLower()].Assets.Add(Path.Substring(AppPathConfig.PlatformRoot.Length + 1));
                    }
                }
            }
            else
            {
                if (((AssetBundleInfo)Parent).IsMergerAssetBundle)
                {
                    AssetBundleName = _AssetBundleName;
                    IsSelectAssetBundle = false;
                }
                else
                {
                    AssetBundleName =  Path.Substring(rootinfo.Path.Length + 1) + AppConfig.ResFileSuffix;
                    IsSelectAssetBundle = true;
                    if (IsMergerAssetBundle)
                    {
                        if (!Builder.ContainsKey(AssetBundleName.ToLower()))
                        {
                            Builder[AssetBundleName.ToLower()] = new ResBuileInfo();
                            Builder[AssetBundleName.ToLower()].Id = AssetBundleName.ToLower();
                            Builder[AssetBundleName.ToLower()].CheckModel = CheckMode;
                            Builder[AssetBundleName.ToLower()].Assets = new List<string>();
                        }
                    }
                }
                for (int i = 0; i < Childs.Count; i++)
                {
                    ((AssetBundleInfo)Childs[i]).SetAssetBundleName(AssetBundleName, rootinfo, ref Builder);
                }
            }
        }


        public void SetCheckMode(AssetCheckMode _CheckMode)
        {
            CheckMode = _CheckMode;
            if (FileType == FileType.Folder && !IsMergerAssetBundle)
            {
                for (int i = 0; i < Childs.Count; i++)
                {
                    ((AssetBundleInfo)Childs[i]).SetCheckMode(_CheckMode);
                }
            }
        }
    }
}
