using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) // The collider of the other object that hits this trigger event or trigger hits
    {
        Color randomlySelectedColor = GetRandomColorWithAlpha();
        GetComponent<Renderer>().material.color = randomlySelectedColor; // Get the renderer of this object and change the material color
    }

    private Color GetRandomColorWithAlpha()
    {
        return new Color(   // Creates a new Color from 0-1 for rgb (on slider scale) | Alpha sets transparency
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            0.9f);
    }
}
