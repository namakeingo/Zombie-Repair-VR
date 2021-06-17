using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5;
    public float damage = 5;
    public float lifeTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        lifeTime -= Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (transform.position.y < 0.0f || transform.position.y > 15.0f)
        {
            Destroy(gameObject);
        }

        if (lifeTime <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Player shot themselves
        if (other.CompareTag("Player"))
        {
            PlayerHandler playerHandler = FindObjectOfType<PlayerHandler>();
            playerHandler.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
