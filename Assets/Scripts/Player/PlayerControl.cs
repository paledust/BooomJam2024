using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MouseLook m_mouseLook;
    [SerializeField] private PlayerEyeControl eyeControl;
#region PP
    [SerializeField] private float focusTime = 0.2f;
    [SerializeField] private Volume focusVolume; 
#endregion
    private Camera mainCam;

    public Vector3 m_hoverPos{get; private set;}
    public Basic_Clickable m_hoveringInteractable{get; private set;} //The hovering interactable
    public Basic_Clickable m_holdingInteractable{get; private set;} //Currently holding interactable (Not hold by Maie's hand, but by cursor)

    private CoroutineExcuter ppFader;
    void Start(){
        mainCam = Camera.main;
        m_mouseLook.Init();
        ppFader = new CoroutineExcuter(this);
    }
    void Update(){
        m_mouseLook.UpdateLookRotation();
        Ray ray = mainCam.ViewportPointToRay(Vector2.one*0.5f);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Service.interactableLayerMask)){
            Basic_Clickable hit_Interactable = hit.collider.GetComponent<Basic_Clickable>();
            m_hoverPos = hit.point;
            if(hit_Interactable!=null){
                if(m_hoveringInteractable != hit_Interactable) {
                    if(m_hoveringInteractable!=null) m_hoveringInteractable.OnExitHover();
                    ppFader.Excute(CommonCoroutine.coroutineLoop(focusTime, (t)=>focusVolume.weight = Mathf.Lerp(0,1,EasingFunc.Easing.QuadEaseOut(t))));
                    m_hoveringInteractable = hit_Interactable;
                    if(m_hoveringInteractable.IsAvailable) m_hoveringInteractable.OnHover(this);
                }
            }
            else{
                ClearCurrentInteractable();
            }
        }
        else{
            ClearCurrentInteractable();
        }
    }
    void ClearCurrentInteractable(){
        if(m_hoveringInteractable != null){
            m_hoveringInteractable.OnExitHover();
            m_hoveringInteractable = null;
            ppFader.Excute(CommonCoroutine.coroutineLoop(focusTime, (t)=>focusVolume.weight = Mathf.Lerp(1,0,EasingFunc.Easing.QuadEaseOut(t))));
        }
    }
    public void HoldInteractable(Basic_Clickable interactable){
        m_holdingInteractable = interactable;
    }
#region Input
    void OnClick(InputValue value){
    //Pressing Behavior
        if(value.isPressed){
            eyeControl.BlinkEye();
            if(m_holdingInteractable != null) return;
            if(m_hoveringInteractable == null) return;
        //Interact with object
            // m_playerCommandManager.AbortCommands();
            if(m_hoveringInteractable.IsAvailable){
                EventHandler.Call_OnPlayerInteract();
                m_hoveringInteractable.OnClick(this);
            //Play Click SFX
                // m_playerAudio.PlayFeedback(m_hoveringInteractable.sfx_clickSound);
            }
            else{
                m_hoveringInteractable.OnFailClick(this);
                // m_playerAudio.PlayFeedback(string.Empty);
            }
        }
        //Releasing Behavior
        else if(m_holdingInteractable != null){
            m_holdingInteractable.OnRelease(this);
            m_holdingInteractable = null;
        }
    }
    private void OnLook(InputValue value){
        Vector2 input = value.Get<Vector2>();
        m_mouseLook.LookRotation(input);
    }
#endregion

#region Coroutine

#endregion
}
