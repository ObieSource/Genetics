using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class CritterController : MonoBehaviour
{
    public float consumed, hydrated;
    public float consumptionLostPerSecond;
    public float hydrationLostPerSecond;
    public float force;

    public GameObject target;
    private Rigidbody rb;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;

        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        consumed -= consumptionLostPerSecond * Time.deltaTime;
        hydrated -= hydrationLostPerSecond * Time.deltaTime;

        if (consumed <= 0 || hydrated <= 0)
        {
            Die();
        }

        target = ChooseTarget();

        if (target)
        {
            Vector3 forceToMove = target.transform.position - transform.position;
            forceToMove.y = 0;
            forceToMove.Normalize();
            rb.AddForce(forceToMove * force);
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

    private GameObject ChooseTarget()
    {
        if (consumed > 230 && hydrated > 230)
        {
            GameObject closestCritter = Closest(GameObject.FindGameObjectsWithTag("Critter"));
            if (closestCritter) return closestCritter;
        }

        GameObject closestFood = Closest(GameObject.FindGameObjectsWithTag("Food"));
        GameObject closestWater = Closest(GameObject.FindGameObjectsWithTag("Water"));

        if (closestFood == null) return closestWater;
        if (closestWater == null) return closestFood;

        // do whatever is the most worthwhile
        if (DistanceTo(closestFood) / consumed < DistanceTo(closestWater) / hydrated)
        {
            return closestFood;
        }
        else
        {
            return closestWater;
        }
    }

    private GameObject Closest(GameObject[] gameObjects)
    {
        GameObject closest = null;
        float closestDistance = -1;
        foreach (GameObject obj in gameObjects)
        {
            float dist = DistanceTo(obj);
            if (obj != this.gameObject && (closestDistance == -1 || dist < closestDistance))
            {
                closest = obj;
                closestDistance = dist;
            }
        }
        return closest;
    }

    private float DistanceTo(GameObject other)
    {
        return DistanceBetween(gameObject, other);
    }

    private static float DistanceBetween(GameObject o1, GameObject o2)
    {
        return Vector3.Distance(o1.transform.position, o2.transform.position);
    }

    private void Die()
    {
        GameObject.Find("Manager").GetComponent<Manager>().Kill(gameObject);
    }
}