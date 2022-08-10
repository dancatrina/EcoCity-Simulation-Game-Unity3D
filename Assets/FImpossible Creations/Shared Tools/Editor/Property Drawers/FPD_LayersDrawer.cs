using UnityEditor;
using UnityEngine;

namespace FIMSpace.FEditor
{
    [CustomPropertyDrawer(typeof(FPD_LayersAttribute))]
    public class FPropDrawers_Layers : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}

