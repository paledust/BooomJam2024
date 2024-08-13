using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Retro_RenderPass : ScriptableRenderPass
{
    private Material retroMat;
    private RTHandle retroTexHandle;
    private RenderTextureDescriptor retroTexDescriptor;

    private HashSet<Renderer> retroRenderers;

    public Retro_RenderPass(Material mat){
        retroMat = mat;
        retroTexDescriptor = new RenderTextureDescriptor(Screen.width,
            Screen.height, RenderTextureFormat.DefaultHDR, 0);
    }
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        retroTexDescriptor.height = cameraTextureDescriptor.height;
        retroTexDescriptor.width = cameraTextureDescriptor.width;

        cmd.SetGlobalTexture("Retro_CMD",retroTexHandle);

        RenderingUtils.ReAllocateIfNeeded(ref retroTexHandle, retroTexDescriptor);
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        retroRenderers = Retro_RenderFeature.retroRenderers;
        if(retroRenderers==null || retroRenderers.Count == 0) return;

        CommandBuffer cmd = CommandBufferPool.Get("Retro screen");

        cmd.SetRenderTarget(retroTexHandle);
        cmd.ClearRenderTarget(true, true, Color.clear);

        foreach(var retro in retroRenderers){
            cmd.DrawRenderer(retro, retroMat, 1);
        }
        cmd.SetGlobalTexture("Retro_CMD",retroTexHandle);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
    public void Dispose(){
        if(retroTexHandle!=null) retroTexHandle.Release();
    }
}
