using UnityEngine.Rendering.Universal;

public class URPDrawRenderFeature : ScriptableRendererFeature
{
    private URPDrawRenderPass m_DrawRenderPass;
    //private URPHighlightingSystemRenderPass m_HighlightingSystemPass;

    public override void Create()
    {

        m_DrawRenderPass = new URPDrawRenderPass();

        m_DrawRenderPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        //m_HighlightingSystemPass = new URPHighlightingSystemRenderPass();

        //m_HighlightingSystemPass.renderPassEvent = RenderPassEvent.AfterRendering;
    }

    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        RenderTargetHandle dest = RenderTargetHandle.CameraTarget;
        m_DrawRenderPass.Setup(renderer.cameraColorTarget, dest);
        //m_HighlightingSystemPass.Setup(renderer.cameraColorTarget, dest, m_DrawRenderPass.stencilBuffer);
        renderer.EnqueuePass(m_DrawRenderPass);
        //renderer.EnqueuePass(m_HighlightingSystemPass);
    }
    
}