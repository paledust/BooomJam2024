using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour
{
    [SerializeField] private Material tvMat;
    [SerializeField] private int gameChannel;
    [SerializeField] private Clickable_SwitchView tvView;
    [SerializeField] private Transform overlayRoot;
    [SerializeField] private Animation tvAnimation;
[Header("Button Trans")]
    [SerializeField] private Transform knobSwitch;
    [SerializeField] private Transform powerSwitch;
    private const int TOTAL_CHANNEL = 12;
    private int currentChannel = 1;
    private GameObject stickingItem;

    void OnEnable(){
        EventHandler.E_OnPlayerOverview += handlePlayerOverview;
    }
    void OnDisable(){
        EventHandler.E_OnPlayerOverview -= handlePlayerOverview;
    }
    void handlePlayerOverview(){
        tvView.EnableHitbox();
    }
    public void ChangeChannle(int channelIndex){
        currentChannel = channelIndex;
        currentChannel %= TOTAL_CHANNEL;
        
        knobSwitch.localRotation = Quaternion.Euler(0,-channelIndex*30,0);
        tvMat.SetFloat("_ScrollingStaticStrength", channelIndex==gameChannel?0.001f:0.5f);
        tvMat.SetFloat("_StaticStrength", channelIndex==gameChannel?0.001f:0.1f);

        tvAnimation.Play();
    }
    public void SwitchPower(bool isOn){
        tvMat.SetFloat("_ImageBrightness", isOn?0:2);
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
