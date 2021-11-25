using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MapScript : MonoBehaviour
{
    public GameObject Nest;
    private GameObject LocalNest;

    public string[][][] environments = new string[][][] {

    new string[][] { new string[] { "pidgeon", "Gull", "Eagle", "plane", "Hot Air Balloon", "Blimp" }, new string[] { "god", "Jesus" } }, //Sky World
    new string[][] { new string[] { "Rock Man", "Lava Blob", "Fire Bat", "Burning Bug", "Diamond Dude", "Demon" }, new string[] { "Devil", "Satan" } }, //Under Ground
    new string[][] { new string[] { "fly", "spider", "dragonfly", "Leaf Bug", "Tarantula", "Ogre" }, new string[] { "Shrek", "Donkey" } } //Forest
    
    }; //An array of different envionments. Each environment contains both an array of standard encounters, and an array of bosses.

    //Encounters are to be replaced with actual monsters and environments - to be planned later.

    private List<int> monsterOdds = new List<int>() { 0, 0, 0, 1, 1, 1, 2, 2, 3, 3, 4, 4, 5 }; //Generates odds for selecting a monster from an environment - 
    //as each monster list goes up in rarity, the odds of selecting a higher number is low, making rarer monsters less likely to appear.

    private int selectedMonsterOdd; //The selected number from the above array

    private int[] environmentOdds = new int[] { 0, 0, 0, 1, 1, 1, 2, 2 }; //The same as the monster odds, but for environments. 
    private int selectedEnvironmentOdd;

    private float nestCount; //The number of nests for a map - is a float as the number is randomly generated then rounded up
    public List<string> monsterList = new List<string>();
    private List<int> usedMonsterOdds = new List<int>();
    private float difficulty; //Each map has a difficulty that will be displayed on the front of each map.
    private int roundedNestCount; //The rounded up nest value, and the final number of nests to be created in a map.

    private float notRoundedDistance; //The number of KM between each map - to be stored in a nest. Also randomly generated and needs to be rounded up
    public List<int> distances = new List<int>(); //Said rounded up KM number.
    private Vector3 oldDistance;

    private int counter;
    private List<GameObject> nestList = new List<GameObject>();
    private List<GameObject> hasBranch = new List<GameObject>();
    private List<GameObject> isBranch = new List<GameObject>();
    private GameObject randomNest;

    private int branchCounter;
    private float branchTotal;

    private List<Vector3> placedNests = new List<Vector3>();

    private Vector3 test;
    private float rangeMultiplyer = 1;
    private int randomSide;
    private GameObject oldNest;
    private GameObject tempOldNest;

    private List<GameObject> children = new List<GameObject>();
    private int childCounter = 0;

    //This will be in charge of generating the nest's theme
    //The theme will contain a pool of monsters that it can select from
    //Nest Generation will also be done here

    private void Start()
    {

        DontDestroyOnLoad(transform.gameObject);

        selectedEnvironmentOdd = environmentOdds[Random.Range(0, environmentOdds.Length)]; //Generates the environment for the map

        difficulty = Random.Range(1f, 2f);
        nestCount = Random.Range(5f, 6.5f) * difficulty;
        roundedNestCount = (Mathf.RoundToInt(nestCount));
        branchTotal = Random.Range(4f, 5f) * difficulty;

        for (int i = 0; i < roundedNestCount; i++) //For every nest, generate:
        {

            if (monsterOdds.Count != 0)
            {
                selectedMonsterOdd = monsterOdds[Random.Range(0, monsterOdds.Count)]; //its own monster, taken from the array of monsters for the selected environment
                monsterOdds.Remove(selectedMonsterOdd);
                usedMonsterOdds.Add(selectedMonsterOdd);
            }

            else
            {
                selectedMonsterOdd = usedMonsterOdds[Random.Range(0, usedMonsterOdds.Count)]; //If the used monsters list is empty, re-use previous ones
            }

            notRoundedDistance = ((Random.Range(1.0f, 1.2f) * (selectedMonsterOdd + 1) / (Random.Range(2.0f, 2.2f)) * difficulty) * 2); //a distance to the next nest
            LocalNest = Nest;

            LocalNest.GetComponent<NestScript>().Monster = environments[selectedEnvironmentOdd][0][selectedMonsterOdd];
            LocalNest.GetComponent<NestScript>().maxDistance = Mathf.RoundToInt(notRoundedDistance);
            LocalNest.GetComponent<NestScript>().oldPos = oldDistance;

            if (counter == 0)
            {
                LocalNest.name = "Nest " + counter.ToString();
                LocalNest.GetComponent<NestScript>().order = counter;
                oldNest = Instantiate(LocalNest, oldDistance = new Vector3(0, -210, 0), Quaternion.identity, this.gameObject.transform);
                placedNests.Add(oldDistance);
                GameObject.FindGameObjectWithTag("Player").GetComponent<NestNavigator>().currentNest = oldNest;
                GameObject.FindGameObjectWithTag("Player").GetComponent<NestNavigator>().enabled = true;
                counter += 1;
            }

            else
            {
                LocalNest.name = "Nest " + counter.ToString();
                LocalNest.GetComponent<NestScript>().order = counter;
                nestList.Add(Instantiate(LocalNest, oldDistance = new Vector3(Random.Range(-25f, 25f), oldDistance.y + (Mathf.RoundToInt(notRoundedDistance) * 100) / (nestCount * 6) + 20, 0), Quaternion.identity, this.gameObject.transform));
                placedNests.Add(oldDistance);
                oldNest.GetComponent<NestScript>().nextNest.Add(nestList[counter - 1]);
                oldNest.GetComponent<NestScript>().nextNest.RemoveAt(0);
                oldNest = nestList[counter - 1];
                counter += 1;
            }
        }
        //foreach (var x in nestList)
        //{
        //    Debug.Log(x.ToString());
        //}

        for (int k = 0; k < Mathf.RoundToInt(branchTotal); k++)
        {
            randomSide = Random.Range(0, 2);
            GetUnusedNest();
            oldDistance = randomNest.transform.position;
            oldNest = randomNest;
            for (int i = 0; i < (nestList.Count - (nestList.IndexOf(randomNest))); i++)
            {
                if (monsterOdds.Count != 0)
                {
                    selectedMonsterOdd = monsterOdds[Random.Range(0, monsterOdds.Count)]; //its own monster, taken from the array of monsters for the selected environment
                    monsterOdds.Remove(selectedMonsterOdd);
                    usedMonsterOdds.Add(selectedMonsterOdd);
                }

                else
                {
                    selectedMonsterOdd = usedMonsterOdds[Random.Range(0, usedMonsterOdds.Count)]; //If the used monsters list is empty, re-use previous ones
                }

                notRoundedDistance = (Random.Range(1.0f, 2.0f) * (selectedMonsterOdd + 1) / (Random.Range(2.0f, 3.0f)) * difficulty); //a distance to the next nest

                LocalNest = Nest;


                LocalNest.GetComponent<NestScript>().Monster = environments[selectedEnvironmentOdd][0][selectedMonsterOdd];
                LocalNest.GetComponent<NestScript>().maxDistance = Mathf.RoundToInt(notRoundedDistance);
                LocalNest.GetComponent<NestScript>().oldPos = oldDistance;
                LocalNest.name = randomNest.name + " branch " + branchCounter;
                LocalNest.name = LocalNest.name.Replace("(Clone)", "").Trim();
                LocalNest.GetComponent<NestScript>().order = nestList.IndexOf(randomNest) + branchCounter + 2;
                GetUnnocupiedSpace();

                counter += 1;
                branchCounter += 1;
            }
            isBranch.Clear();
            hasBranch.Add(randomNest);
            branchCounter = 0;
        }
        //Removes null nextnest from the beginning of the last nest's next nest list.
        nestList[nestList.Count - 1].GetComponent<NestScript>().nextNest.RemoveAt(0);

        //Sets the LocalNest value to the boss of whichever environment is chosen, this being signified by the [1], rather than the usual [0]
        LocalNest.GetComponent<NestScript>().Monster = environments[selectedEnvironmentOdd][1][Random.Range(1,2)];
        LocalNest.GetComponent<NestScript>().maxDistance = Mathf.RoundToInt(notRoundedDistance);
        LocalNest.GetComponent<NestScript>().oldPos = oldDistance;
        LocalNest.name = "BOSS";
        
        // finds every object with the NestScript - essentially finding every nest
        NestScript[] withScript = GameObject.FindObjectsOfType<NestScript>();
        float highestY = 0;

        //For each nest
        foreach (NestScript script in withScript)
        {
            //If the order value is equal to the nest count - essentially meaning if the nest is the last one in its branch
            if(script.GetComponent<NestScript>().order == roundedNestCount)
            {
                //if the Y value of this nest is higher then highest Y
                if(script.transform.position.y > highestY)
                {
                    //set highest Y to current nest value. This ensures that the highestY value is equal to the nest's Y value that's the largest of them all
                    highestY = script.transform.position.y;
                }
                //Creates a child to bare a linerender that connects from each final nest to the boss.
                //Since each gameobject can only have one line renderer, creating a new object for each linerenderer is unfortunately necessary.
                GameObject lineChild = new GameObject("LineChild" + childCounter.ToString());
                childCounter += 1;

                lineChild.AddComponent<LineRenderer>();
                //Sets line's first point to the current nest
                lineChild.GetComponent<LineRenderer>().SetPosition(0, script.transform.position);

                lineChild.transform.parent = transform;
                children.Add(lineChild);
                //Adds Boss to nextnest list of current nest
                script.GetComponent<NestScript>().nextNest.Add(LocalNest);
                //Removes null nest
                script.GetComponent<NestScript>().nextNest.RemoveAt(0);
            }

        }
        Instantiate(LocalNest, new Vector3(0, highestY + 50, 0), Quaternion.identity, this.gameObject.transform);
        //Finally sets the line's second point to the boss nest, finishing the line drawn from each final nest to the boss.
        foreach(GameObject child in children)
        {
            child.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, highestY + 50, 0));
        }
    }

    void GetUnusedNest()
    {
        if (hasBranch.Count + 1 == roundedNestCount)
        {
            hasBranch.Clear();
        }

        randomNest = nestList[Random.Range(0, nestList.Count)];
        if (hasBranch.Contains(randomNest))
        {
            GetUnusedNest();
        }
    }

    void GetUnnocupiedSpace()
    {
        print(randomSide);
        if (randomSide == 0)
        {
            test = new Vector3((Random.Range(-40f * rangeMultiplyer, -15f * rangeMultiplyer)), oldDistance.y + (Mathf.RoundToInt(notRoundedDistance) * 100) / (nestCount * 6) + 20, 0);
        }

        if (randomSide == 1)
        {
            test = new Vector3((Random.Range(15f * rangeMultiplyer, 40f * rangeMultiplyer)), oldDistance.y + (Mathf.RoundToInt(notRoundedDistance) * 100) / (nestCount * 6) + 20, 0);
        }

        bool checkResult = Physics2D.OverlapCircle(test, 9f);

        if (checkResult == false)
        {
            if (branchCounter == 0)
            {
                print("Functioning!");
                tempOldNest = Instantiate(LocalNest, oldDistance = new Vector3(test.x, test.y, 0), Quaternion.identity, randomNest.transform);
                placedNests.Add(oldDistance);
                rangeMultiplyer = 1;
                oldNest.GetComponent<NestScript>().nextNest.Add(tempOldNest);
                oldNest = tempOldNest;
            }

            else
            {
                print("Functioning!");
                isBranch.Add(Instantiate(LocalNest, oldDistance = new Vector3(test.x, test.y, 0), Quaternion.identity, randomNest.transform));
                placedNests.Add(oldDistance);
                rangeMultiplyer = 1;
                oldNest.GetComponent<NestScript>().nextNest.Add(isBranch[branchCounter - 1]);
                oldNest = isBranch[branchCounter - 1];

            }
        }
        else
        {
            rangeMultiplyer *= 1.25f;
            //print(rangeMultiplyer);
            GetUnnocupiedSpace();
        }


    }
}

//load item in environments folder of same name
//POSSIBLY Try and weight tougher monsters towards the end of the map?
//Give player the ability to traverse through a map.