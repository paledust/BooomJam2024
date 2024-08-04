using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public MouseLook m_mouseLook;
    void Start(){
        m_mouseLook.Init();
    }
    void Update(){
        m_mouseLook.UpdateLookRotation();
    }
    private void OnLook(InputValue value){
        Vector2 input = value.Get<Vector2>();
        m_mouseLook.LookRotation(input);
    }
}
