using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_OverlayView : Clickable_SwitchView
{
    [SerializeField] private OverlayController overlayController;
    public override void OnClick(PlayerControl player)
    {
        base.OnClick(player);
        overlayController.OnOverlayView();
    }
}
