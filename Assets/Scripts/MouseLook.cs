using UnityEngine;

[System.Serializable]
public class MouseLookData
{
    public float Sensitivity = .15f;
    public float offsetWidth = 0;
    public float offsetHeight = 0;
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
    [SerializeField] private bool clampVerticalRotation = true;
    [SerializeField] private bool clampHorizontalRotation = false;
    [SerializeField] private bool useSmooth;
    [SerializeField] private float Sensitivity = 0.15f;
    [SerializeField] private float offsetWidth = 0;
    [SerializeField] private float offsetHeight = 0;
    [SerializeField] private float MinimumX = -90F;
    [SerializeField] private float MaximumX = 90F;
    [SerializeField] private float MinimumY = -70f;
    [SerializeField] private float MaximumY = 70f;
    [SerializeField] private float smoothSpeed = 5f;
    
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private Vector3 m_headOffset;

    public void Init(){
        m_CharacterTargetRot = characterTrans.localRotation;
        m_CameraTargetRot = camTrans.localRotation;
        m_headOffset = characterTrans.localPosition;
    }
    public void SetDefaultMouseLookData()=>SetMouseLookData(defaultMouseLookData);
    public void SetMouseLookData(MouseLookData mouseLookData){
        Sensitivity  = mouseLookData.Sensitivity;
        offsetHeight = mouseLookData.offsetHeight;
        offsetWidth  = mouseLookData.offsetWidth;
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
    public void ResetRotation(){
        m_CharacterTargetRot= Quaternion.identity;
        m_CameraTargetRot   = Quaternion.identity;
        characterTrans.localRotation = m_CharacterTargetRot;
        camTrans.localRotation  = m_CameraTargetRot;
    }
    public void ResetRotation(Vector3 euler){
        m_CharacterTargetRot= Quaternion.Euler(0, euler.y, 0);
        m_CameraTargetRot   = Quaternion.Euler(euler.x, 0, 0);
        characterTrans.localRotation = m_CharacterTargetRot;
        camTrans.localRotation  = m_CameraTargetRot;
    }
    public void HandleLookInput(Vector2 input){
        float yInput = input.x * Sensitivity;
        float xInput = input.y * Sensitivity;

        m_CharacterTargetRot *= Quaternion.Euler (0f, yInput, 0f);
        if(clampHorizontalRotation) m_CharacterTargetRot = ClampRotationAroundYAxis (m_CharacterTargetRot);

        m_CameraTargetRot *= Quaternion.Euler (-xInput, 0f, 0f);
        if(clampVerticalRotation) m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

        float yAngle = m_CharacterTargetRot.eulerAngles.y;
        if(yAngle>180) yAngle = yAngle-360f;
        float xAngle = m_CameraTargetRot.eulerAngles.x;
        if(xAngle>180) xAngle = xAngle-360f;
        m_headOffset.x = yAngle/(MaximumY-MinimumY) * offsetWidth;
        m_headOffset.y = -xAngle/(MaximumX-MinimumX) * offsetHeight;
    }
    public void UpdateLookTrans(){
        characterTrans.localPosition = m_headOffset;
        if(useSmooth){
            float lerp = smoothSpeed * Time.deltaTime;
            characterTrans.localRotation = Quaternion.Slerp (characterTrans.localRotation, m_CharacterTargetRot,
                lerp);
            camTrans.localRotation = Quaternion.Slerp (camTrans.localRotation, m_CameraTargetRot,
                lerp);
        }
        else{
            characterTrans.localRotation = m_CharacterTargetRot;
            camTrans.localRotation = m_CameraTargetRot;
        }
    }
    public Vector3 GetPoseEuler(){
        Vector3 euler = Vector3.zero;
        euler = m_CameraTargetRot.eulerAngles+m_CharacterTargetRot.eulerAngles;
        return euler;
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