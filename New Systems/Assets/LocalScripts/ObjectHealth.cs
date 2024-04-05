using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] string Event;
    CustomAnimationEvent customEvent;

    private void Awake()
    {
        customEvent = GetComponent<CustomAnimationEvent>();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if(Health <= 0)
        {
            if(destroyEffect != null)
            {
                GameObject newEffect = Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(newEffect, 20);
            }
            if(customEvent != null)
            {
                customEvent.playEvent(Event);
            }

            Destroy(gameObject);
        }
    }

}
