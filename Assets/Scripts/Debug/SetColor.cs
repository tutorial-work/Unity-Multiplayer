using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    [SerializeField] Color color = new Color();

    /// <summary>
    /// Unity Method; This function is called when the script is loaded or a value is changed in the
    /// Inspector (Called in the editor only)
    /// </summary>
    void OnValidate()
    {
        Set();
    }

    /// <summary>
    /// Unity Method; Awake() is called before Start() upon GameObject creation
    /// </summary>
    void Awake()
    {
        Set();
    }

    void Set()
    {
        Debug.Log("Setting Color on " + name);

        Material mat = GetComponent<Renderer>().sharedMaterial;
        mat.SetColor("_BaseColor", color);
    }
}
