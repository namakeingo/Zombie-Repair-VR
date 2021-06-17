using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using System;
using Valve.VR.InteractionSystem;

/// <summary>
/// Define behaviour of gun base
/// </summary>
public class WeaponScaffold : MonoBehaviour
{
    WeaponSlot[] slots;

    List<WeaponAugment> currentAugments = new List<WeaponAugment>();
    List<WeaponBarrel> currentBarrels = new List<WeaponBarrel>();

    float timer;
    public float baseFireRate = 2;
    float fireRate = 2;
    public int ammo = 50;
    int spread = 1;
    int burstPower;

    // Start is called before the first frame update
    void Start()
    {
        slots = GetComponentsInChildren<WeaponSlot>();
        UpdateWeaponStats();

    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    /// <summary>
    /// Call this every frame if the weapon trigger is held
    /// </summary>
    public void Shooting()
    {
        //Shoot and manage ammos
        if (timer >= 1f / fireRate)
        {
            ammo--;
            timer = 0;

            if (burstPower > 0)
            {
                StartCoroutine(ShootBurst(burstPower));
            }
            else
            {
                Shoot();
            }
        }

        //Destroy weapon if out of ammo
        if (ammo <= 0)
        {
            Explode();
        }
    }

    private IEnumerator ShootBurst(int power)
    {
        for (int i = 0; i < power; i++)
        {
            Shoot();
            yield return new WaitForSeconds(0.2f / (fireRate * power));
        }
    }

    private void Shoot()
    {
        foreach (var barrel in currentBarrels)
        {
            barrel.Shoot(spread);
        }
    }

    private void Explode()
    {
        GetComponent<Interactable>().attachedToHand.DetachObject(gameObject, false);
        FindObjectOfType<PlayerHandler>().WeaponBroken();
        foreach (WeaponSlot slot in slots)
        {
           if (slot.pluggedItem)
            {
                slot.pluggedItem.gameObject.GetComponent<Item>().Die();
            }
        }
        this.gameObject.GetComponent<Item>().Die();
    }

    /// <summary>
    /// Update weapon stats based on attachments
    /// </summary>
    public void UpdateWeaponStats()
    {
        //Reset Initial values
        currentAugments.Clear();
        currentBarrels.Clear();
        fireRate = baseFireRate;
        spread = 1;
        burstPower = 0;

        //For each slot get properties of attachment
        foreach (var slot in slots)
        {
            if (slot.pluggedItem != null)
            {
                var augments = slot.pluggedItem.GetComponentsInChildren<WeaponAugment>();
                currentAugments.AddRange(augments);

                var barrels = slot.pluggedItem.GetComponentsInChildren<WeaponBarrel>();
                currentBarrels.AddRange(barrels);
            }
        }

        //Apply augmentations
        foreach (var augment in currentAugments)
        {
            fireRate *= augment.fireRateMultiplier;
            spread += augment.spread;
            burstPower += augment.burstPower;
        }
    }

    public IEnumerable<WeaponSlot> Slots => slots;
}
