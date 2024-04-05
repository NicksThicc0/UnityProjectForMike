using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRenderDistance:MonoBehaviour
{
    public float derenderRange = 30;
    public bool canBeDestroyed;


    private void Start()
    {
        ObjectRenderManager.instance.renderedObjects.Add(this);
    }

    private void OnDestroy()
    {
        ObjectRenderManager.instance.renderedObjects.Remove(this);
    }
}
