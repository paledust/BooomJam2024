using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TVGameControl : MonoBehaviour
{
[Header("Input")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction restAction;
[Header("Player Move")]
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private Rigidbody2D player_rigid;
[Header("NPC")]
    [SerializeField] private Rigidbody2D npc_rigid;
[Header("Ball")]
    [SerializeField] private Rigidbody2D ball_rigid;
    private Vector2 moveSpeed = Vector2.zero;
    void OnEnable(){
        moveAction.performed += Move;
        moveAction.canceled += Move;
        moveAction.Enable();

        restAction.canceled += Restart;
        restAction.Enable();
    }
    void OnDisable(){
        moveAction.Disable();
        moveAction.performed -= Move;
        moveAction.canceled -= Move;

        restAction.Disable();
        restAction.canceled -= Restart;
    }
    void FixedUpdate()
    {
        player_rigid.velocity = Vector2.Lerp(player_rigid.velocity, moveSpeed, Time.fixedDeltaTime*5);
    }
    void Restart(InputAction.CallbackContext callback){
        player_rigid.transform.localPosition = Vector2.left*0.55f;
        npc_rigid.transform.localPosition = Vector2.right*0.55f;
        ball_rigid.transform.localPosition = Vector3.zero;

        player_rigid.position = player_rigid.transform.position;
        npc_rigid.position = npc_rigid.transform.position;
        ball_rigid.position = ball_rigid.transform.position;

        player_rigid.velocity = Vector2.zero;
        npc_rigid.velocity = Vector2.zero;
        ball_rigid.velocity = Vector2.zero;
    }
    void Move(InputAction.CallbackContext callback){
        var input = callback.ReadValue<Vector2>();
        moveSpeed = input*maxSpeed;
    }
}
