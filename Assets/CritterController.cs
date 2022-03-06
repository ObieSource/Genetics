using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterController : MonoBehaviour
{
    public float consumed, hydrated;
    public float consumptionLostPerSecond = 30;
    public float hydrationLostPerSecond = 30;

    void Start()
    {

    }

    void Update()
    {
        consumed -= consumptionLostPerSecond * Time.deltaTime;
        hydrated -= hydrationLostPerSecond * Time.deltaTime;

        if (consumed <= 0 || hydrated <= 0)
        {
            Die();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var otherGameObject = collision.gameObject;
        if (otherGameObject.CompareTag("Food"))
        {
            consumed += 10;
            Destroy(otherGameObject);
        }
        else if (otherGameObject.CompareTag("Water"))
        {
            hydrated += 10;
            Destroy(otherGameObject);
        }
        else if (otherGameObject.CompareTag("Critter"))
        {
            var otherController = otherGameObject.GetComponent<CritterController>();

            if (consumed > 230 && hydrated > 230 && otherController.consumed > 230 && otherController.hydrated > 230)
            {
                consumed -= 100;
                hydrated -= 100;

                otherController.consumed -= 100;
                otherController.hydrated -= 100;

                // make baby
                Instantiate(gameObject);
            }
        }
    }

    private void Die()
    {
        GameObject.Find("Manager").GetComponent<Manager>().Kill(gameObject);
    }
}