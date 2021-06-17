using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotType
{
    OmniSlot = 0,
    Augment,
    Barrel,
}

/// <summary>
/// Defines part of an item that can act as a slot
/// </summary>
public class WeaponSlot : MonoBehaviour
{
    [HideInInspector]
    public WeaponPluggable pluggedItem;

    public SlotType slotType;

    [HideInInspector]
    public WeaponScaffold frame;

    // Start is called before the first frame update
    void Start()
    {
        frame = GetComponentInParent<WeaponScaffold>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
