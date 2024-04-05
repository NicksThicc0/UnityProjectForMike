using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class YSorter : MonoBehaviour
{
    [SerializeField] SortingGroup gfx;
    [SerializeField] int offset;
    [SerializeField] bool sortOnce;
    float minY;

    private void Start()
    {
        if (sortOnce)
        {
            minY = Mathf.Round(transform.position.y * 10);
            gfx.sortingOrder = -(int)minY + +1000 + offset;
        }
    }

    private void Update()
    {
        minY = Mathf.Round(transform.position.y * 10);
        gfx.sortingOrder = -(int)minY + +1000 + offset;
    }
    private void OnEnable()
    {
        if (sortOnce)
        {
            minY = Mathf.Round(transform.position.y * 10);
            gfx.sortingOrder = -(int)minY + +1000 + offset;
        }
    }
}
