using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomAnimationEvent : MonoBehaviour
{
    [SerializeField] animEvents[] Events;
    [SerializeField] bool hitBox;

    public void playEvent(string eventName)
    {
        for (int i = 0; i < Events.Length; i++)
        {
            if (Events[i].eventName == eventName)
            {
                Events[i].Event.Invoke();
            }
        }
    }


    public void SpawnObject(GameObject spawntObject)
    {
        GameObject newObject = Instantiate(spawntObject,transform.position,transform.rotation);
        Destroy(newObject,20);

    }
    public void DestroyObject(GameObject destroyedObject)
    {
        Destroy(destroyedObject);
    }
    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hitBox) return;
        if(collision.gameObject.tag == "Enemy")
        {
            playEvent("HitBox");
        }
    }
}



[Serializable]
class animEvents
{
    public string eventName;
    public UnityEvent Event;
}
