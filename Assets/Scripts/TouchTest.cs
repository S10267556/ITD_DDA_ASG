using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchTest : MonoBehaviour
{

    public GameObject hamsterInfoPopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //left click/touch on screen
        {
            Debug.Log("Screen touched");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("hit");
                Debug.Log(hit.transform.name + ":" + hit.transform.tag);

                if (hit.transform.tag == "Hamster")
                {
                    Vector3 pos = hit.point;
                    pos.y += 0.1f; //raise popup above hamster 
                    pos.z += 0.1f; //move popup forward a bit
                    Instantiate(hamsterInfoPopup, pos, transform.rotation); 
                }

                if(hit.transform.tag == "Info")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
