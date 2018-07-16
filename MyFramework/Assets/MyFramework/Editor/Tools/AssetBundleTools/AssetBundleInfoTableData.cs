using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyFramework
{
    [Serializable]
    public class AssetBundleInfoData
    {
        public bool IsRootNode = false;
        public string Path;
        public bool IsMergerAssetBundle = true;
        public bool IsSelectAssetBundle = true;
        public AssetCheckMode CheckMode;
        public List<AssetBundleInfoData> Chiles;
    }

    public class AssetBundleInfoTableData : ScriptableObject
    {
        public AppPlatform BuildPlatform;
        public AssetBundleInfoData Config;
    }
}
