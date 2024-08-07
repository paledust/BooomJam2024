using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : Singleton<UI_Manager>
{
    private Camera mainCam;
[Header("UI Element")]
    [SerializeField] private RectTransform cursorPointer;
    [SerializeField] private GameObject centerPointer;
    protected override void Awake(){
        base.Awake();
        mainCam = Camera.main;
        EventHandler.E_AfterLoadScene += handleAfterLoadScene;
        EventHandler.E_UI_SwitchFreeCursor += handle_UI_SwitchFreeCursor;
        EventHandler.E_UI_OnCursorPosChange += handleCursorPosChange;
    }
    protected override void OnDestroy(){
        base.OnDestroy();
        EventHandler.E_AfterLoadScene -= handleAfterLoadScene;
        EventHandler.E_UI_SwitchFreeCursor -= handle_UI_SwitchFreeCursor;
        EventHandler.E_UI_OnCursorPosChange -= handleCursorPosChange;
    }
    void handleAfterLoadScene(){
        mainCam = Camera.main;
    }
    void handleCursorPosChange(Vector2 newViewPos){
        cursorPointer.position = mainCam.ViewportToScreenPoint(newViewPos);
    }
    void handle_UI_SwitchFreeCursor(bool isFreeCursor){
        cursorPointer.gameObject.SetActive(isFreeCursor);
        centerPointer.gameObject.SetActive(!isFreeCursor);
    }
}
