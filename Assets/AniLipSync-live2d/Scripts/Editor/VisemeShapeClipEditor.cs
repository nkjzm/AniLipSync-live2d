using Live2D.Cubism.Core;
using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AniLipSync.Live2D
{
    [CustomEditor(typeof(VisemeShapeClip))]
    public class VisemeShapeClipEditor : Editor
    {
        SerializedProperty m_PresetProp;
        SerializedProperty m_ValuesProp;
        SerializedProperty m_TransitionCurveProp;
        ReorderableList m_ValuesList;

        VisemeShapeClip m_target;

        protected GameObject Prefab
        {
            get { return m_target.Prefab; }
            private set { m_target.Prefab = value; }
        }

        protected void OnEnable()
        {
            m_target = (VisemeShapeClip)target;

            m_PresetProp = serializedObject.FindProperty("Preset");
            m_ValuesProp = serializedObject.FindProperty("Values");
            m_TransitionCurveProp = serializedObject.FindProperty("TransitionCurve");

            m_ValuesList = new ReorderableList(serializedObject, m_ValuesProp);
            m_ValuesList.elementHeight = BlendShapeBindingHeight;
            m_ValuesList.drawElementCallback =
              (rect, index, isActive, isFocused) =>
              {
                  var element = m_ValuesProp.GetArrayElementAtIndex(index);
                  rect.height -= 4;
                  rect.y += 2;
                  DrawBlendShapeBinding(rect, element, m_target.Prefab);
              };
        }

        public override void OnInspectorGUI()
        {
            Prefab = (GameObject)EditorGUILayout.ObjectField("prefab", Prefab, typeof(GameObject), false);

            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_PresetProp, true);
            EditorGUILayout.PropertyField(m_TransitionCurveProp, true);

            EditorGUILayout.LabelField("ShapeBindings", EditorStyles.boldLabel);
            m_ValuesList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        public override string GetInfoString()
        {
            return ((VisemeShapeClip)target).Preset.ToString();
        }

        public static int BlendShapeBindingHeight = 40;
        public static void DrawBlendShapeBinding(Rect position, SerializedProperty property, GameObject prefab)
        {
            if (prefab == null)
            {
                return;
            }

            var model = prefab.GetComponent<CubismModel>();
            var height = 16;

            var y = position.y;
            var rect = new Rect(position.x, y, position.width, height);
            int blendShapeIndex;
            IntPopup(rect, property.FindPropertyRelative("Index"), model.Parameters.Select(p => p.Id).ToArray(), out blendShapeIndex);

            var minVal = 0f;
            var maxVal = 1f;
            if (blendShapeIndex >= 0 && blendShapeIndex < model.Parameters.Length)
            {
                minVal = model.Parameters[blendShapeIndex].MinimumValue;
                maxVal = model.Parameters[blendShapeIndex].MaximumValue;
            }

            y += height;
            rect = new Rect(position.x, y, position.width, height);
            FloatSlider(rect, property.FindPropertyRelative("Weight"), minVal, maxVal);
        }

        static bool StringPopup(Rect rect, SerializedProperty prop, string[] options, out int newIndex)
        {
            if (options == null)
            {
                newIndex = -1;
                return false;
            }

            var oldIndex = Array.IndexOf(options, prop.stringValue);
            newIndex = EditorGUI.Popup(rect, oldIndex, options);
            if (newIndex != oldIndex && newIndex >= 0 && newIndex < options.Length)
            {
                prop.stringValue = options[newIndex];
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool IntPopup(Rect rect, SerializedProperty prop, string[] options, out int newIndex)
        {
            if (options == null)
            {
                newIndex = -1;
                return false;
            }

            var oldIndex = prop.intValue;
            newIndex = EditorGUI.Popup(rect, oldIndex, options);
            if (newIndex != oldIndex && newIndex >= 0 && newIndex < options.Length)
            {
                prop.intValue = newIndex;
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool FloatSlider(Rect rect, SerializedProperty prop, float minValue, float maxValue)
        {
            var oldValue = prop.floatValue;
            var newValue = EditorGUI.Slider(rect, prop.floatValue, minValue, maxValue);
            if (newValue != oldValue)
            {
                prop.floatValue = newValue;
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool ColorProp(Rect rect, SerializedProperty prop)
        {
            var oldValue = (Color)prop.vector4Value;
            var newValue = EditorGUI.ColorField(rect, prop.displayName, oldValue);
            if (newValue != oldValue)
            {
                prop.vector4Value = newValue;
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool OffsetProp(Rect rect, SerializedProperty prop)
        {
            var oldValue = prop.vector4Value;
            var newValue = EditorGUI.Vector4Field(rect, prop.displayName, oldValue);
            if (newValue != oldValue)
            {
                prop.vector4Value = newValue;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
