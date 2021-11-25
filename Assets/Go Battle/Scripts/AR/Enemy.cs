using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Enemy : MonoBehaviour
{
    public GameObject projectile;
    public Transform player;
    public GameObject midRange;
    public GameObject CloseRange;

    public float speed = 1;
    public int health = 500;

    public Slider mSlider;
    private GameObject localProjectile;
    public int attackStat;
    public int defenceStat;

    private bool projectileCooldown = false;
    private bool midCooldown = false;
    private bool closeCooldown = false;
    private float distance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "ProjectileB")
        {
            health -= Mathf.FloorToInt(collision.GetComponent<ProjectileScript>().totalDamage);
        }

        if(health <= 0)
        {
            print("You Win!!");
            SceneManager.LoadScene("NestMapless");
            GameObject.FindGameObjectWithTag("Map").SetActive(true);
        }
    }


    void Update()
    {
        distance = (Vector3.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("MainCamera").transform.position));
        //Debug.Log(distance);
        mSlider.value = health;

        Vector3 playerDirection = player.position - transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, playerDirection, singleStep, 0);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);

        //    if (distance > 1 && closeCooldown == false)
        //    {
        //        CloseAttack();
        //    }

        //    if (distance > 2 && midCooldown == false)
        //    {
        //        MidRangeAttack();
        //    }

        //    else
        //    {
        //        if (projectileCooldown == false)
        //        {
        //            localProjectile = Instantiate(projectile, this.transform.position, Quaternion.LookRotation(playerDirection));
        //            localProjectile.GetComponent<ProjectileScript>().Origin = this.gameObject;
        //            projectileCooldown = true;
        //            StartCoroutine(Cooldown());
        //        }
        //    }
        //}

        //IEnumerator CloseAttack()
        //{
        //    GameObject close = Instantiate(CloseRange, gameObject.transform);
        //    closeCooldown = true;
        //    yield return new WaitForSeconds(2);
        //    Destroy(close);
        //    yield return new WaitForSeconds(5);
        //    closeCooldown = false;
        //}

        //IEnumerator MidRangeAttack()
        //{
        //    Vector3 wario = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        //    if (GameObject.FindGameObjectsWithTag("GroundSpike").Length >= 2 && midCooldown == true)
        //    {

        //    }
        //    else
        //    {
        //        GameObject spike = Instantiate(midRange, (new Vector3(wario.x + Random.Range(-2f, 2f), wario.y, wario.z + Random.Range(1f, 2f))), Quaternion.Euler(0, 0, 0), null);
        //        StartCoroutine(DestroySpike());
        //        midCooldown = true;
        //        yield return new WaitForSeconds(15);
        //        midCooldown = false;
        //    }
        //}

        //void SpecialAttack()
        //{

        //}

        //private IEnumerator DestroySpike()
        //{
        //    yield return new WaitForSeconds(15);
        //    Destroy(gameObject);
        //}

        //private IEnumerator Cooldown()
        //{
        //    yield return new WaitForSeconds(2);
        //    projectileCooldown = false;
        //}
    }
}