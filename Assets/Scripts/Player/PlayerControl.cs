using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerControl : MonoBehaviour
{
    public MouseLook m_mouseLook;
    public PlayerEyeControl eyeControl;
#region PP
    [SerializeField] private float focusTime = 0.2f;
    [SerializeField] private Volume focusVolume; 
#endregion
    private Camera mainCam;
    private PlayerState currentPlayerState;
    public bool m_controlable{get{return !isTransition;}}
    public Vector3 m_hoverPos{get; private set;}
    public Basic_Clickable m_hoveringInteractable{get; private set;} //The hovering interactable
    public Basic_Clickable m_holdingInteractable{get; private set;} //Currently holding interactable (Not hold by Maie's hand, but by cursor)

    private CoroutineExcuter ppFader;
    private bool isTransition;
    private Vector3 defaultPos;
    private Quaternion defaultRot;
    private Vector3 lastOverviewEuler;

    void Start(){
        m_mouseLook.Init();
        mainCam = Camera.main;
        ppFader = new CoroutineExcuter(this);

        defaultPos = transform.position;
        defaultRot = transform.rotation;

        currentPlayerState = new OverviewState();
        currentPlayerState.EnterState(this);
    }
    void Update(){
        var newState = currentPlayerState.UpdateState(this);
        if(newState != null){
            currentPlayerState = newState as PlayerState;
            currentPlayerState.EnterState(this);
        }
    }

#region Handle Interactable
    void ClearCurrentInteractable(){
        if(m_hoveringInteractable != null){
            m_hoveringInteractable.OnExitHover();
            m_hoveringInteractable = null;
            ppFader.Excute(CommonCoroutine.coroutineLoop(focusTime, (t)=>focusVolume.weight = Mathf.Lerp(1,0,EasingFunc.Easing.QuadEaseOut(t))));
        }
    }
    public void RaycastDetectInteractable(Ray ray){
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
    public void HoldInteractable(Basic_Clickable interactable)=>m_holdingInteractable = interactable;
    public void ReleaseHoldedInteractable(){
        m_holdingInteractable.OnRelease(this);
        m_holdingInteractable = null;
    }
#endregion

#region State Func
    public void GoToObserveView(CinemachineCamera c_cam,MouseLookData mouseLookData){
        currentPlayerState = new ObserveState();
        currentPlayerState.EnterState(this);
        lastOverviewEuler = m_mouseLook.GetPoseEuler();
        StartCoroutine(coroutineBlinkTransition_Twice(()=>{
            transform.position = c_cam.transform.position;
            transform.rotation = c_cam.transform.rotation;

            m_mouseLook.ResetRotation();
            m_mouseLook.SetMouseLookData(mouseLookData);
        }));
    }
    public void GoToOverview(){
        currentPlayerState = new OverviewState();
        currentPlayerState.EnterState(this);
        StartCoroutine(coroutineBlinkTransition_Once(()=>{
            transform.position = defaultPos;
            transform.rotation = defaultRot;

            m_mouseLook.ResetRotation(lastOverviewEuler);
            m_mouseLook.SetDefaultMouseLookData();
        }));
    }
#endregion

#region Input
    void OnRightClick(InputValue value)=>currentPlayerState.HandleRightClick(value, this);
    void OnClick(InputValue value)=>currentPlayerState.HandleClick(value, this);
    void OnLook(InputValue value)=>currentPlayerState.HandleLook(value, this);
#endregion

    IEnumerator coroutineBlinkTransition_Twice(Action transitionAction){
        isTransition = true;
        eyeControl.BlinkEye(()=>
        StartCoroutine(CommonCoroutine.coroutineWait(0.25f, ()=>
        eyeControl.BlinkEye(0.1f, transitionAction, ()=>isTransition = false))));
        yield return null;
    }
    IEnumerator coroutineBlinkTransition_Once(Action transitionAction){
        isTransition = true;
        eyeControl.BlinkEye(0.1f, transitionAction, ()=>isTransition = false);
        yield return null;
    }
}
