using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }
    public float MaxSpeed { get; set; }
    [SerializeField] Vector2 minScale = new Vector2(0.1f, 0.1f), maxScale = new Vector2(1, 1);
    [SerializeField] private Stat health;

    private SpriteRenderer spriteRenderer;
    private Animator myAnimator;

    [SerializeField] private Element elementType;
    public Element ElementType { get { return elementType; } }
    private int typeInvulnerability = 2;

    public Point GridPosition { get; set; }
    private Stack<Node> path;
    private Vector3 destination;

    private List<Debuff> debuffs = new List<Debuff>(), debuffsToRemove = new List<Debuff>(), newDebuffs = new List<Debuff>();

    public bool Alive { get { return health.CurrentVal > 0; } }
    public bool IsActive { get; set; }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxSpeed = speed;
        health.Initialize();
    }

    private void Update()
    {
        HandleDebuffs();
        Move();
    }

    public void Spawn(float health)
    {
        transform.position = LevelManager.Instance.BluePortal.transform.position;

        this.health.Bar.Reset();
        this.health.MaxVal = health;
        this.health.CurrentVal = this.health.MaxVal;
        typeInvulnerability = 2;

        StartCoroutine(Scale(minScale, maxScale, false));

        SetPath(LevelManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);    // for changing a value over time
            progress += Time.deltaTime;

            yield return null;  // pause
        }

        transform.localScale = to;  // to make sure that the transform's scale is exact to the desired scale

        IsActive = true;

        if (remove)
        {
            Game_Manager.Instance.Lives--;
            Release();
        }
    }

    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPosition);

                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }

    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            path = newPath;

            Animate(GridPosition, path.Peek().GridPosition);

            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }

    private void Animate(Point currentPos, Point newPos)
    {
        // up and down are inverted for some unknown reason
        if (currentPos.y > newPos.y)
        {
            // moving down
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", 1);
        } 
        else if (currentPos.y < newPos.y)
        {
            // moving up
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
        }
        else // not moving up or down
        {
            if (currentPos.x > newPos.x)
            {
                // moving to the left
                myAnimator.SetInteger("Horizontal", -1);
                myAnimator.SetInteger("Vertical", 0);
            }
            else if (currentPos.x < newPos.x)
            {
                // moving to the right
                myAnimator.SetInteger("Horizontal", 1);
                myAnimator.SetInteger("Vertical", 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RedPortal"))
        {
            StartCoroutine(Scale(maxScale, minScale, true));

            if (other.GetComponent<Portal>() != null)
            {
                other.GetComponent<Portal>().Open();
            }
        }

        if (other.tag == "Tile")
        {
            spriteRenderer.sortingOrder = other.GetComponent<TileScript>().GridPosition.y;
        }
    }

    public void Release()
    {
        debuffs.Clear();    // removes all debuffs

        IsActive = false;
        GridPosition = LevelManager.Instance.BlueSpawn;
        Game_Manager.Instance.Pool.ReleaseObject(gameObject);
        Game_Manager.Instance.RemoveMonster(this);
    }

    public void TakeDamage(float damage, Element dmgSource)
    {
        if (IsActive)
        {
            if (dmgSource == ElementType)
            {
                damage /= typeInvulnerability;
                typeInvulnerability++;
            }

            health.CurrentVal -= damage;

            if (health.CurrentVal <= 0)
            {
                SoundManager.Instance.PlaySFX("Splat");
                Game_Manager.Instance.Currency += 2;
                debuffs.Clear();
                myAnimator.SetTrigger("Die");

                typeInvulnerability = 2;
                IsActive = false;

                GetComponent<SpriteRenderer>().sortingOrder--;
            }
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))  // Looks through the debuffs list, and for each x element in the list, check to see if that element's debuff type is equivalent to the method argument's debuff type
        {
            newDebuffs.Add(debuff);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        debuffsToRemove.Add(debuff);
    }

    private void HandleDebuffs()
    {
        // Add new debuffs
        if (newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear(); // to make sure that new debuffs are only added once
        }

        // Remove debuffs
        foreach (Debuff debuff in debuffsToRemove)
        {
            debuffs.Remove(debuff);
        }

        debuffsToRemove.Clear();    // so that the same set of debuffs to remove are removed

        // Finally, update available debuffs
        foreach(Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }
}