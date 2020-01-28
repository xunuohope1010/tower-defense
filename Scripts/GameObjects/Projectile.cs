using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Monster target;
    private Tower parent;
    private Animator myAnimator;
    private Element elementType;

	// Use this for initialization
	void Start ()
    {
        myAnimator = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveToTarget();    	
	}

    public void Initialize(Tower parent)
    {
        target = parent.Target;
        this.parent = parent;
        elementType = parent.ElementType;
    }

    private void MoveToTarget()
    {
        if (target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, parent.ProjectileSpeed * Time.deltaTime);

            // rotate the projectile into the correct direction
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg; // in degrees

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (!target.IsActive)
        {
            Game_Manager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage, elementType);   // parent is the tower that the projectile came from

                myAnimator.SetTrigger("Impact");

                ApplyDebuff();
            }
        }
    }

    private void ApplyDebuff()
    {
        if (target.ElementType != elementType)  // probability is implemented with applying the debuff only if the projectile element type is not equivalent to the monster's element type
        {
            float roll = Random.Range(0, 100);

            if (roll <= parent.Proc)
            {
                target.AddDebuff(parent.GetDebuff());
            }
        }
    }
}
