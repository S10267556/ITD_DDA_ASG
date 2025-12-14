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

    private DatabaseReference db;
    
    public GameObject targetObject;  // The target transform for the Hamster to look at
    public GameObject HamsterObject;  // The Hamster GameObject
    

    public TMP_Text usernameDisplayText;
    public TMP_InputField newNameInputField;

    [SerializeField]
    private AudioClip toxicFoodSound; 
    [SerializeField]
    private AudioClip FoodSound; 

    private int coins = 0;

    [SerializeField]
    private int affection = 0;

    public TMP_Text energyDisplayText;
    [SerializeField]
    private int energy = 10;

    [SerializeField]
    private float timePassed = 0f;

    private bool foodPlaced = false;

    //all food serializable fields
    [SerializeField]
    private TMP_Text almondAmtText;
    private int almondAmt = 0;

    [SerializeField]
    private TMP_Text broccoliAmtText;
    private int broccoliAmt = 0;

    [SerializeField]
    private TMP_Text caffeineAmtText;
    private int caffeineAmt = 0;

    [SerializeField]
    private TMP_Text carrotAmtText;
    private int carrotAmt = 0;

    [SerializeField]
    private TMP_Text onionAmtText;
    private int onionAmt = 0;

    [SerializeField]
    private TMP_Text strawberryAmtText;
    private int strawberryAmt = 0;

    [SerializeField]
    private TMP_Text sunflowerSeedsAmtText;
    private int sunflowerSeedsAmt = 0;


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

    void eatFood()
    {
        
    }

    void buyFood()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var db = FirebaseDatabase.DefaultInstance.RootReference;
        var playerRetrieveTask = db.Child("players").Child(uid).GetValueAsync();
        playerRetrieveTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Error loading player!!!");
                return;
            }

            if (task.IsCompleted)
            {
                string json = task.Result.GetRawJsonValue();
                Debug.Log("Player loaded successfully!");

                Player player = JsonUtility.FromJson<Player>(json);

                //if()

                //usernameDisplayText.text = player.name;
                //itemsInInventory = player.items.Count;
                //numberOfItemsText.text = "(" + itemsInInventory.ToString() + "/4)";

                

            }
        });
    }
}
