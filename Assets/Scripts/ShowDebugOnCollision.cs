using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDebugOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Impacted at "+ collision.contacts[0].point);

        Debug.DrawRay(
            collision.contacts[0].point,   // the point where the collision happened 
            collision.contacts[0].normal,   // the direction that point is facing is the normal
            Color.red,
            1f);
    }
}
