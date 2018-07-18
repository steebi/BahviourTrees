using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameObjectFollower : MonoBehaviour {

    public GameObject m_targetGameObject;

    private void LateUpdate()
    {
        Transform position = m_targetGameObject.transform;
        transform.position = new Vector3(position.position.x, transform.position.y, position.position.z);
    }

}
