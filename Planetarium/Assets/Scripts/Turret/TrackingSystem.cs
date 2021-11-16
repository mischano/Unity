using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingSystem : MonoBehaviour
{
    public float speed = 50.0f;
    GameObject m_target = null;
    Vector3 m_lastKnownPos = Vector3.zero;
    Quaternion m_lookAtRotation;

    void Start()
    {
        m_target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_target)
        {
            if (m_lastKnownPos != m_target.transform.position)
            {
                m_lastKnownPos = m_target.transform.position;
                m_lookAtRotation = Quaternion.LookRotation(m_lastKnownPos - transform.position);
            }

            if (transform.rotation != m_lookAtRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_lookAtRotation, speed * Time.deltaTime);
            }
        }
    }

    bool SetTarget(GameObject target)
    {
        if (target)
        {
            return false;
        }

        m_target = target;

        return true;
    }
}
