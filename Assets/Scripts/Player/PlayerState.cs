using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : State<PlayerControl>
{
    protected Camera mainCam;
    public bool m_focusOnHover = true;
    protected bool blinkOnClick = true;
    public override void EnterState(PlayerControl context)
    {
        mainCam = Camera.main;
    }
    public override State<PlayerControl> UpdateState(PlayerControl context)
    {
        context.m_mouseLook.UpdateLookTrans();
        return null;
    }
    public virtual void HandleLook(InputValue value, PlayerControl context){
        if(!context.m_controlable) return;
        Vector2 input = value.Get<Vector2>();
        context.m_mouseLook.HandleLookInput(input);
    }
    public virtual void HandleClick(InputValue value, PlayerControl context){
        if(!context.m_controlable) return;
    //Pressing Behavior
        if(value.isPressed){
            if(blinkOnClick) context.eyeControl.BlinkEye();
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
    public virtual void HandleRightClick(InputValue value, PlayerControl context){}
}
public class OverviewState: PlayerState{
    public override void EnterState(PlayerControl context)
    {
        base.EnterState(context);
        EventHandler.Call_OnPlayerOverview(); 
        EventHandler.Call_UI_SwitchFreeCursor(false);
    }
    public override State<PlayerControl> UpdateState(PlayerControl context)
    {
        base.UpdateState(context);
        Ray ray = mainCam.ViewportPointToRay(Vector2.one*0.5f);
        context.RaycastDetectInteractable(ray);
        return null;
    }
}
public class ObserveState: PlayerState{
    public float mouseSpeed = 0.1f;
    private Vector2 mouseViewPortPos;
    public override void EnterState(PlayerControl context)
    {
        base.EnterState(context);
        mouseViewPortPos = Vector2.one*0.5f;
        m_focusOnHover = false;
        blinkOnClick = false;
        EventHandler.Call_UI_SwitchFreeCursor(true);
        EventHandler.Call_UI_OnCursorPosChange(mouseViewPortPos);
    }
    public override void HandleLook(InputValue value, PlayerControl context){
        if(!context.m_controlable) return;
        Vector2 input = value.Get<Vector2>();
        context.m_mouseLook.HandleLookInput(input);
        input.y *= mainCam.pixelWidth/mainCam.pixelHeight;

        mouseViewPortPos += input*mouseSpeed*Time.deltaTime;
        mouseViewPortPos.x = Mathf.Clamp01(mouseViewPortPos.x);
        mouseViewPortPos.y = Mathf.Clamp01(mouseViewPortPos.y);
        EventHandler.Call_UI_OnCursorPosChange(mouseViewPortPos);
    }
    public override State<PlayerControl> UpdateState(PlayerControl context)
    {
        base.UpdateState(context);
        Ray ray = mainCam.ViewportPointToRay(mouseViewPortPos);
        context.RaycastDetectInteractable(ray);
        return null;
    }
    public override void HandleRightClick(InputValue value, PlayerControl context){
        context.GoToOverview();
    }
}