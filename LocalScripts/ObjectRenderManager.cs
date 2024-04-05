using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRenderManager : MonoBehaviour
{
    public static ObjectRenderManager instance;
    public List<ObjectRenderDistance> renderedObjects;


    private void Awake()
    {
        instance = this;
        StartCoroutine(checkRenders(0));
    }

    IEnumerator checkRenders(float renderTime)
    {

        
        yield return new WaitForSeconds(renderTime);
        //
        for (int i = 0; i < renderedObjects.Count; i++)
        {
            if (Vector3.Distance(renderedObjects[i].transform.position, PlayerMovement.instance.transform.position) > renderedObjects[i].derenderRange)
            {
                renderedObjects[i].gameObject.SetActive(false);
                if (renderedObjects[i].canBeDestroyed)
                {
                    Destroy(renderedObjects[i].gameObject);
                }
            }
            else
            {
                renderedObjects[i].gameObject.SetActive(true);
            }
        }
        StartCoroutine(checkRenders(1));
    }
}
