using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_TVChannel : Basic_Clickable
{
    [SerializeField] private TVController tvController;
    public override void OnClick(PlayerControl player)
    {
        base.OnClick(player);
        tvController.ChangeChannle();
    }
}
