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
        var gameObject = collision.gameObject;
        if (gameObject.CompareTag("Critter"))
        {
            var otherController = gameObject.GetComponent<CritterController>();

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