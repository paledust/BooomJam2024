using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Clickable_SwitchView : Basic_Clickable
{
    [SerializeField] private CinemachineCamera camView;
    public override void OnClick(PlayerControl player)
    {
        player.m_mouseLook.RecalculateRotationFromTransform(camView.transform);
        player.transform.position = camView.transform.position;
    }
}
