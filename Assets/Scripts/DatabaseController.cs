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
using UnityEngine.XR.Templates.AR;
using UnityEngine.XR.ARKit;

public class DatabaseController : MonoBehaviour
{
    private Player newPlayer;

    public TMP_InputField signUpUsernameInput;
    public TMP_InputField signUpEmailInput;
    public TMP_InputField signUpPasswordInput;
    public TMP_Text signUpErrorText;

    public TMP_InputField signInEmailInput;
    public TMP_InputField signInPasswordInput;
    public TMP_Text signInErrorText;

    public GameObject gameScreen;
    public GameObject foodMenuScreen;
    public GameObject hMenuScreen;
    public GameObject minigameScreen;
    public GameObject sleepScreen;
    public GameObject endScreen;
    public GameObject signInScreen;
    public GameObject signUpScreen;

    private DatabaseReference db;

    [SerializeField] 
    private FoodSpawnController foodBehaviour;
    [SerializeField]
    private ARTemplateMenuManager aRTemplateMenuManager;

    
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
    [SerializeField]
    private TMP_Text almondCostText;
    public int almondAmt = 0;
    public int almondCost = 3;
    public int almondAff = 2;
    [SerializeField]
    private int almondsFed = 0;


    [SerializeField]
    private TMP_Text broccoliAmtText;
    [SerializeField]
    private TMP_Text broccoliCostText;
    public int broccoliAmt = 0;
    public int broccoliCost = 5;
    public int broccoliAff = 3;

    [SerializeField]
    private TMP_Text caffeineAmtText;
    [SerializeField]
    private TMP_Text caffeineCostText;
    public int caffeineAmt = 0;
    public int caffeineCost = 2;
    public int caffeineAff = 1;
    [SerializeField]
    private int caffeineFed = 0;

    [SerializeField]
    private TMP_Text carrotAmtText;
    [SerializeField]
    private TMP_Text carrotCostText;
    public int carrotAmt = 0;
    public int carrotCost = 4;
    public int carrotAff = 4;

    [SerializeField]
    private TMP_Text onionAmtText;
    [SerializeField]
    private TMP_Text onionCostText;
    public int onionAmt = 0;
    public int onionCost = 9;
    public int onionAff = -10;
    [SerializeField]
    private bool givenOnion = false;

    [SerializeField]
    private TMP_Text strawberryAmtText;
    [SerializeField]
    private TMP_Text strawberryCostText;
    public int strawberryAmt = 0;
    public int strawberryCost = 10;
    public int strawberryAff = 6;

    [SerializeField]
    private TMP_Text sunflowerSeedsAmtText;
    [SerializeField]
    private TMP_Text sunflowerSeedsCostText;
    public int sunflowerSeedsAmt = 0;
    public int sunflowerSeedsCost = 8;
    public int sunflowerSeedsAff = 5;


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

    [SerializeField]
    private GameObject correctDisplay;
    [SerializeField]
    private GameObject WrongDisplay;
    [SerializeField]
    private GameObject nextQnButton;

