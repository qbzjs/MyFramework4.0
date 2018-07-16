using UnityEngine;

namespace MyFramework
{
    public class MFWAttributeRename : PropertyAttribute
    {
        public string PropertyName;
        public MFWAttributeRename(string name)
        {
            PropertyName = name;
        }
    }

    public class MFWAttribute16Int : PropertyAttribute
    {
        public MFWAttribute16Int()
        {
        }
    }
}
