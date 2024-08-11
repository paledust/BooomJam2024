using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour
{
    [SerializeField] private Material tvMat;
    [SerializeField] private RenderTexture gameRT;
    [SerializeField] private int gameChannel;
    [SerializeField] private Clickable_SwitchView tvView;
    [SerializeField] private Transform overlayRoot;
    [SerializeField] private Animation tvAnimation;
[Header("Button Trans")]
    [SerializeField] private Transform knobSwitch;
    [SerializeField] private Transform powerSwitch;
    private const int TOTAL_CHANNEL = 12;
    private bool isOn = false;
    private int currentChannel = 0;
    private GameObject stickingItem;

    void OnEnable(){
        EventHandler.E_OnPlayerOverview += handlePlayerOverview;
    }
    void OnDisable(){
        EventHandler.E_OnPlayerOverview -= handlePlayerOverview;
    }
    void Start(){
        RefreshScreen();
        tvMat.SetFloat("_ImageBrightness", isOn?2:0);
    }
    void handlePlayerOverview(){
        tvView.EnableHitbox();
    }
    public void ChangeChannle(){
        currentChannel += 1;
        currentChannel %= TOTAL_CHANNEL;
        
        if(!isOn) return;
        RefreshScreen();

        tvAnimation.Play();
    }
    public void SwitchPower(){
        isOn = !isOn;
        powerSwitch.localRotation = Quaternion.Euler(0,0,isOn?-8:8);
        if(isOn) RefreshScreen();
        tvMat.SetFloat("_ImageBrightness", isOn?2:0);
    }
    void RefreshScreen(){
        knobSwitch.localRotation = Quaternion.Euler(0,-currentChannel*30,0);
        tvMat.SetFloat("_ScrollingStaticStrength", currentChannel==gameChannel?0.001f:0.5f);
        tvMat.SetFloat("_StaticStrength", currentChannel==gameChannel?0.001f:0.1f);
        tvMat.SetTexture("_ScreenTexture",currentChannel==gameChannel?gameRT:null);
    }
    public void StickOverlay(GameObject stickOverlay){
        if(stickingItem!=null) Destroy(stickingItem);
        tvView.OnClick(FindFirstObjectByType<PlayerControl>());
        stickingItem = Instantiate(stickOverlay);
        stickingItem.transform.parent = overlayRoot;
        stickingItem.transform.localPosition = Vector3.zero;
        stickingItem.transform.localRotation = Quaternion.Euler(Random.Range(85,95),90,90);
        stickingItem.layer = Service.DefaultLayer;
    }
}
