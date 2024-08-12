using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerRendererRetro : PerRendererBehavior
{
    public float imageBrightness;
    public float scrollingStaticStrength;
    public float staticStrength;
    public Texture screenTex;

    private readonly int ImageBrightnessID = Shader.PropertyToID("_ImageBrightness");
    private readonly int ScrollingStaticStrengthID = Shader.PropertyToID("_ScrollingStaticStrength");
    private readonly int StaticStrengthID = Shader.PropertyToID("_StaticStrength");
    private readonly int ScreenTexID = Shader.PropertyToID("_ScreenTexture");

    public void Refresh(){
        UpdateProperties();
    }
    protected override void UpdateProperties()
    {
        mpb.SetFloat(ImageBrightnessID, imageBrightness);
        mpb.SetFloat(ScrollingStaticStrengthID, scrollingStaticStrength);
        mpb.SetFloat(StaticStrengthID, staticStrength);
        mpb.SetTexture(ScreenTexID, screenTex!=null?screenTex:Texture2D.blackTexture);
    }
}
