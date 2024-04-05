using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brickle : EnemyClass
{
    [Header("Brickle Specs")]
    [SerializeField] bool unstoned;

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, Player.position) <= attackRange & !unstoned)
        {
            anim.Play("Unstone");
        }
        UIstuff();
    }

    private void FixedUpdate()
    {
        if (unstoned)
        {
            agent.SetDestination(Player.position);
            if (agent.destination.x < transform.position.x)
            {
                gfx.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                gfx.localScale = new Vector3(1, 1, 1);
            }
        }

    }

    public override void UIstuff()
    {
        base.UIstuff();
    }

    public override void TakeDamage(int amount)
    {
        if (unstoned)
        {
            base.TakeDamage(amount);
        }
    }
}
