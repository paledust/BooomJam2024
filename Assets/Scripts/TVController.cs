using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour
{
    [SerializeField] private int gameChannel;
    [SerializeField] private RenderTexture gameRT;
    [SerializeField] private PerRendererRetro perRendererRetro;
    [SerializeField] private Animation tvAnimation;
    [SerializeField] private Transform overlayRoot;
[Header("Interaction")]
    [SerializeField] private Clickable_SwitchView tvView;
    [SerializeField] private Clickable_TVChannel channelInteraction;
    [SerializeField] private Clickable_TVPower powerInteraction;
[Header("Button Trans")]
    [SerializeField] private Transform knobSwitch;
    [SerializeField] private Transform powerSwitch;

    private int currentChannel = 0;
    private bool isOn = false;
    private GameObject stickingItem;

    private const int TOTAL_CHANNEL = 12;

    void OnEnable(){
        EventHandler.E_OnPlayerOverview += ResetInteraction;
    }
    void OnDisable(){
        EventHandler.E_OnPlayerOverview -= ResetInteraction;
    }
    void Start(){
        RefreshScreen();
        ResetInteraction();
        perRendererRetro.imageBrightness = isOn?6:0;
        perRendererRetro.Refresh();
    }
    void ResetInteraction(){
        tvView.EnableHitbox();
        channelInteraction.DisableHitbox();
        powerInteraction.DisableHitbox();
    }
    public void EnableTVInteraction(){
        tvView.DisableHitbox();
        channelInteraction.EnableHitbox();
        powerInteraction.EnableHitbox();        
    }
    public void ChangeChannle(){
        currentChannel += 1;
        currentChannel %= TOTAL_CHANNEL;
        knobSwitch.localRotation = Quaternion.Euler(0,-currentChannel*(360/TOTAL_CHANNEL),0);
        
        if(!isOn) return;
        RefreshScreen();

        tvAnimation.Play();
    }
    public void SwitchPower(){
        isOn = !isOn;
        powerSwitch.localRotation = Quaternion.Euler(0,0,isOn?-8:8);
        if(isOn) RefreshScreen();

        perRendererRetro.imageBrightness = isOn?2:0;
        perRendererRetro.Refresh();
    }
    void RefreshScreen(){
        perRendererRetro.scrollingStaticStrength = currentChannel==gameChannel?0.001f:0.5f;
        perRendererRetro.staticStrength = currentChannel==gameChannel?0.001f:0.1f;
        perRendererRetro.screenTex = currentChannel==gameChannel?gameRT:null;
        perRendererRetro.Refresh();
    }
    public void StickOverlay(GameObject stickOverlay){
        if(stickingItem!=null) Destroy(stickingItem);
        tvView.OnClick(FindFirstObjectByType<PlayerControl>());
        stickingItem = Instantiate(stickOverlay);
        stickingItem.transform.parent = overlayRoot;
        stickingItem.transform.localPosition = Vector3.zero;
        stickingItem.transform.localRotation = Quaternion.Euler(180,0,Random.Range(-2.5f,2.5f));
        stickingItem.transform.localScale = Vector3.one * 0.47f;
        stickingItem.layer = Service.DefaultLayer;
    }
}
