using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int eatScore;

    public float EatScore { get { return eatScore; } }
    //public float KillScore { get { return killScore; } }

    Transform player;
    private Rigidbody2D rb;
    private bool isFood = true;
    private Transform closestTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = Player.Instance.transform;
        closestTarget = FindClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFood)
            Food();
        else
            Enemyy();
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
            gameObject.tag = "Enemy";
            isFood = false;
        }
    }

    void Food() { 
        //todo : we might want different values of speed on each level.
        transform.position = Vector2.Lerp(transform.position, closestTarget.position,  speed * Time.deltaTime);
    }

    void Enemyy() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    public void Die() {
        Destroy(gameObject);
    }
}


