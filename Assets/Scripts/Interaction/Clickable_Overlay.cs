using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_Overlay : Basic_Clickable, IInspectable
{
    [SerializeField] private OverlayController overlayController;
    [SerializeField] private MeshRenderer overlayRenderer;
    [SerializeField] private GameObject inspectItem;

    public bool m_canInteract{get{return !isSticking;}}

    private bool isSticking = false;

    public override void OnClick(PlayerControl player)
    {
        DisableHitbox();
        overlayRenderer.enabled = false;
        player.GoToInspectItem(this, inspectItem);
    }
    public void OnConfirm(){
        isSticking = true;
        overlayController.StickOverlayOnTV(this, inspectItem);
    }
    public void OnCancel(){
        EnableHitbox();
        overlayRenderer.enabled = true;
    }
    public void ResetOverlay(){
        isSticking = false;
        overlayRenderer.enabled = true;
    }
}
