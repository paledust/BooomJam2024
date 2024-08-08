using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Manager : Singleton<UI_Manager>
{
    private Camera mainCam;
[Header("UI Element")]
    [SerializeField] private Image cursorPointer;
    [SerializeField] private GameObject centerPointer;

    private CoroutineExcuter cursorSizeChanger;

    protected override void Awake(){
        base.Awake();
        mainCam = Camera.main;
        EventHandler.E_AfterLoadScene += handleAfterLoadScene;
        EventHandler.E_UI_SwitchFreeCursor += handle_UI_SwitchFreeCursor;
        EventHandler.E_UI_OnCursorPosChange += handleCursorPosChange;
        EventHandler.E_UI_OnCursorHover += handleCursorHover;
    }
    protected override void OnDestroy(){
        base.OnDestroy();
        EventHandler.E_AfterLoadScene -= handleAfterLoadScene;
        EventHandler.E_UI_SwitchFreeCursor -= handle_UI_SwitchFreeCursor;
        EventHandler.E_UI_OnCursorPosChange -= handleCursorPosChange;
        EventHandler.E_UI_OnCursorHover -= handleCursorHover;
    }
    void Start(){
        cursorSizeChanger = new CoroutineExcuter(this);
    }
    void handleCursorHover(bool isHover){
        cursorSizeChanger.Excute(coroutineCursorVisual(isHover, 0.2f));
    }
    void handleAfterLoadScene(){
        mainCam = Camera.main;
    }
    void handleCursorPosChange(Vector2 newViewPos){
        cursorPointer.rectTransform.position = mainCam.ViewportToScreenPoint(newViewPos);
    }
    void handle_UI_SwitchFreeCursor(bool isFreeCursor){
        cursorPointer.gameObject.SetActive(isFreeCursor);
        centerPointer.gameObject.SetActive(!isFreeCursor);
    }
    IEnumerator coroutineCursorVisual(bool isHover, float duration){
        Color initColor = cursorPointer.color;
        Color targetColor = initColor;
        targetColor.a = isHover?0.7f:0.2f;
        Vector3 targetSize = isHover?Vector3.one*1.5f:Vector3.one;
        Vector3 initSize = cursorPointer.rectTransform.localScale;

        yield return new WaitForLoop(duration, (t)=>{
            cursorPointer.rectTransform.localScale = Vector3.LerpUnclamped(initSize, targetSize, EasingFunc.Easing.BackEaseOut(t));
            cursorPointer.color = Color.Lerp(initColor, targetColor, EasingFunc.Easing.SmoothInOut(t));
        });
    }
}
