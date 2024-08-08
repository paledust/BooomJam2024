using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour
{
    [SerializeField] private Clickable_SwitchView tvView;
    [SerializeField] private Transform overlayRoot;

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
