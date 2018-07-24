using UnityEditor;
using UnityEngine;
using System.IO;
using MyFramework.Tools;

namespace MyFramework
{
    public class ScriptDataCreateTools
    {
        [MenuItem("Assets/MyFrameworkTools/ScriptDataCreateTools", false, 81)]
        public static void LuaToTxt()
        {
            string TargetPath = EditorHelper.GetSelectedPathOrFallback();
            Debug.Log(TargetPath);
            AssetDatabase.CreateAsset(new MyFramework.SkillSystem.OrdinaryAairBombTableData(), TargetPath+ "/OrdinaryAairBombTableData.asset");
        }
    }
}
