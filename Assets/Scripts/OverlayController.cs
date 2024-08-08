using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
    [SerializeField] private TVController tvController;
    [SerializeField] private Clickable_Overlay[] overlays;
    [SerializeField] private Clickable_OverlayView overlayView;

    private Clickable_Overlay busyOverlay;

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
            if(overlay.m_canInteract)overlay.EnableHitbox();
        }
    }
    public void StickOverlayOnTV(Clickable_Overlay overlay, GameObject stickItem){
        if(busyOverlay!=null){
            busyOverlay.ResetOverlay();
        }
        busyOverlay = overlay;
        tvController.StickOverlay(stickItem);
    }
}
