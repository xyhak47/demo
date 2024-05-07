using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class URPDrawRenderPass : ScriptableRenderPass
{

	public const string RenderTag = "DrawRender";

	// Blur iterations
	public int iterations = 2;//2;

	// Stencil (highlighting) buffer depth
	public int stencilZBufferDepth = 0;

	// Blur minimal spread
	public float blurMinSpread = 0.65f;//0.65f;

	// Blur spread per iteration
	public float blurSpread = 0.25f;//0.25f;

	// Stencil (highlighting) buffer size downsample factor
	public int _downsampleFactor = 2;//4;

	private static readonly int stencilTexPropID = Shader.PropertyToID("_StencilTex");

	private static readonly int blurTexPropID = Shader.PropertyToID("_BlurTex");

	private static List<URPDrawRenderObject> targetObjects = new List<URPDrawRenderObject>();

	private static Shader preoutlineShader = Shader.Find("Hidden/URPHighlighted/StencilOpaque");

	private RenderTargetHandle destination;

	private RenderTargetIdentifier currentTarget;

	private FilterMode filterMode { get; set; }

	// RenderTexture with stencil buffer
	private RenderTexture stencilBuffer = null;

	private RenderTargetHandle m_temporaryColorTexture;

	// Blur Shader
	private static Shader _blurShader;

	private static Shader blurShader
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

	// Compositing Shader
	private static Shader _compShader;
	private static Shader compShader
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


	// Blur Material
	private static Material _blurMaterial = null;
	private static Material blurMaterial
	{
		get
		{
			if (_blurMaterial == null)
			{
				_blurMaterial = new Material(blurShader);
				_blurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return _blurMaterial;
		}
	}

	// Compositing Material
	private static Material _compMaterial = null;
	private static Material compMaterial
	{
		get
		{
			if (_compMaterial == null)
			{
				_compMaterial = new Material(compShader);
				_compMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return _compMaterial;
		}
	}

	public URPDrawRenderPass()
	{

	}

	public static void AddTarget(URPDrawRenderObject target)
	{
		if (target.material == null)
			target.material = new Material(preoutlineShader);
		targetObjects.Add(target);
		//RefreshCommandBuff();
	}
	public static void RemoveTarget(URPDrawRenderObject target)
	{
		bool found = false;

		for (int i = 0; i < targetObjects.Count; i++)
		{
			if (targetObjects[i] == target)
			{
				targetObjects.Remove(target);
				//DestroyImmediate(target.material);
				target.material = null;
				found = true;
				break;
			}
		}
	}

	public void RefreshCommandBuff(CommandBuffer cmd, Camera camera)
	{
		if (stencilBuffer)
			RenderTexture.ReleaseTemporary(stencilBuffer);
		stencilBuffer = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, stencilZBufferDepth,RenderTextureFormat.ARGB32);
		//CoreUtils.SetRenderTarget(cmd, stencilBuffer, ClearFlag.All, Color.clear);
		cmd.SetRenderTarget(stencilBuffer);
		cmd.ClearRenderTarget(true, true, Color.clear);
		for (int i = 0; i < targetObjects.Count; i++)
		{
			Renderer[] renderers = targetObjects[i].GetComponentsInChildren<Renderer>();
			foreach (Renderer r in renderers)
			{
				targetObjects[i].material.SetColor("_Outline", targetObjects[i].OutLineColor);
				cmd.DrawRenderer(r, targetObjects[i].material);
			}
		}
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		RenderTargetIdentifier source = currentTarget;
		CommandBuffer cmd = CommandBufferPool.Get(RenderTag);
		RefreshCommandBuff( cmd, renderingData.cameraData.camera);
		RenderPocessing(cmd,ref renderingData);
		//Blit(cmd, stencilBuffer, destination.Identifier());
		context.ExecuteCommandBuffer(cmd);
		CommandBufferPool.Release(cmd);
	}

	void RenderPocessing(CommandBuffer cmd, ref RenderingData renderingData)
	{

		RenderTargetIdentifier source = currentTarget;

		// If stencilBuffer is not created by some reason
		if (stencilBuffer == null)
		{
			Debug.Log("stencilBuffer is null");
			// Simply transfer framebuffer to destination
			Blit(cmd, source, destination.Identifier());
			return;
		}

		RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;

		// Create two buffers for blurring the image
		int width = stencilBuffer.width / _downsampleFactor;
		int height = stencilBuffer.height / _downsampleFactor;
		RenderTexture buffer = RenderTexture.GetTemporary(width, height, stencilZBufferDepth, RenderTextureFormat.ARGB32);
		RenderTexture buffer2 = RenderTexture.GetTemporary(width, height, stencilZBufferDepth, RenderTextureFormat.ARGB32);

		// Copy stencil buffer to the 4x4 smaller texture
		DownSample4x(ref cmd, stencilBuffer, buffer);

		// Blur the small texture
		bool oddEven = true;
		for (int i = 0; i < iterations; i++)
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

		// Compose
		cmd.GetTemporaryRT(stencilTexPropID, opaqueDesc);
		cmd.GetTemporaryRT(blurTexPropID, opaqueDesc);
		cmd.Blit(stencilBuffer, stencilTexPropID);
		if (oddEven)
		{
			cmd.Blit(buffer, blurTexPropID);
		}
		else
		{
			cmd.Blit(buffer2, blurTexPropID);
		}
        //Blit(cmd, source, destination.Identifier(), compMaterial);
        if (destination == RenderTargetHandle.CameraTarget)
        {
            cmd.GetTemporaryRT(m_temporaryColorTexture.id, opaqueDesc, filterMode);
            Blit(cmd, source, m_temporaryColorTexture.Identifier(), compMaterial);
            Blit(cmd, m_temporaryColorTexture.Identifier(), source);
        }
        else
        {
            Blit(cmd, source, destination.Identifier(), compMaterial);
        }

        // Cleanup
        RenderTexture.ReleaseTemporary(buffer);
		RenderTexture.ReleaseTemporary(buffer2);
	}

	// Performs one blur iteration
	private void FourTapCone(ref CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier dest, int iteration)
	{
		float off = blurMinSpread + iteration * blurSpread;
		blurMaterial.SetFloat("_OffsetScale", off);
		Blit(cmd, source, dest, blurMaterial);
	}

	// Downsamples source texture
	private void DownSample4x(ref CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier dest)
	{
		float off = 1.0f;
		blurMaterial.SetFloat("_OffsetScale", off);
		Blit(cmd, source, dest, blurMaterial);
	}

	public void Setup(in RenderTargetIdentifier currentTarget, RenderTargetHandle dest)
	{
		this.destination = dest;
		this.currentTarget = currentTarget;
	}
}
