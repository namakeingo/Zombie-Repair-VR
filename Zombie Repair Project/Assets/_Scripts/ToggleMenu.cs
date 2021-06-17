
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Valve.Newtonsoft.Json.Serialization;
using Valve.VR;
using Valve.VR.Extras;

/// <summary>
/// Manage scene for all menu ralated things
/// </summary>
public class ToggleMenu : MonoBehaviour
{
    public SteamVR_Action_Boolean toggleMenuAction;
    public SteamVR_Input_Sources handType;
    
    public GameObject menuObject; //Parent menu game object
    public GameObject gameoverObject;
    public SteamVR_LaserPointer leftLaserPointer;
    public SteamVR_LaserPointer rightLaserPointer;
    public bool menuEnabled = false; //If true menu is enabled on start

    public float buttonScale = 0.1f; //Scale of the buttons
    public float hoverButtonScale = 0.12f; //Scale of the buttons when pointed at
    public GameObject table;
    public GameObject barrel;
    public GameObject handle;
    public GameObject magazine;

    private int timer = 0;
    private int startUpDelay = 3;
    private GameObject leftHolder;
    private GameObject rightHolder;

    // Start is called before the first frame update
    void Start()
    {
        StartingGun();
    }

    public void EnableMenu(int gameSurviveTimer = 0, int enemiesKilled = 0)
    {
        //Show nemu
        menuEnabled = true;
        menuObject.SetActive(true);

        //Show GameOver canvases
        gameoverObject.SetActive(true);
        //Update score boards
        foreach (Transform gameover in gameoverObject.transform)
        {
            foreach (Transform child in gameover.transform)
            {
                child.transform.Find("Score Text").gameObject
                .GetComponent<UnityEngine.UI.Text>().text =
                    string.Format("{0} Seconds{1}{2} Enemies Killed", (int)gameSurviveTimer, System.Environment.NewLine, enemiesKilled);
            }
        }

        //Show laser pointers
        rightLaserPointer.enabled = true;
        leftLaserPointer.enabled = true;

        //UNUSED
        if (leftHolder) leftHolder.SetActive(true);
        if (rightHolder) rightHolder.SetActive(true);

        //Restore menu starting gun
        table.SetActive(true);
        barrel.SetActive(true);
        handle.SetActive(true);
        magazine.SetActive(true);
        StartingGun();
    }

    public void DisableMenu()
    {
        //Hide all menu only objects
        menuEnabled = false;
        menuObject.SetActive(false);
        gameoverObject.SetActive(false);
        table.SetActive(false);
        menuObject.SetActive(false);
        rightLaserPointer.enabled = false;
        leftLaserPointer.enabled = false;

        //UNUSED
        if (leftHolder) leftHolder.SetActive(false);
        if (rightHolder) rightHolder.SetActive(false);
    }

    void Update()
    {
        if (timer < startUpDelay) //dealyed Start function
        {
            timer++;
            if (timer == startUpDelay)
            {
                leftHolder = leftLaserPointer.holder;
                rightHolder = rightLaserPointer.holder;

                if (menuEnabled)
                {
                    menuObject.SetActive(true);
                    rightLaserPointer.enabled = true;
                    leftLaserPointer.enabled = true;

                    //UNUSED
                    if (leftHolder) leftHolder.SetActive(true);
                    if (rightHolder) rightHolder.SetActive(true);
                }
                else
                {
                    menuObject.SetActive(false);
                    rightLaserPointer.enabled = false;
                    leftLaserPointer.enabled = false;

                    //UNUSED
                    if (leftHolder) leftHolder.SetActive(false);
                    if (rightHolder) rightHolder.SetActive(false);
                }
            }
        }

        //Debug action
        if (Input.GetKeyDown("tab"))
        {
            if (menuEnabled)
            {
               DisableMenu();
            }
            else
            {
               EnableMenu();
            }
        }
    }

    private void StartingGun()
    {
        //Add gun parts to active items
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.activeItems.Add(barrel);
        spawner.activeItems.Add(handle);
        spawner.activeItems.Add(magazine);

        //Instantiate Gun parts for next run
        barrel = Instantiate(barrel);
        barrel.SetActive(false);
        handle = Instantiate(handle);
        handle.SetActive(false);
        magazine = Instantiate(magazine);
        magazine.SetActive(false);
    }

    void Awake()
    {
        //Add events handlers to each left hand laser
        leftLaserPointer.PointerIn += PointerInside;
        leftLaserPointer.PointerOut += PointerOutside;
        leftLaserPointer.PointerClick += PointerClick;

        //Add events handlers to each right hand laser
        rightLaserPointer.PointerIn += PointerInside;
        rightLaserPointer.PointerOut += PointerOutside;
        rightLaserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "PlayButton")
        {
            //Start the game
            var spawner = FindObjectOfType<Spawner>();
            spawner.StartGame();
        }
        else if (e.target.name == "ExitButton")
        {
            //Close application
            Application.Quit();
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "PlayButton")
        {
            //Scale up button
            e.target.localScale = new Vector3(hoverButtonScale, hoverButtonScale, hoverButtonScale);
        }
        else if (e.target.name == "ExitButton")
        {
            //Scale up button
            e.target.localScale = new Vector3(hoverButtonScale, hoverButtonScale, hoverButtonScale);
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "PlayButton")
        {
            //Scale back to 1 button
            e.target.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
        }
        else if (e.target.name == "ExitButton")
        {
            //Scale back to 1 button
            e.target.localScale = new Vector3(buttonScale, buttonScale, buttonScale);
        }
    }
}