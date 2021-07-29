using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var collider = GetComponent<BaseCollider>();
        collider.OnColliderEnter += OnColliderEnter;
        collider.OnColliderStay += OnColliderStay;
        collider.OnColliderExit += OnColliderExit;
    }


    private void OnColliderEnter(BaseCollider collider)
    {
        Debug.Log("OnColliderEnter---" + collider);
    }

    private void OnColliderStay(BaseCollider collider)
    {
        Debug.Log("OnColliderStay---" + collider);

    }

    private void OnColliderExit(BaseCollider collider)
    {
        Debug.Log("OnColliderExit---" + collider);

    }

}
