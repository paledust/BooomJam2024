using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_Overlay : Basic_Clickable
{
    [SerializeField] private GameObject overlayItem;
    public override void OnClick(PlayerControl player)
    {
           player.GoToInspectItem(overlayItem);
    }
}
