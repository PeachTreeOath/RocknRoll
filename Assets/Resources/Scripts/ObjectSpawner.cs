using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

    public GameObject objectToSpawn;
    public float spawnDelay = 1f;
    public int enemySupply;
    public float scrollSpeed = 1f;
    protected bool startedSpawning = false;
    public float homeYPosition = 7f; //location for spawners to warp to once they touch the level
    private Rigidbody2D rbody;

    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        ResumeMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 5.16f)
        {
            transform.position = new Vector2(transform.position.x, homeYPosition);
            if (!startedSpawning)
            {
                Invoke("Spawn", spawnDelay);
                startedSpawning = true;
            }
        }
    }

    protected virtual GameObject Spawn()
    {
        enemySupply--;
        GameObject obj = (GameObject)Instantiate(objectToSpawn, transform.position, transform.rotation);
        if (enemySupply > 0)
        {
            Invoke("Spawn", spawnDelay);
        }
        else
        {
            Destroy(gameObject);
        }

        return obj;
    }

    public void StopMovement()
    {
        rbody.velocity = Vector2.zero;
    }

    public void ResumeMovement()
    {
        rbody.velocity = new Vector2(0, -1) * scrollSpeed; //This is really down
    }
}
