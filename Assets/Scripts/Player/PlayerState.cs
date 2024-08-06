using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : State<PlayerControl>
{
    public override State<PlayerControl> UpdateState(PlayerControl context)
    {
        context.m_mouseLook.UpdateLookRotation();
        context.RaycastDetectInteractable();
        return null;
    }
    public virtual void HandleLook(InputValue value, PlayerControl context){
        Vector2 input = value.Get<Vector2>();
        context.m_mouseLook.LookRotation(input);
    }
    public virtual void HandleClick(InputValue value, PlayerControl context){
    //Pressing Behavior
        if(value.isPressed){
            context.eyeControl.BlinkEye();
            if(context.m_holdingInteractable != null) return;
            if(context.m_hoveringInteractable == null) return;
        //Interact with object
            // m_playerCommandManager.AbortCommands();
            if(context.m_hoveringInteractable.IsAvailable){
                EventHandler.Call_OnPlayerInteract();
                context.m_hoveringInteractable.OnClick(context);
            //Play Click SFX
                // m_playerAudio.PlayFeedback(m_hoveringInteractable.sfx_clickSound);
            }
            else{
                context.m_hoveringInteractable.OnFailClick(context);
                // m_playerAudio.PlayFeedback(string.Empty);
            }
        }
        //Releasing Behavior
        else if(context.m_holdingInteractable != null){
            context.ReleaseHoldedInteractable();
        }
    }
}
public class OverviewState: PlayerState{}