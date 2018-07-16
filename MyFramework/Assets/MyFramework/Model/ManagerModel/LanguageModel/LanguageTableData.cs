using UnityEngine;

namespace MyFramework
{ 
    [SerializeField]
    public class LanguageItemData: ConfigDataBase<string>
    {
        public string Value;
    }

    public class LanguageTableData : ConfigTableDataBase<string, LanguageItemData>
    {

    }
}
