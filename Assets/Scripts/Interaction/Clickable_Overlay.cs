using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_Overlay : Basic_Clickable
{
    [SerializeField] private MeshRenderer overlayRenderer;
    [SerializeField] private GameObject overlayItem;
    public override void OnClick(PlayerControl player)
    {
        DisableHitbox();
        overlayRenderer.enabled = false;
        player.GoToInspectItem(overlayItem);
    }
}
