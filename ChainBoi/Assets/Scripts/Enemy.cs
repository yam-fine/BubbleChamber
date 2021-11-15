using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    //[SerializeField] int eatScore;

    //public float EatScore { get { return eatScore; } }
    //public float KillScore { get { return killScore; } }

    Transform player;
    private Rigidbody2D rb;
    private bool isFood = true;
    private Transform closestTarget;
    float minDistFromOtherEnemy = 2; // minimum distance from another enemy in order to become an enemy
    bool toBeDeleted;
    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = Player.Instance.transform;
        closestTarget = FindClosestTarget();
        player.GetComponent<Player>().enemyDestroyedEvent.AddListener(EnemyKilledOrEaten);
        toBeDeleted = false;
        anim = GetComponent<Animator>();
        anim.SetBool("Food", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFood)
            Food();
        else
            Enemyy();
    }

    // called when any enemy has been killed or eaten
    void EnemyKilledOrEaten() {
        closestTarget = FindClosestTarget();
        if (closestTarget != null) {
            if (Vector2.Distance(transform.position, closestTarget.position) > minDistFromOtherEnemy) {
                if (!toBeDeleted)
                    gameObject.tag = "Food";
                isFood = true;
                anim.SetBool("Food", true);
            }
        }
        gameObject.tag = "Food";
        isFood = true;
        anim.SetBool("Food", true);
    }
    
    Transform FindClosestTarget()
    {     
        float distanceToClosestTarget = Mathf.Infinity;
        Transform closestTarget = null;
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
        GameObject[] allEnems = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] allTargets = allFood.Concat(allEnems).ToArray();

        foreach (GameObject targ in allTargets)
        {
            if (targ.gameObject == this.gameObject)
            {
                continue;
            }
            
            float distanceToTarget = (targ.transform.position - transform.position).sqrMagnitude;
            if (distanceToTarget < distanceToClosestTarget)
            {
                distanceToClosestTarget = distanceToTarget;
                closestTarget = targ.transform;
            }
        }
        return closestTarget;
    }

    private void OnTriggerEnter2D(Collider2D other) // only for collision between enemies, NOT PLAYER
    {
        if (other.tag == "Food" || other.tag == "Enemy")
        {
            TurnToEnemy();
        }
        else if (other.tag == "Tail") {
            Die();
        }
    }

    void TurnToEnemy() {
        gameObject.tag = "Enemy";
        isFood = false;
        anim.SetBool("Food", false);
    }

    void Food() {
        //todo : we might want different values of speed on each level.
        if (closestTarget != null)
            transform.position = Vector2.Lerp(transform.position, closestTarget.position, speed * Time.deltaTime);
    }

    void Enemyy() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    // called from player if this gameobject touches him
    public void Die() {
        toBeDeleted = true;
        gameObject.tag = "Untagged";
        player.GetComponent<Player>().enemyDestroyedEvent.Invoke();
        Destroy(gameObject);
    }
}


