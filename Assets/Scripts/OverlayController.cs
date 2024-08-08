using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
    [SerializeField] private Clickable_Overlay[] overlays;
    [SerializeField] private Clickable_OverlayView overlayView;
    void OnEnable(){
        EventHandler.E_OnPlayerOverview += handlePlayerOverview;
    }
    void OnDisable(){
        EventHandler.E_OnPlayerOverview -= handlePlayerOverview;
    }
    void handlePlayerOverview(){
        overlayView.EnableHitbox();
        foreach(var overlay in overlays){
            overlay.DisableHitbox();
        }
    }
    public void OnOverlayView(){
        foreach(var overlay in overlays){
            overlay.EnableHitbox();
        }
    }
}