    [SerializeField]
    private TMP_Text causeOfDeath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        foodCosts();
        db.Child("players").Child(newPlayer.playerID).Child("newGame").SetValueAsync(false);
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
                    
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            signUpErrorText.text = "Please enter an email address.";
                            break;
                        case AuthError.MissingPassword:
                            signUpErrorText.text = "Please enter a password.";
                            break;
                        case AuthError.InvalidEmail:
                            signUpErrorText.text = "The email address is not valid.";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            signUpErrorText.text = "The email address is already in use by another account.";
                            break;
                        default:
                            signUpErrorText.text = "Can't sign up user!";
                            break;
                    }
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
            signInErrorText.text = "Please ensure all fields are filled out!";
            return;
        }

        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(signInEmailInput.text, signInPasswordInput.text);
        signInTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                signInErrorText.text = "Can't sign in user!";
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
                days = newPlayer.days;
                updateDays();
                energy = newPlayer.energy;
                foodAmount();
                coins = newPlayer.money;
                updateCoins();
                affection = newPlayer.score;
                updateAffection();
            }
        });
    }

    public void foodCosts() //set the amount for all the foods
    {
        almondCostText.text = "Buy - $" + almondCost.ToString();
        broccoliCostText.text = "Buy - $" + broccoliCost.ToString();
        caffeineCostText.text = "Buy - $" + caffeineCost.ToString();
        carrotCostText.text = "Buy - $" + carrotCost.ToString();
        onionCostText.text = "Buy - $" + onionCost.ToString();
        strawberryCostText.text = "Buy - $" + strawberryCost.ToString();
        sunflowerSeedsCostText.text = "Buy - $" + sunflowerSeedsCost.ToString();
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
        foodMenuScreen.SetActive(false);
        hMenuScreen.SetActive(false);
        minigameScreen.SetActive(false);
        sleepScreen.SetActive(false);
        sleepScreen.SetActive(true);
    }

    public void actionDone()
    {
        timePassed = 0f;
    }


    public void idlePet()
    {
        if ((UnityEngine.Random.Range(0, 4) == 3) && energy > 0)
        {
            energy -= 1;
            if (energy == 0)
            {
                noEnergy();
            }
            updateEnergy();
            actionDone();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hamster"))
        {
            HamsterObject = other.gameObject;
        }

        if (other.gameObject.CompareTag("Empty"))
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Almond") || other.gameObject.CompareTag("Broccoli") || other.gameObject.CompareTag("Caffeine") || other.gameObject.CompareTag("Carrot") || other.gameObject.CompareTag("Onion") || other.gameObject.CompareTag("Strawberry") || other.gameObject.CompareTag("SunflowerSeeds") )
        {
            aRTemplateMenuManager.SetObjectToSpawn(0);
        }
    }

    //done in use food
    /*public void CheckUserOverlap() 
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, userOverlapRadius); //check the area around the user - if there are any colliders with food

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Almond"))
            {
                almondAmt -= 1;
                energy -= 1;  
                affection += almondAff;
            }
            if (hit.CompareTag("Broccoli"))
            {
                broccoliAmt -= 1;
                energy -= 1;
                affection += broccoliAff;
            }
            if (hit.CompareTag("Caffeine"))
            {
                caffeineAmt -= 1;
                energy -= 1;
                affection += caffeineAff;
            }
            if (hit.CompareTag("Carrot"))
            {
                carrotAmt -= 1;
                energy -= 1;
                affection += carrotAff;
            }
            if (hit.CompareTag("Strawberry"))
            {
                strawberryAmt -= 1;
                energy -= 1;
                affection += strawberryAff;
            }
            if (hit.CompareTag("SunflowerSeeds"))
            {
                sunflowerSeedsAmt -= 1;
                energy -= 1;
                affection += sunflowerSeedsAff;
            }

            updateFood();
            updateEnergy();
            updateAffection();
        }
    }*/

    public void foodAmount()
    {
        almondAmtText.text = "Use (" + almondAmt.ToString() + ")";
        broccoliAmtText.text = "Use (" + broccoliAmt.ToString() + ")";
        caffeineAmtText.text = "Use (" + caffeineAmt.ToString() + ")";
        carrotAmtText.text = "Use (" + carrotAmt.ToString() + ")";
        onionAmtText.text = "Use (" + onionAmt.ToString() + ")";
        strawberryAmtText.text = "Use (" + strawberryAmt.ToString() + ")";
        sunflowerSeedsAmtText.text = "Use (" + sunflowerSeedsAmt.ToString() + ")";
    }

    public void updateFood()
    {
        db.Child("players").Child(newPlayer.playerID).Child("food").Child("almonds").SetValueAsync(almondAmt);
        db.Child("players").Child(newPlayer.playerID).Child("food").Child("broccoli").SetValueAsync(broccoliAmt);
        db.Child("players").Child(newPlayer.playerID).Child("food").Child("caffeine").SetValueAsync(caffeineAmt);
        db.Child("players").Child(newPlayer.playerID).Child("food").Child("carrots").SetValueAsync(carrotAmt);
        db.Child("players").Child(newPlayer.playerID).Child("food").Child("onions").SetValueAsync(onionAmt);
        db.Child("players").Child(newPlayer.playerID).Child("food").Child("strawberries").SetValueAsync(strawberryAmt);
        db.Child("players").Child(newPlayer.playerID).Child("food").Child("sunflowerSeeds").SetValueAsync(sunflowerSeedsAmt);
            
        foodAmount();
    }

    public void useFood(int foodnum)
    {   
        if (energy >= 1)
        {
            if(foodnum == 1 && almondAmt >= 1)
            {
                almondAmt -= 1;
                energy -= 1;
                affection += almondAff;
                almondsFed += 1;
                if(almondsFed > 1)
                {
                    endGame();
                }
                aRTemplateMenuManager.SetObjectToSpawn(foodnum);
            }
            if(foodnum == 2 && broccoliAmt >= 1)
            {
                broccoliAmt -= 1;
                energy -= 1;
                affection += broccoliAff;
                aRTemplateMenuManager.SetObjectToSpawn(foodnum);
            }
            if(foodnum == 3 && caffeineAmt >= 1)
            {
                caffeineAmt -= 1;
                energy -= 1;
                affection += caffeineAff;
                caffeineFed += 1;
                if(caffeineFed > 1)
                {
                    endGame();
                }
                aRTemplateMenuManager.SetObjectToSpawn(foodnum);
            }
            if(foodnum == 4 && carrotAmt >= 1)
            {
                carrotAmt -= 1;
                energy -= 1;
                affection += carrotAff;
                aRTemplateMenuManager.SetObjectToSpawn(foodnum);
            }
            if(foodnum == 5 && onionAmt >= 1)
            {
                onionAmt -= 1;
                energy -= 1;
                affection += onionAff;
                givenOnion = true;
                endGame();
                aRTemplateMenuManager.SetObjectToSpawn(foodnum);
            }
            if(foodnum == 6 && strawberryAmt >= 1)
            {
                strawberryAmt -= 1;
                energy -= 1;
                affection += strawberryAff;
                aRTemplateMenuManager.SetObjectToSpawn(foodnum);
            }
            if(foodnum == 7 && sunflowerSeedsAmt >= 1)
            {
                sunflowerSeedsAmt -= 1;
                energy -= 1;
                affection += sunflowerSeedsAff;
                aRTemplateMenuManager.SetObjectToSpawn(foodnum);
            }

            updateFood();
            updateEnergy();
            updateAffection();
            actionDone();
        }
    }

    public void buyFood(string foodType)
    {
        if(foodType == "Almond")
        {
            if(coins >= almondCost)
            {
                energy -= 1;
                coins -= almondCost;
                almondAmt += 1;
            }
        }

        if(foodType == "Broccoli")
        {
            if(coins >= broccoliCost)
            {
                energy -= 1;
                coins -= broccoliCost;
                broccoliAmt += 1;
            }
        }

        if(foodType == "Caffeine")
        {
            if(coins >= caffeineCost)
            {
                energy -= 1;
                coins -= caffeineCost;
                caffeineAmt += 1;
            }
        }

        if(foodType == "Carrot")
        {
            if(coins >= carrotCost)
            {
                energy -= 1;
                coins -= carrotCost;
                carrotAmt += 1;
            }
        }

        if(foodType == "Onion")
        {
            if(coins >= onionCost)
            {
                energy -= 1;
                coins -= onionCost;
                onionAmt += 1;
            }
        }

        if(foodType == "Strawberry")
        {
            if(coins >= strawberryCost)
            {
                energy -= 1;
                coins -= strawberryCost;
                strawberryAmt += 1;
            }
        }

        if(foodType == "SunflowerSeeds")
        {
            if(coins >= sunflowerSeedsCost)
            {
                energy -= 1;
                coins -= sunflowerSeedsCost;
                sunflowerSeedsAmt += 1;
            }
        }

        updateCoins();
        updateFood();
        updateEnergy();
        actionDone();
    }

    public void quizQuestions()
    {
        var db = FirebaseDatabase.DefaultInstance.RootReference;
        correctDisplay.SetActive(false);
        WrongDisplay.SetActive(false);
        nextQnButton.SetActive(false);

        db.Child("QuizList").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                return;
            }
                
            var children = new List<DataSnapshot>(task.Result.Children);

            int currentQuestion = UnityEngine.Random.Range(0, children.Count);
            DataSnapshot selected = children[currentQuestion];

            quizQn.text = selected.Child("Question").Value.ToString();

            quizOp1.text = selected.Child("Options").Child("0").Value.ToString();
            quizOp2.text = selected.Child("Options").Child("1").Value.ToString();
            quizOp3.text = selected.Child("Options").Child("2").Value.ToString();
            quizOp4.text = selected.Child("Options").Child("3").Value.ToString();

            quizAns = selected.Child("Answer").Value.ToString();
        });
    }

    public void checkAnswer(TMP_Text buttonText)
    {
        if(buttonText.text == quizAns)
        {
            affection += 3;
            coins += 2;
            energy -= 1;
            correctDisplay.SetActive(true);
        }
        else
        {
            affection -= 1;
            energy -= 1;
            WrongDisplay.SetActive(true);
        }

        nextQnButton.SetActive(true);
        updateEnergy();
        actionDone();
        updateCoins();
        updateAffection();
    }

    public void nextQn()
    {
        quizQuestions();
    }

    public void updateAffection()
    {
        affectionDisplayText.text = affection.ToString();
        db.Child("players").Child(newPlayer.playerID).Child("score").SetValueAsync(affection);
        if(affection <= -5)
        {
            endGame();
        }
    }

    public void updateEnergy()
    {
        energyDisplayText.text = energy.ToString();
        db.Child("players").Child(newPlayer.playerID).Child("energy").SetValueAsync(energy);
        if(energy <= 0)
        {
            noEnergy();
        }
    }

    public void updateCoins()
    {
        coinsDisplayText.text = coins.ToString();
        db.Child("players").Child(newPlayer.playerID).Child("money").SetValueAsync(coins);
    }

    public void updateDays()
    {
        daysDisplayText.text = days.ToString();
        db.Child("players").Child(newPlayer.playerID).Child("days").SetValueAsync(days);
    }

    public void sleep()
    {
        days += 1;
        energy = 10;
        affection += 1;

        if(days == 5)
        {
            endGame();
        }

        updateDays();
        updateEnergy();
        updateAffection();
        updateDays();
        gameScreen.SetActive(true);
        hMenuScreen.SetActive(true);
        sleepScreen.SetActive(false);
    }

    public void endGame()
    {
        gameScreen.SetActive(false);
        endScreen.SetActive(true);
        if(almondsFed > 1)
        {
           causeOfDeath.text = "One too many Almonds.";
        }
        else if (caffeineFed > 1)
        {
            causeOfDeath.text = "Coffee? Seriously?";
        }
        else if (givenOnion)
        {
            causeOfDeath.text = "Onion.";
        }
        else if(affection <= -5)
        {
            causeOfDeath.text = "Feeling unloved, it ran away.";
        }
        else if(days >= 5)
        {
            causeOfDeath.text = "Your pet's love for you took it to greater heights!";
        }
    }

    public void openURL()
    {
        Application.OpenURL("https://dda-itd-asg.web.app");
    }
}

