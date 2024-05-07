using UnityEngine.Rendering.Universal;

public class URPHighlightingSystemFeature : ScriptableRendererFeature
{
    private URPHighlightingSystemRenderPass m_HighlightingSystemPass;


    public override void Create()
    {
        m_HighlightingSystemPass = new URPHighlightingSystemRenderPass();

        m_HighlightingSystemPass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        RenderTargetHandle dest = RenderTargetHandle.CameraTarget;
        m_HighlightingSystemPass.Setup(renderer.cameraColorTarget, dest);
        renderer.EnqueuePass(m_HighlightingSystemPass);
    }
    
}