using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_rigid;
    [SerializeField] private float maxSpeed = 1;
    // Update is called once per frame
    void FixedUpdate()
    {
        m_rigid.velocity = Vector3.ClampMagnitude(m_rigid.velocity, maxSpeed);
    }
}
