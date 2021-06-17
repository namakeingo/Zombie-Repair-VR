using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public void Die()
    {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.activeItems.Remove(this.gameObject);
        Destroy(gameObject);
    }
}
