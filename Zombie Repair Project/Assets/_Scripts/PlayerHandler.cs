using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerHandler : MonoBehaviour
{
    public Hand rightHand;
    public Hand leftHand;

    public float hp = 1.0f;
    private float hpStart = 0f;
    public GameObject sceneManager;

    public AudioSource takeDamage;
    public AudioSource weaponBreak;
    public AudioSource clickSound;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        AliveCheck();
        takeDamage.Play();
    }

    void AliveCheck()
    {
        if (hp <= 0)
        {
            //Player Died
            Spawner spawner = sceneManager.GetComponent<Spawner>();
            //End game
            spawner.StopGame();
            //Reset Hp
            hp = hpStart;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (sceneManager == null)
        {
            sceneManager = GameObject.Find("SceneManager");
        }
        //Save hp setting
        hpStart = hp;
    }

    public IEnumerable<WeaponScaffold> CurrentWeapons()
    {
        //retun a list of all handles bieng held by the player
        return rightHand.AttachedObjects.Concat(leftHand.AttachedObjects)
            .Select(it => it.attachedObject.GetComponent<WeaponScaffold>())
            .Where(it => it != null);
    }

    public void WeaponBroken()
    {
        weaponBreak.Play();
    }

    public void WeaponAttach()
    {
        clickSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
