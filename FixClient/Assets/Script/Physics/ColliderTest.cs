using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ColliderTest : MonoBehaviour
{
    public RayShape rayShape = new RayShape(Vector2.zero, Vector2.one);
    public Vector2 point1 = Vector2.one;
    public Vector2 point2 = Vector2.one;
    void Start()
    {
        var collider = GetComponent<BaseCollider>();
        collider.OnColliderEnter += OnColliderEnter;
        collider.OnColliderStay += OnColliderStay;
        collider.OnColliderExit += OnColliderExit;

        print(Vector2.Dot(point1, point2.normalized));
    }
    private void Update()
    {

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

