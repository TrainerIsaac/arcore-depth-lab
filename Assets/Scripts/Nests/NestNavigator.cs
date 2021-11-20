using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NestNavigator : MonoBehaviour
{
    public GameObject DataHolder;
    public GameObject currentNest;
    private GameObject nextNest;

    private int hasSetNextNest;

    private float currentDistance;
    private float maxDistance;

    private UnityEngine.UI.Text distanceText;
    private UnityEngine.UI.Text goalText;

    private DataHolder localDataHolder;
    private NestScript localNestScript;

    private GameObject destroyableData;

    public string environment;

    void OnEnable()
    {
        Destroy(destroyableData);
        hasSetNextNest = 0;
        gameObject.transform.position = currentNest.transform.position;

        distanceText = GameObject.Find("Distance").GetComponent<Text>();
        goalText = GameObject.Find("DistanceGoal").GetComponent<Text>();

        localNestScript = currentNest.GetComponent<NestScript>();
        localDataHolder = DataHolder.GetComponent<DataHolder>();

        maxDistance = localNestScript.maxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasSetNextNest == 0)
        {
            if (currentNest.GetComponent<NestScript>().nextNest.Count <= 1)
            {
                nextNest = currentNest.GetComponent<NestScript>().nextNest[0];
                localDataHolder.monster = localNestScript.Monster;
                Debug.Log(currentNest.GetComponent<NestScript>().Monster);
            }
            else
            {
                //CREATE UI FOR NUMBER OF NESTS
                //BELOW LINE JUST SETS NEXTNEST TO FIRST ONE IN LIST!
                nextNest = currentNest.GetComponent<NestScript>().nextNest[0];
                DataHolder.GetComponent<DataHolder>().monster = currentNest.GetComponent<NestScript>().Monster;
                Debug.Log(currentNest.GetComponent<NestScript>().Monster);
            }
            hasSetNextNest = 1;
        }
        distanceText.text = currentDistance.ToString();
        goalText.text = maxDistance.ToString();

        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.E) == true)
        {
            currentDistance += 1;
        }


        if (currentDistance == maxDistance)
        {
            currentNest = nextNest;
            currentDistance = 0;
            localDataHolder.GetComponent<DataHolder>().map = transform.parent.gameObject;

            destroyableData = Instantiate(DataHolder);

            SceneManager.LoadScene("FIGHT");
            transform.parent.gameObject.SetActive(false);
        }
    }
}
