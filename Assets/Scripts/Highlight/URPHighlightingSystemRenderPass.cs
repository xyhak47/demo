using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class URPHighlightingSystemRenderPass : ScriptableRenderPass
{

	private const string RenderTag = "DrawRender";

	// Blur iterations
	private int m_Iterations = 2;

	// Stencil (highlighting) buffer depth
	private int m_StencilZBufferDepth = 0;

	// Blur minimal spread
	private float m_BlurMinSpread = 0.65f;

	// Blur spread per iteration
	private float m_BlurSpread = 0.25f;

	// Stencil (highlighting) buffer size downsample factor
	private int m_DownsampleFactor = 4;

	private Color m_OutLineColor;

    private bool m_IsSwitchNormal;

    private float m_OutLineWidth;

    private static readonly int m_StencilTexPropID = Shader.PropertyToID("_StencilTex");

	private static readonly int m_BlurTexPropID = Shader.PropertyToID("_BlurTex");

	private static List<URPHighlightableObject> m_TargetObjects = new List<URPHighlightableObject>();

	private static Shader m_OutlineShader = Shader.Find("Hidden/URPHighlighted/StencilOpaque");

	private RenderTargetHandle m_Destination;

	private RenderTargetIdentifier m_CurrentTarget;

	private RenderTexture m_StencilBuffer = null;

	private RenderTargetHandle m_TemporaryColorTexture;

	// Blur Shader
	private static Shader _blurShader;

	private static Shader m_BlurShader
	{
		get
		{
			if (_blurShader == null)
			{
				_blurShader = Shader.Find("Hidden/Highlighted/Blur");
			}
			return _blurShader;
		}
	}
	//normal OutLineShader
	private static Shader _outLineShader;
	private static Shader m_OutLineShader
	{
		get
		{
			if (_outLineShader == null)
			{
				_outLineShader = Shader.Find("Hidden/URPHighlighted/Extend");
			}
			return _outLineShader;
		}
	}



	// Compositing Shader
	private static Shader _compShader;
	private static Shader m_CompShader
	{
		get
		{
			if (_compShader == null)
			{
				_compShader = Shader.Find("Hidden/Highlighted/Composite");
			}
			return _compShader;
		}
	}

	//normal OutLineMaterial
	private static Material _outLineMaterial = null;
	private static Material m_outLineMaterial
	{
		get
		{
			if (_outLineMaterial == null)
			{
				_outLineMaterial = new Material(m_OutLineShader);
				_outLineMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return _outLineMaterial;
		}
	}



	// Blur Material
	private static Material _blurMaterial = null;
	private static Material m_BlurMaterial
	{
		get
		{
			if (_blurMaterial == null)
			{
				_blurMaterial = new Material(m_BlurShader);
				_blurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return _blurMaterial;
		}
	}

	// Compositing Material
	private static Material _compMaterial = null;
	private static Material m_CompMaterial
	{
		get
		{
			if (_compMaterial == null)
			{
				_compMaterial = new Material(m_CompShader);
				_compMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return _compMaterial;
		}
	}

	private URPHighlightingSystem m_HighlightingSystem;

	public URPHighlightingSystemRenderPass()
	{

	}

	public static void AddTarget(URPHighlightableObject target)
	{
		if (target.material == null)
			target.material = new Material(m_OutlineShader);
		m_TargetObjects.Add(target);
	}
	public static void RemoveTarget(URPHighlightableObject target)
	{
		bool found = false;

		for (int i = 0; i < m_TargetObjects.Count; i++)
		{
			if (m_TargetObjects[i] == target)
			{
				m_TargetObjects[i].DestoryMaterial();
				m_TargetObjects.Remove(target);
				target.material = null;
				found = true;
				break;
			}
		}
	}

	public void RefreshCommandBuff(CommandBuffer cmd, Camera camera)
	{
		if (m_StencilBuffer)
			RenderTexture.ReleaseTemporary(m_StencilBuffer);
		m_StencilBuffer = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, m_StencilZBufferDepth, RenderTextureFormat.ARGB32);
		cmd.SetRenderTarget(m_StencilBuffer);
		cmd.ClearRenderTarget(true, true, Color.clear);
		for (int i = 0; i < m_TargetObjects.Count; i++)
		{
			Renderer[] renderers = m_TargetObjects[i].GetComponentsInChildren<Renderer>();
			foreach (Renderer r in renderers)
			{
				m_TargetObjects[i].material.SetColor("_Outline", m_OutLineColor);
				cmd.DrawRenderer(r, m_TargetObjects[i].material);
			}
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (m_TargetObjects.Count <= 0) return;
		if (renderingData.cameraData.camera == Camera.main || renderingData.cameraData.camera.tag == "ModelC")
		{
			RenderTargetIdentifier source = m_CurrentTarget;
			CommandBuffer cmd = CommandBufferPool.Get(RenderTag);
			RefreshCommandBuff(cmd, renderingData.cameraData.camera);
			RenderPocessing(cmd, ref renderingData);
			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}
	}

	void RenderPocessing(CommandBuffer cmd, ref RenderingData renderingData)
	{
		RenderTargetIdentifier source = m_CurrentTarget;
		// If stencilBuffer is not created by some reason
		if (m_StencilBuffer == null)
		{
			Debug.Log("stencilBuffer is null");
			// Simply transfer framebuffer to destination
			Blit(cmd, source, m_Destination.Identifier());
			return;
		}
#if UNITY_ANDROID || UNITY_IOS
		RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
		opaqueDesc.colorFormat = RenderTextureFormat.ARGB32;
#else
		RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
#endif

		// Create two buffers for blurring the image
		int width = m_StencilBuffer.width / m_DownsampleFactor;
		int height = m_StencilBuffer.height / m_DownsampleFactor;
		RenderTexture buffer = RenderTexture.GetTemporary(width, height, m_StencilZBufferDepth, RenderTextureFormat.ARGB32);
		RenderTexture buffer2 = RenderTexture.GetTemporary(width, height, m_StencilZBufferDepth, RenderTextureFormat.ARGB32);

		// Copy stencil buffer to the 4x4 smaller texture
		DownSample4x(ref cmd, m_StencilBuffer, buffer);

		// Blur the small texture
		bool oddEven = true;
		if (m_IsSwitchNormal)
		{
			DrawOutLine(ref cmd, buffer);
		}
		else {
			for (int i = 0; i < m_Iterations; i++)
			{
				if (oddEven)
				{
					FourTapCone(ref cmd, buffer, buffer2, i);
				}
				else
				{
					FourTapCone(ref cmd, buffer2, buffer, i);
				}
				oddEven = !oddEven;
			}
		}

		// Compose
		cmd.GetTemporaryRT(m_StencilTexPropID, opaqueDesc);
		cmd.GetTemporaryRT(m_BlurTexPropID, opaqueDesc);
		cmd.Blit(m_StencilBuffer, m_StencilTexPropID);
		if (oddEven)
		{
			cmd.Blit(buffer, m_BlurTexPropID);
		}
		else
		{
			cmd.Blit(buffer2, m_BlurTexPropID);
		}
		if (m_Destination == RenderTargetHandle.CameraTarget)
		{
			cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc, FilterMode.Bilinear);
			Blit(cmd, source, m_TemporaryColorTexture.Identifier(), m_CompMaterial);
			Blit(cmd, m_TemporaryColorTexture.Identifier(), source);
		}
		else
		{
			Blit(cmd, source, m_Destination.Identifier(), m_CompMaterial);
		}

		// Cleanup
		RenderTexture.ReleaseTemporary(buffer);
		RenderTexture.ReleaseTemporary(buffer2);
	}

    private void DrawOutLine(ref CommandBuffer cmd, RenderTexture buffer)
    {
		cmd.SetRenderTarget(buffer);
		cmd.ClearRenderTarget(true, true, Color.clear);
		for (int i = 0; i < m_TargetObjects.Count; i++)
		{
			Renderer[] renderers = m_TargetObjects[i].GetComponentsInChildren<Renderer>();
			foreach (Renderer r in renderers)
			{
				m_outLineMaterial.SetFloat("_Expand", m_OutLineWidth);
				m_outLineMaterial.SetColor("_Outline", m_OutLineColor);
				cmd.DrawRenderer(r, m_outLineMaterial);
			}
		}
	}

    // Performs one blur iteration
    private void FourTapCone(ref CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier dest, int iteration)
	{
		float off = m_BlurMinSpread + iteration * m_BlurSpread;
		m_BlurMaterial.SetFloat("_OffsetScale", off);
		Blit(cmd, source, dest, m_BlurMaterial);
	}

	// Downsamples source texture
	private void DownSample4x(ref CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier dest)
	{
		float off = 1.0f;
		m_BlurMaterial.SetFloat("_OffsetScale", off);
		Blit(cmd, source, dest, m_BlurMaterial);
	}

	public void Setup(in RenderTargetIdentifier currentTarget, RenderTargetHandle dest)
	{
		this.m_Destination = dest;
		this.m_CurrentTarget = currentTarget;
		if (m_HighlightingSystem == null) {
			VolumeStack stack = VolumeManager.instance.stack;
			m_HighlightingSystem = stack.GetComponent<URPHighlightingSystem>();
		}
		this.m_BlurMinSpread = m_HighlightingSystem.BlurSpread.value;
		this.m_BlurSpread = m_HighlightingSystem.BlurSpread.value;
		this.m_DownsampleFactor = m_HighlightingSystem.DownsampleFactor.value <= 0 ? 1 : m_HighlightingSystem.DownsampleFactor.value;
		this.m_Iterations = m_HighlightingSystem.Iterations.value <= 0 ? 1 : m_HighlightingSystem.Iterations.value;
		this.m_OutLineColor = m_HighlightingSystem.ColorFactor.value;
		this.m_IsSwitchNormal = m_HighlightingSystem.IsSwitchNormalState.value;
		this.m_OutLineWidth = m_HighlightingSystem.OutLineWidth.value;
	}
}
