using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Clickable_SwitchView : Basic_Clickable
{
    [SerializeField] private CinemachineCamera camView;
    [SerializeField] private MouseLookData mouseLookData;
    public override void OnClick(PlayerControl player)
    {
        player.GoToObserveView(camView, mouseLookData);
        DisableHitbox();
    }
}