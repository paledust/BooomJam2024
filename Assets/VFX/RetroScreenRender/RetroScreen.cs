using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetroScreen : MonoBehaviour
{
    [SerializeField] private MeshRenderer retroRenderer;
    void OnEnable(){
        Retro_RenderFeature.RegisterRetroRenderer(retroRenderer);
    }
    void OnDisable(){
        Retro_RenderFeature.RegisterRetroRenderer(retroRenderer);        
    }
}
