using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_TVView : Clickable_SwitchView
{
    [SerializeField] private TVController tVController;
    public override void OnClick(PlayerControl player)
    {
        base.OnClick(player);
        tVController.EnableTVInteraction();
    }
}
