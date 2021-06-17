using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define behaviour of gun based on the barrel
/// </summary>
public class WeaponBarrel : MonoBehaviour
{
    public GameObject bulletPrefab;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Shoot(int spread = 1)
    {
        //shoot bullets and play sound
        if (audioSource)
        {
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();
        }

        var miss = Mathf.Log(spread) * 0.25f;
        var angle = (360f / spread);

        for (int i = 0; i <= spread; i++)
        {
            var bulletAngle = angle * i;
            var rotOffset = new Vector3(Mathf.Sin(bulletAngle) * miss, Mathf.Cos(bulletAngle) * miss, 1f);
            var rot = Quaternion.LookRotation(rotOffset);

            Instantiate(bulletPrefab, transform.position, transform.rotation * rot);
        }
    }
}
