using Live2D.Cubism.Core;
using System;
using UnityEngine;

namespace AniLipSync.Live2D
{
    [Serializable]
    public struct BlendShapeBinding
    {
        public int Index;
        public float Weight;
    }

    [CreateAssetMenu(menuName = "AniLipSync/VisemeClip")]
    public class VisemeShapeClip : ScriptableObject
    {
        [SerializeField]
        GameObject m_prefab;
        public GameObject Prefab
        {
            set { m_prefab = value; }
            get { return m_prefab; }
        }

        [SerializeField]
        public OVRLipSync.Viseme Preset = OVRLipSync.Viseme.aa;

        [SerializeField]
        public AnimationCurve TransitionCurve = null;

        [SerializeField]
        public BlendShapeBinding[] Values = new BlendShapeBinding[] { };

        public void Apply(CubismModel root, float value)
        {
            foreach (var x in Values)
            {
                var parameter = root.Parameters[x.Index];
                if (parameter != null)
                {
                    var target = Mathf.Lerp(parameter.DefaultValue, x.Weight, value);
                    parameter.Value = Mathf.Clamp(target, parameter.MinimumValue, parameter.MaximumValue);
                }
            }
        }

        public void Reset(CubismModel root)
        {
            foreach (var x in Values)
            {
                var parameter = root.Parameters[x.Index];
                if (parameter != null)
                {
                    parameter.Value = parameter.DefaultValue;
                }
            }
        }
    }
}