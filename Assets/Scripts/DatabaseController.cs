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

    public TMP_InputField signUpUsernameInput;
    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPasswordInput;

    public TMP_InputField signInEmailInput;
    public TMP_InputField signInPasswordInput;

    public GameObject gameScreen;
    public GameObject sleepScreen;
    public GameObject endScreen;
    public GameObject signInScreen;
    public GameObject signUpScreen;

    private DatabaseReference db;

    [SerializeField] 
    private FoodSpawnController foodBehaviour;

    
    public GameObject targetObject;  // The target transform for the Hamster to look at
    public GameObject HamsterObject;  // The Hamster GameObject

    public TMP_Text usernameDisplayText;
    public TMP_InputField newNameInputField;

   
    public AudioClip toxicFoodSound; 
   
    public AudioClip FoodSound; 

    [SerializeField]
    private TMP_Text coinsDisplayText;
    public int coins = 0;

    [SerializeField]
    private int affection = 0;

    [SerializeField]
    private TMP_Text affectionDisplayText;

    [SerializeField]
    public TMP_Text energyDisplayText;

    public int energy = 10;
    public int days = 0;
    [SerializeField]
    private TMP_Text daysDisplayText;

    [SerializeField]
    private float timePassed = 0f;

    public bool foodPlaced = false;

    //all food fields
    public bool spawnFood = false;

    [SerializeField]
    private TMP_Text almondAmtText;
    public int almondAmt = 0;
    public int almondCost = 3;

    [SerializeField]
    private TMP_Text broccoliAmtText;
    public int broccoliAmt = 0;
    public int broccoliCost = 5;

    [SerializeField]
    private TMP_Text caffeineAmtText;
    public int caffeineAmt = 0;
    public int caffeineCost = 2;

    [SerializeField]
    private TMP_Text carrotAmtText;
    public int carrotAmt = 0;
    public int carrotCost = 4;

    [SerializeField]
    private TMP_Text onionAmtText;
    public int onionAmt = 0;
    public int onionCost = 9;

    [SerializeField]
    private TMP_Text strawberryAmtText;
    public int strawberryAmt = 0;
    public int strawberryCost = 10;

    [SerializeField]
    private TMP_Text sunflowerSeedsAmtText;
    public int sunflowerSeedsAmt = 0;
    public int sunflowerSeedsCost = 8;


    [SerializeField]
    private float idleTime = 30f; //amt of time player is allowed to idle before pet starts losing energy


    [SerializeField]
    private TMP_Text quizQn;

    [SerializeField]
    private TMP_Text quizOp1;
    [SerializeField]
    private TMP_Text quizOp2;
    [SerializeField]
    private TMP_Text quizOp3;
    [SerializeField]
    private TMP_Text quizOp4;

    private string quizAns;

    private string bufferedQuestion;
    private string[] bufferedOptions;
    private string bufferedAnswer;
    private bool quizReady = false; //to load quiz once ready - error when trying to load with firebase


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

        if (quizReady)
        {
            quizQn.text = bufferedQuestion;
            quizOp1.text = bufferedOptions[0];
            quizOp2.text = bufferedOptions[1];
            quizOp3.text = bufferedOptions[2];
            quizOp4.text = bufferedOptions[3];

            quizAns = bufferedAnswer;
            quizReady = false;
        }
    }


    public void Signup()
    {
        if (signUpEmailInput == null || signUpPasswordInput == null || signUpUsernameInput == null)
        {
            //signUpErrorText.text = "Please ensure all fields are filled out!";
            return;
        }

        var signUpTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(signUpEmailInput.text, signUpPasswordInput.text);
        signUpTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                if(task.Exception != null)
                {
                    var exception = task.Exception.GetBaseException() as FirebaseException;
                    var errorCode = (AuthError)exception.ErrorCode;
                    Debug.Log(errorCode);
                }
            }

            else if (task.IsCompleted)
            {
                signInScreen.SetActive(true);
                signUpScreen.SetActive(false);
                var uid = task.Result.User.UserId;
                Debug.Log($"User signed up, user ID is: {uid}");

                var newPlayer = new Player(uid, signUpUsernameInput.text);
                var db = FirebaseDatabase.DefaultInstance.RootReference;
                string newPlayerJson = JsonUtility.ToJson(newPlayer);
                db.Child("players").Child(newPlayer.playerID).SetRawJsonValueAsync(newPlayerJson);
            }
        });
    }

    public void SignIn()
    {
        if (signInEmailInput == null || signInPasswordInput == null)
        {
            //signInErrorText.text = "Please ensure all fields are filled out!";
            return;
        }

        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(signInEmailInput.text, signInPasswordInput.text);
        signInTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //signInErrorText.text = "Can't sign in user!";
                Debug.Log("Can't sign in user!");
                return;
            }

            if (task.IsCompleted)
            {
                newPlayer = new Player(task.Result.User.UserId, ""); //temporary name, will be updated when retrieving data
                gameScreen.SetActive(true);
                signInScreen.SetActive(false);
                var uid = task.Result.User.UserId;
                Debug.Log($"User signed in, user ID is: {uid}");
            }
        });
    }



        public void changeName(TMP_InputField newName)
    {
        usernameDisplayText.text = newName.text;
        db.Child("players").Child(newPlayer.playerID).Child("name").SetValueAsync(newName.text);
    }

    public void noEnergy()
    {
        //disable all actions that require energy
        gameScreen.SetActive(false);
        sleepScreen.SetActive(true);
        days += 1;
        if(days == 5)
        {
            endGame();
        }
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
            updateEnergy();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hamster"))
        {
            HamsterObject = other.gameObject;
        }
    }

    public void foodAmount()
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

                almondAmt = player.food.almonds;
                almondAmtText.text = "Use (" + almondAmt.ToString() + ")";
                
                broccoliAmt = player.food.broccoli;
                broccoliAmtText.text = "Use (" + broccoliAmt.ToString() + ")";

                caffeineAmt = player.food.caffeine;
                caffeineAmtText.text = "Use (" + caffeineAmt.ToString() + ")";
                
                carrotAmt = player.food.carrots;
                carrotAmtText.text = "Use (" + carrotAmt.ToString() + ")";
                
                onionAmt = player.food.onions;
                onionAmtText.text = "Use (" + onionAmt.ToString() + ")";
                
                strawberryAmt = player.food.strawberries;
                strawberryAmtText.text = "Use (" + strawberryAmt.ToString() + ")";

                sunflowerSeedsAmt = player.food.sunflowerSeeds;
                sunflowerSeedsAmtText.text = "Use (" + sunflowerSeedsAmt.ToString() + ")";
            }
        });
    }

    public void updateFood()
    {
        if (foodBehaviour.spawnedFood.CompareTag("Almond"))
        {
            db.Child("players").Child(newPlayer.playerID).Child("food").Child("almonds").SetValueAsync(almondAmt-1);
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins-almondCost);
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection+3);
        }
        if (foodBehaviour.spawnedFood.CompareTag("Broccoli"))
        {
            db.Child("players").Child(newPlayer.playerID).Child("food").Child("broccoli").SetValueAsync(broccoliAmt-1);
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins-broccoliCost);
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection+3);
        }
        if (foodBehaviour.spawnedFood.CompareTag("Caffeine"))
        {
            db.Child("players").Child(newPlayer.playerID).Child("food").Child("caffeine").SetValueAsync(caffeineAmt-1);
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins-caffeineCost);
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection+3);
        }
        if (foodBehaviour.spawnedFood.CompareTag("Carrot"))
        {
            db.Child("players").Child(newPlayer.playerID).Child("food").Child("carrot").SetValueAsync(carrotAmt-1);
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins-carrotCost);
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection+3);
        }
        if (foodBehaviour.spawnedFood.CompareTag("Onion"))
        {
            db.Child("players").Child(newPlayer.playerID).Child("food").Child("onion").SetValueAsync(onionAmt-1);
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins-onionCost);
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection+3);
        }
        if (foodBehaviour.spawnedFood.CompareTag("Strawberry"))
        {
            db.Child("players").Child(newPlayer.playerID).Child("food").Child("strawberries").SetValueAsync(strawberryAmt-1);
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins-strawberryCost);
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection+3);
        }
        if (foodBehaviour.spawnedFood.CompareTag("SunflowerSeeds"))
        {
            db.Child("players").Child(newPlayer.playerID).Child("food").Child("sunflowerSeeds").SetValueAsync(sunflowerSeedsAmt-1);
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins-sunflowerSeedsCost);
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection+3);
        }       
    }

    public void eatingFood()
    {
        
    }

    public void buyFood()
    {
        
    
    }

    public void quizQuestions()
    {
        var db = FirebaseDatabase.DefaultInstance.RootReference;

        db.Child("QuizList").GetValueAsync().ContinueWithOnMainThread(task =>
{
        if (task.IsFaulted || task.IsCanceled)
            return;

        int questionNum = UnityEngine.Random.Range(0, 5);
        int index = 0;

        foreach (var child in task.Result.Children)
        {
            if (index == questionNum)
            {
                bufferedQuestion = child.Child("Question").Value.ToString();

                bufferedOptions = new string[4];
                int i = 0;
                foreach (var opt in child.Child("Option").Children)
                {
                    bufferedOptions[i++] = opt.Value.ToString();
                }

                bufferedAnswer = child.Child("Answer").Value.ToString();
                quizReady = true;
                break;
            }
            index++;
        }
    });
    }

    public void checkAnswer(TMP_Text buttonText)
    {
        if(buttonText.text == quizAns)
        {
            affection += 3;
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection);
            coins += 2;
            db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins);
            energy -= 1;
            quizQuestions();
            updateEnergy();
        }
        else
        {
            affection -= 1;
            db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection);
            energy -= 1;
            quizQuestions();
            updateEnergy();
        }
    }

    public void updateAffection()
    {
        affectionDisplayText.text = affection.ToString();
    }

    public void updateEnergy()
    {
        energyDisplayText.text = energy.ToString();
    }

    public void updateCoins()
    {
        coinsDisplayText.text = coins.ToString();
    }

    public void endGame()
    {
        gameScreen.SetActive(false);
        endScreen.SetActive(true);
    }
}

