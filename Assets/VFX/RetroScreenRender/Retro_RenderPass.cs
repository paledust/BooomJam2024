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
    public Retro_RenderPass(Material mat){
        retroMat = mat;
        retroTexDescriptor = new RenderTextureDescriptor(Screen.width,
            Screen.height, RenderTextureFormat.Default, 0);
    }
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        retroTexDescriptor.height = cameraTextureDescriptor.height;
        retroTexDescriptor.width = cameraTextureDescriptor.width;

        RenderingUtils.ReAllocateIfNeeded(ref retroTexHandle, retroTexDescriptor);
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Retro screen");

        RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
    public void Dispose(){

    }
}
