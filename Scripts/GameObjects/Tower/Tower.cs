using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour {

    [SerializeField] private string projectileType;

    private Queue<Monster> monsters = new Queue<Monster>();

    private bool canAttack = true;
    private float attackTimer;
    [SerializeField] private float attackCooldown, projectileSpeed;
    public float ProjectileSpeed { get { return projectileSpeed; } }

    [SerializeField] private float damage, debuffDuration, proc;
    public float Damage { get { return damage; } }
    public float DebuffDuration { get { return debuffDuration; } set { debuffDuration = value; } }
    public float Proc { get { return proc; } }

    public int Level { get; protected set; }

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private Monster target;
    public Monster Target { get { return target; } }

    public Element ElementType { get; protected set; }
    public int Price { get; set; }

    public TowerUpgrade[] Upgrades { get; protected set; }
    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length > Level-1)
            {
                return Upgrades[Level-1];
            }
            return null;
        }
    }

    // Use this for initialization
    void Awake()
    {
        myAnimator = transform.parent.GetComponent<Animator>();
        mySpriteRenderer = transform.GetComponent<SpriteRenderer>();

        Level = 1;
    }
	
	// Update is called once per frame
	void Update()
    {
        Attack();
	}

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        Game_Manager.Instance.UpdateUpgradeTip();
    }

    private void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }

        if (target == null && monsters.Count > 0 && monsters.Peek().IsActive)   // Peek() checks the monster on top of the stack
        {
            target = monsters.Dequeue();    // removes first monster from queue and store it into target
        }
        else if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Projectile projectile = Game_Manager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
                projectile.transform.position = transform.position;
                projectile.Initialize(this);

                myAnimator.SetTrigger("Attack");

                canAttack = false;
            }
        }
        else if (target != null && !target.Alive || target != null && !target.IsActive)
        {
            target = null;
        }
    }

    public abstract Debuff GetDebuff();

    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            /**
             * 
             */
            Debug.Log(string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff>+{4}</color> \nProc: {2}% <color=#00ff00ff>+{5}%</color> \nDebuff Duration: {3}sec <color=#00ff00ff>+{6}sec</color>", Level, Damage, Proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration));
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff>+{4}</color> \nProc: {2}% <color=#00ff00ff>+{5}%</color> \nDebuff Duration: {3}sec <color=#00ff00ff>+{6}sec</color>", Level, Damage, Proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }
        /**
         * 
         */
        Debug.Log(string.Format("\nLevel: {0} \nDamage: {1} \nProc: {2}% \nDebuff Duration: {3}sec", Level, Damage, Proc, DebuffDuration));
        
        return string.Format("\nLevel: {0} \nDamage: {1} \nProc: {2}% \nDebuff Duration: {3}sec", Level, Damage, Proc, DebuffDuration);
    }

    public virtual void Upgrade()
    {
        Game_Manager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        damage += NextUpgrade.Damage;
        proc += NextUpgrade.ProcChance;
        DebuffDuration += NextUpgrade.DebuffDuration;
        Level++;
        Game_Manager.Instance.UpdateUpgradeTip();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            target = null;
        }
    }
}