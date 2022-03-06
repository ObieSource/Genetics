using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterController : MonoBehaviour
{
    public int consumed, hydrated;

    void Start()
    {

    }

    void Update()
    {
        
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
}