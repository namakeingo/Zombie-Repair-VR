using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using UnityEngine;

/// <summary>
/// Defines a part of the weapon that can be snapped into place
/// </summary>
public class WeaponPluggable : MonoBehaviour
{
    [HideInInspector]
    public WeaponSlot slot;

    private Rigidbody rb;
    private Interactable interactable;
    private PlayerHandler player;

    public SlotType type;

    // Start is called before the first frame update
    void Start()
    {
        //Find components
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
        player = FindObjectOfType<PlayerHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (slot != null)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 0.2f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 0.2f);
        }
    }

    private void Update()
    {
        //If it is holded by user
        if (interactable?.attachedToHand)
        {
            foreach (var weapon in player.CurrentWeapons())
            {
                foreach (var slot in weapon.Slots)
                {
                    var distance = transform.position - slot.transform.position;

                    //If it is really close to an atraching point of another gun part
                    if (distance.magnitude <= 0.1f)
                    {
                        //Attempt slot
                        TryPlugInto(slot);
                    }
                }
            }
        }
    }

    public void TryPlugInto(WeaponSlot slot)
    {
        if (slot.pluggedItem) return;

        // Check if slot and plug are correct types
        if (type == SlotType.OmniSlot || slot.slotType == SlotType.OmniSlot
            || type == slot.slotType)
        {
            //Attach parts together

            FindObjectOfType<PlayerHandler>().WeaponAttach();

            this.slot = slot;
            interactable.attachedToHand.DetachObject(gameObject, false);
            transform.parent = slot.transform;
            slot.pluggedItem = this;
            if (rb != null) rb.isKinematic = true;

            slot.frame.UpdateWeaponStats();

            Destroy(GetComponent<Throwable>());
            Destroy(GetComponent<Collider>());
            Destroy(interactable);
        }
    }

    /// <summary>
    /// UNUSED
    /// </summary>
    public void Detach()
    {
        if (slot != null)
        {
            interactable.enabled = true;
            if (rb != null) rb.isKinematic = false;
            slot.pluggedItem = null;
            slot = null;
            transform.parent = null;

            slot.frame.UpdateWeaponStats();
        }
    }

    // DEBUG CODE:
    public bool testSnap;
    void OnValidate()
    {
        if (testSnap)
        {
            testSnap = false;
            var slot = GameObject.Find("Slot").GetComponent<WeaponSlot>();
            TryPlugInto(slot);
        }
    }
}
