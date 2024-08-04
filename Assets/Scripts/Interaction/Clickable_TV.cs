using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_TV : Basic_Clickable
{
    public override void OnHover(PlayerControl player)
    {
        Debug.Log("Hovered");
    }
    public override void OnClick(PlayerControl player)
    {
        Debug.Log("Hello There");
    }
}
