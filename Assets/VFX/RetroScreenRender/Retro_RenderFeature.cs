using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Retro_RenderFeature : ScriptableRendererFeature
{
    [SerializeField] private Material Mat_retroScreen;
    private Retro_RenderPass retroPass;

    public static HashSet<Renderer> retroRenderers{get; private set;} = new HashSet<Renderer>();

    public override void Create()
    {
        retroPass = new Retro_RenderPass(Mat_retroScreen){renderPassEvent = RenderPassEvent.BeforeRenderingTransparents};
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(renderingData.cameraData.cameraType == CameraType.Game){
            renderer.EnqueuePass(retroPass);
        }
    }
    protected override void Dispose(bool disposing)
    {
        retroPass.Dispose();
    }
    public static void RegisterRetroRenderer(Renderer _renderer){
        retroRenderers.Add(_renderer);
    }
    public static void UnregisterRetroRenderer(Renderer _renderer){
        retroRenderers.Remove(_renderer);
    }
}
