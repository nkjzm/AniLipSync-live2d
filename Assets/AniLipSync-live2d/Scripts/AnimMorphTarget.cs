using Live2D.Cubism.Core;
using UnityEngine;

namespace AniLipSync.Live2D
{
    public class AnimMorphTarget : MonoBehaviour
    {
        [Tooltip("カーブの値を適用する際の倍率")]
        public float curveAmplifier = 100.0f;

        [Range(0.0f, 100.0f), Tooltip("この閾値未満の音素の重みは無視する")]
        public float weightThreashold = 2.0f;

        [Tooltip("Shapeの重みを変化させるフレームレート")]
        public float frameRate = 12.0f;

        [Tooltip("値を変化させるCubismModel")]
        public CubismModel model;

        [Tooltip("aa, E, ih, oh, ouの順で割り当てるVisemeShapeClip")]
        public VisemeShapeClip[] visemeToShape = new VisemeShapeClip[5];

        [Tooltip("OVRLipSyncに渡すSmoothing amountの値")]
        public int smoothAmount = 100;

        OVRLipSyncContextBase context;
        OVRLipSync.Viseme previousViseme = OVRLipSync.Viseme.sil;
        float transitionTimer = 0.0f;
        float frameRateTimer = 0.0f;

        void Start()
        {
            if (model == null)
            {
                Debug.LogError("CubismModelが指定されていません。", this);
            }

            context = GetComponent<OVRLipSyncContextBase>();
            if (context == null)
            {
                Debug.LogError("同じGameObjectにOVRLipSyncContextBaseを継承したクラスが見つかりません。", this);
            }

            context.Smoothing = smoothAmount;
        }

        void LateUpdate()
        {
            if (context == null || model == null)
            {
                return;
            }

            var frame = context.GetCurrentPhonemeFrame();
            if (frame == null)
            {
                return;
            }

            transitionTimer += Time.deltaTime;

            // 設定したフレームレートへUpdate関数を低下させる
            frameRateTimer += Time.deltaTime;
            if (frameRateTimer < 1.0f / frameRate)
            {
                return;
            }
            frameRateTimer -= 1.0f / frameRate;

            // すでに設定されている重みをリセット
            foreach (var shape in visemeToShape)
            {
                shape.Reset(model);
            }

            // 最大の重みを持つ音素を探す
            var maxVisemeIndex = 0;
            var maxVisemeWeight = 0.0f;
            // 子音は無視する
            for (var i = (int)OVRLipSync.Viseme.aa; i < frame.Visemes.Length; i++)
            {
                if (frame.Visemes[i] > maxVisemeWeight)
                {
                    maxVisemeWeight = frame.Visemes[i];
                    maxVisemeIndex = i;
                }
            }

            // 音素の重みが小さすぎる場合は口を閉じる
            if (maxVisemeWeight * 100.0f < weightThreashold)
            {
                transitionTimer = 0.0f;
                return;
            }

            // 音素の切り替わりでタイマーをリセットする
            if (previousViseme != (OVRLipSync.Viseme)maxVisemeIndex)
            {
                transitionTimer = 0.0f;
                previousViseme = (OVRLipSync.Viseme)maxVisemeIndex;
            }

            var visemeIndex = maxVisemeIndex - (int)OVRLipSync.Viseme.aa;
            var targetShape = visemeToShape[visemeIndex];
            targetShape.Apply(model, targetShape.TransitionCurve.Evaluate(transitionTimer) * curveAmplifier);
        }
    }
}