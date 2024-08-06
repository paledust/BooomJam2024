using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerEyeControl : MonoBehaviour{
[Header("Eye Transform")]
    [SerializeField] private float maxEyeAngle = 90;
    [SerializeField] private float minEyeAngle = -20;
    [SerializeField] private Transform upperEye;
    [SerializeField] private Transform lowerEye;

[Header("Blink")]
    [SerializeField] private float eyeCloseTime = 2;
    [SerializeField] private float eyeBlinkDarkTime = 0.1f;
    [SerializeField] private float eyeReopenTime = 2;

[Header("VFX")]
    [SerializeField] private Volume blinkPP;

    private float eyeAngleDelta = 0;
    private CoroutineExcuter eyeBlinker;

    void Start(){
        eyeBlinker = new CoroutineExcuter(this);
    }
    public void BlinkEye()=>eyeBlinker.Excute(coroutineBlinkEye());
    IEnumerator coroutineBlinkEye(){
        float initEyeDelta = eyeAngleDelta;
        yield return new WaitForLoop(eyeCloseTime, (t)=>{
            eyeAngleDelta = Mathf.Lerp(0, 1, t);
            float eyeAngle = Mathf.Lerp(minEyeAngle, maxEyeAngle, (1-eyeAngleDelta));
            upperEye.transform.localRotation = Quaternion.Euler(-eyeAngle,0,0);
            lowerEye.transform.localRotation = Quaternion.Euler(eyeAngle,0,0);
            blinkPP.weight = EasingFunc.Easing.QuadEaseOut(eyeAngleDelta);
        });

        yield return new WaitForSeconds(eyeBlinkDarkTime);

        yield return new WaitForLoop(eyeReopenTime, (t)=>{
            eyeAngleDelta = Mathf.Lerp(1, 0, EasingFunc.Easing.QuadEaseOut(t));
            float eyeAngle = Mathf.Lerp(minEyeAngle, maxEyeAngle, (1-eyeAngleDelta));
            upperEye.transform.localRotation = Quaternion.Euler(-eyeAngle,0,0);
            lowerEye.transform.localRotation = Quaternion.Euler(eyeAngle,0,0);
            blinkPP.weight = EasingFunc.Easing.SmoothInOut(eyeAngleDelta);
        });
    }
}