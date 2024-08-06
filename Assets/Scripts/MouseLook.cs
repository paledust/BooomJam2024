using UnityEngine;

[System.Serializable]
public class MouseLookData
{
    public float Sensitivity = .15f;
    public float MinimumX = -90f;
    public float MaximumX = 90f;
    public float MinimumY = -90f;
    public float MaximumY = 90f;
    public float smoothSpeed = 20f;
}
[System.Serializable]
public class MouseLook{
    [SerializeField] private MouseLookData defaultMouseLookData;
    [SerializeField] private Transform characterTrans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private bool applyInitOffset;
    public bool clampVerticalRotation = true;
    public bool clampHorizontalRotation = false;
    public bool useSmooth;
    public float Sensitivity = 0.15f;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public float MinimumY = -70f;
    public float MaximumY = 70f;
    public float smoothSpeed = 5f;
    
    private Quaternion initRot;
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    public void Init(){
        m_CharacterTargetRot = applyInitOffset?Quaternion.identity:characterTrans.localRotation;
        m_CameraTargetRot = camTrans.localRotation;
        initRot = characterTrans.localRotation;
    }
    public void SetDefaultMouseLookData()=>SetMouseLookData(defaultMouseLookData);
    public void SetMouseLookData(MouseLookData mouseLookData){
        Sensitivity = mouseLookData.Sensitivity;
        MinimumX = mouseLookData.MinimumX;
        MaximumX = mouseLookData.MaximumX;
        MinimumY = mouseLookData.MinimumY;
        MaximumY = mouseLookData.MaximumY;
        smoothSpeed = mouseLookData.smoothSpeed;
    }
    public void SetHorizontalLimit(Vector2 limit){
        MinimumY = limit.x;
        MaximumY = limit.y;
    }
    public void SetVerticalLimit(Vector2 limit){
        MinimumX = limit.x;
        MaximumX = limit.y;
    }
    public void SetSensitivity(float _sensitivity){
        Sensitivity = _sensitivity;
    }
    public void RecalculateRotationFromViewImmediately()=>RecalculateRotationFromTransform(Camera.main.transform);
    public void RecalculateRotationFromTransform(Transform target){
        characterTrans.rotation = Quaternion.Euler(0,target.rotation.eulerAngles.y,0);

        Quaternion localRot = Quaternion.Inverse(characterTrans.rotation) * target.rotation;
        m_CharacterTargetRot= Quaternion.identity;
        m_CameraTargetRot   = Quaternion.Euler(localRot.eulerAngles.x, 0, 0);
        initRot = characterTrans.rotation;
        camTrans.localRotation = m_CameraTargetRot;
    }
    public void LookRotation(Vector2 input){
        float yRot = input.x * Sensitivity;
        float xRot = input.y * Sensitivity;

        m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
        if(clampHorizontalRotation) m_CharacterTargetRot = ClampRotationAroundYAxis (m_CharacterTargetRot);

        m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);
        if(clampVerticalRotation) m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);
    }
    public void UpdateLookRotation(){
        if(useSmooth){
            characterTrans.localRotation = Quaternion.Slerp (characterTrans.localRotation, initRot * m_CharacterTargetRot,
                smoothSpeed * Time.deltaTime);
            camTrans.localRotation = Quaternion.Slerp (camTrans.localRotation, m_CameraTargetRot,
                smoothSpeed * Time.deltaTime);
        }
        else{
            characterTrans.localRotation = initRot * m_CharacterTargetRot;
            camTrans.localRotation = m_CameraTargetRot;
        }
    }
    public void UpdateHeadRotationOnly(){
        if(useSmooth){
            camTrans.localRotation = Quaternion.Slerp (camTrans.localRotation, m_CameraTargetRot*m_CharacterTargetRot,
                smoothSpeed * Time.deltaTime);
        }
        else{
            camTrans.localRotation = m_CameraTargetRot*m_CharacterTargetRot;
        }        
    }
    Quaternion ClampRotationAroundXAxis(Quaternion q){
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }
    Quaternion ClampRotationAroundYAxis(Quaternion q){
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.y);
        angleY = Mathf.Clamp (angleY, MinimumY, MaximumY);
        q.y = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleY);
        return q;
    }
}