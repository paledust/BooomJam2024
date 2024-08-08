using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : State<PlayerControl>
{
    protected Camera mainCam;
    protected bool isFocused = false;
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
            if(context.m_hoveringInteractable.IsAvailable){
                EventHandler.Call_OnPlayerInteract();
                context.m_hoveringInteractable.OnClick(context);
            }
            else{
                context.m_hoveringInteractable.OnFailClick(context);
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
        if(context.RaycastDetectInteractable(ray)){
            if(!isFocused){
                isFocused = true;
                context.FadeFocusPP(isFocused);
            }
        }
        else{
            if(isFocused){
                isFocused = false;
                context.FadeFocusPP(isFocused);
            }
        }
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
        blinkOnClick = false;
        EventHandler.Call_UI_SwitchFreeCursor(true);
        EventHandler.Call_UI_OnCursorPosChange(mouseViewPortPos);
        EventHandler.Call_UI_OnCursorHover(false);
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
        if(context.RaycastDetectInteractable(ray)){
            if(!isFocused){
                isFocused = true;
                EventHandler.Call_UI_OnCursorHover(true);
            }
        }
        else{
            if(isFocused){
                isFocused = false;
                EventHandler.Call_UI_OnCursorHover(false);
            }
        }
        return null;
    }
    public override void HandleRightClick(InputValue value, PlayerControl context){
        context.GoToOverview();
    }
}
public class InspecteState: PlayerState{
    public override void EnterState(PlayerControl context)
    {
        base.EnterState(context);
        EventHandler.Call_UI_SwitchFreeCursor(false);
    }
    public override State<PlayerControl> UpdateState(PlayerControl context)
    {
        return null;
    }
    public override void HandleClick(InputValue value, PlayerControl context)
    {
    }
    public override void HandleRightClick(InputValue value, PlayerControl context)
    {
    }
}