using System;

namespace UnityEngine.Rendering.Universal
{
    [System.Serializable, VolumeComponentMenu("URPDrawRenderSystem/DrawRender")]
    public sealed class URPDrawRenderSystem : VolumeComponent, IPostProcessComponent
    {

        public bool IsActive() {
            return true;
        }

        public bool IsTileCompatible() {
            return false;
        }
    }
}