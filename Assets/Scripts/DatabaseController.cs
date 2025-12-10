using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
using System.Data.Common;
using System;
using UnityEngine.EventSystems;

public class DatabaseController : MonoBehaviour
{
    private Player newPlayer;
    public GameObject targetObject;  // The target transform for the Hamster to look at
    public GameObject HamsterObject;  // The Hamster GameObject
    private DatabaseReference db;
    public TMP_Text usernameDisplayText;
    public TMP_InputField newNameInputField;
    private int coins = 0;

    [SerializeField]
    private int affection = 0;

    public TMP_Text energyDisplayText;
    [SerializeField]
    private int energy = 10;

    [SerializeField]
    private float timePassed = 0f;

    [SerializeField]
    private int eFoodSunflowerSeeds = 1;
    private int eFoodStrawberries = 2;

    private bool foodPlaced = false;
    

    [SerializeField]
    private float idleTime = 30f; //amt of time player is allowed to idle before pet starts losing energy


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed > idleTime)
        {
            //do something
            timePassed = 0f;
            idlePet();
        } 

        if (HamsterObject != null && targetObject != null && foodPlaced == false){
            HamsterObject.transform.LookAt(targetObject.transform);
        }
    }


        public void changeName(TMP_InputField newName)
    {
        usernameDisplayText.text = newName.text;
        db.Child("players").Child(newPlayer.playerID).Child("name").SetValueAsync(newName.text);
    }

    public void noEnergy()
    {
        //disable all actions that require energy
    }

    public void earnCurrency()
    {
        timePassed = 0f;
    }


    public void idlePet()
    {
        if ((UnityEngine.Random.Range(0, 4) == 3) && energy > 0)
        {
            energy -= 1;
            energyDisplayText.text = "Energy: " + energy.ToString();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hamster"))
        {
            HamsterObject = other.gameObject;
        }
    }
}
