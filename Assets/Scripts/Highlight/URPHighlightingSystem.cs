using System;

namespace UnityEngine.Rendering.Universal
{
    [System.Serializable, VolumeComponentMenu("HighlightingSystem/OutLineEffect")]
    public sealed class URPHighlightingSystem : VolumeComponent, IPostProcessComponent
    {
        [Tooltip("Blur最小扩散值")]
        public FloatParameter BlurMinSpread = new FloatParameter(0.65f);

        [Tooltip("Blur扩散值")]
        public FloatParameter BlurSpread = new FloatParameter(0.25f);

        [Tooltip("Blur采样次数")]
        public IntParameter Iterations = new IntParameter(2);

        [Tooltip("采样压缩")]
        public IntParameter DownsampleFactor = new IntParameter(4);

        [Tooltip("后处理颜色")]
        public ColorParameter ColorFactor = new ColorParameter(Color.green);

        [Tooltip("是否开启普通描边模式")]
        public BoolParameter IsSwitchNormalState = new BoolParameter(false);

        [Tooltip("描边边框宽度(开启普通描边)")]
        public FloatParameter OutLineWidth = new FloatParameter(1.1f);

        public bool IsActive() {
            return true;
        }

        public bool IsTileCompatible() {
            return false;
        }
    }
}