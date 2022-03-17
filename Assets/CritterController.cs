using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class CritterController : MonoBehaviour
{
    public float consumption, hydration;
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
        consumption -= consumptionLostPerSecond * Time.deltaTime;
        hydration -= hydrationLostPerSecond * Time.deltaTime;

        if (consumption <= 0 || hydration <= 0)
        {
            Die();
        }

        target = ChooseTarget();

        if (target)
        {
            // accelerate in the direction of the target
            // (This is not a very good aiming strategy.)
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
            consumption += 10;
            Destroy(otherGameObject);
        }
        else if (otherGameObject.CompareTag("Water"))
        {
            hydration += 10;
            Destroy(otherGameObject);
        }
        else if (otherGameObject.CompareTag("Critter"))
        {
            var otherController = otherGameObject.GetComponent<CritterController>();

            if (consumption > 230 && hydration > 230 && otherController.consumption > 230 && otherController.hydration > 230)
            {
                consumption -= 100;
                hydration -= 100;

                otherController.consumption -= 100;
                otherController.hydration -= 100;

                // make baby
                Instantiate(gameObject);
            }
        }
    }

    private GameObject ChooseTarget()
    {
        if (consumption > 230 && hydration > 230)
        {
            GameObject closestCritter = Closest(GameObject.FindGameObjectsWithTag("Critter"));
            if (closestCritter) return closestCritter;
        }

        GameObject closestFood = Closest(GameObject.FindGameObjectsWithTag("Food"));
        GameObject closestWater = Closest(GameObject.FindGameObjectsWithTag("Water"));

        if (closestFood == null) return closestWater;
        if (closestWater == null) return closestFood;

        // do whatever is the most worthwhile
        if (DistanceTo(closestFood) / consumption < DistanceTo(closestWater) / hydration)
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
        // We tell the manager to kill this critter so it can replace this
        // critter with food.
        GameObject.Find("Manager").GetComponent<Manager>().Kill(gameObject);
    }
}