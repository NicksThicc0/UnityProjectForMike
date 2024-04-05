using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{

    private void Start()
    {
        Snap(transform);
    }

    public static void Snap(Transform snappedObject)
    {
        snappedObject.position = new Vector2(Mathf.Round(snappedObject.position.x), Mathf.Round(snappedObject.position.y));
    }

    private void Update()
    {
        Snap(transform);
    }
}
