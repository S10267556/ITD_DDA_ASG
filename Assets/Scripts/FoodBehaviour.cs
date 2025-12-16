using UnityEngine;
using System.Collections;
using UnityEngine.XR.Templates.AR;

public class FoodBehaviour : MonoBehaviour
{
    [SerializeField]
    private DatabaseController databaseController;

    private ARTemplateMenuManager aRTemplateMenuManager;

    private Transform eatPostition;

    [SerializeField]
    private float eatDelay = 2f;

    [SerializeField]
    private float foodOverlapRadius = 1000f;

    void Start()
    {
        if (databaseController == null)
        {
            databaseController = FindAnyObjectByType<DatabaseController>();
        }

        if (aRTemplateMenuManager == null)
        {
            aRTemplateMenuManager = FindAnyObjectByType<ARTemplateMenuManager>();
        }

        CheckOverlap(); //check if food is near hamster
    }

    void CheckOverlap()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, foodOverlapRadius); //check the area around the food - if there are any colliders

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Hamster")) //if the collider is hamster, make it eat
            {
                hamsterEat();
                break;
            }
        }
    }

    public void hamsterEat()
    {
        StartCoroutine(hamsterEatWait());
    }

    IEnumerator hamsterEatWait()
    {
        yield return new WaitForSeconds(eatDelay);
        eatPostition = databaseController.HamsterObject.transform; //get hamster's position
        AudioSource.PlayClipAtPoint(databaseController.FoodSound, eatPostition.position);
        aRTemplateMenuManager.SetObjectToSpawn(0);
        Destroy(gameObject);
        databaseController.actionDone();
    }
}
