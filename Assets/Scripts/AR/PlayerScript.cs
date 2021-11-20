using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int health = 100;
    //public Slider mSlider;
    public GameObject projectile;
    public int attackStat;
    public int defenceStat;
    private GameObject bullet;
    private bool coolDown;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "ProjectileA")
        {
            health -= Mathf.FloorToInt(collision.GetComponent<ProjectileScript>().totalDamage);
        }
    }


    void Update()
    {
        //mSlider.value = health;

        if (Input.touchCount > 0)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began && coolDown == false)
            {
                print("TEST AAAA");
                Vector3 touchPos = Camera.main.ScreenToWorldPoint((Vector3)Input.GetTouch(0).position + new Vector3(0, 0, 0.1f));
                Debug.Log(touchPos);
                Vector3 dir = touchPos - (new Vector3(transform.position.x, transform.position.y, transform.position.z));
                dir.Normalize();
                bullet = Instantiate(projectile, touchPos, Quaternion.LookRotation(dir)) as GameObject;
                bullet.GetComponent<ProjectileScript>().Origin = this.gameObject;
                coolDown = true;
                StartCoroutine(BulletCool());
            }
        }

    }

    IEnumerator BulletCool()
    {
        yield return new WaitForSeconds(1/3);
        coolDown = false;
    }
}
