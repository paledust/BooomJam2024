using UnityEngine;

public abstract class Basic_Clickable : MonoBehaviour
{
[Header("Clickable Basic")]
    public string sfx_clickSound = string.Empty;
    [SerializeField] protected Collider hitbox;
    [SerializeField] protected bool isAvailable = true; //If not available, player can still click on object but will show stop sign

    public bool IsAvailable{get{return isAvailable;}}
    public bool IsInteractable{get{return gameObject.activeInHierarchy && isAvailable && hitbox.enabled;}}
    public Collider m_hitbox{get{return hitbox;}}
    
    public virtual void OnHover(PlayerControl player){}
    public virtual void OnExitHover(){}
    public virtual void OnClick(PlayerControl player){}
    public virtual void OnRelease(PlayerControl player){}
    public virtual void OnFailClick(PlayerControl player){}
    protected virtual void OnBecomeInteractable(){}
    protected virtual void OnBecomeUninteractable(){}

#region Interaction Activation
    public void FreezeInteraction(){
        if(IsInteractable) OnBecomeUninteractable();
        isAvailable = false;
    }
    public void UnfreezeInteraction(){
        if(gameObject.activeInHierarchy && m_hitbox.enabled) OnBecomeInteractable();
        isAvailable = true;
    }
    public void DisableHitbox(){
        if(IsInteractable) OnBecomeUninteractable();
        m_hitbox.enabled = false;
    }
    public void EnableHitbox(){
        if(gameObject.activeInHierarchy && isAvailable) OnBecomeInteractable();
        m_hitbox.enabled = true;
    }
    public void EnableRaycast()=>gameObject.layer = Service.interactableLayer;
    public void DisableRaycast()=>gameObject.layer = Service.ignoreRaycastLayer;
#endregion

    void Reset()=>hitbox = GetComponent<Collider>();
}
