using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {

    //current state. Decides what the AI is currently doing
    public enum AIState { idle, moveToObject, moveToExit, getTeddyBear, hasTeddyBear, dying, walkRandomly }
    public AIState state;
    
    //object selected by the mover to grab
    public GameObject selectedObject;

    //has the mover picked up the selected object?
    public bool pickedUpObject;

    //min distance to an object before the mover can grab it
    public float pickupDistance;

    //drop off point 
    public Transform exit;

    //controller for pathfinding, set targets here
    NavMeshUser agent;

    //controller for subscribing and unsubscribing objects to move
    MoverController controller;

    [Header("Mover Variables")]
    int strength;
    float origWalkingSpeed;
    public float walkingFactor;
    public float walkingSpeed;
    public float strengthAdjustment;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer carryObjectRenderer;

    public GameObject dropObjectReminder;

    GameManager gm;
    Coroutine walkToTeddyRoutine;

    void Start()
    {
        controller = MoverController.Instance;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        agent = GetComponent<NavMeshUser>();

        agent.SetTarget(exit.position);
        state = AIState.idle;

        origWalkingSpeed = walkingSpeed * walkingFactor;
    }

    void Update()
    {
        if (state == AIState.idle)
        {
            GetComponent<NavMeshAgent>().speed = origWalkingSpeed;

            //try to select an object
            if (SelectNewObject())
            {
                state = AIState.moveToObject;
            }
            else
            {
                if (SelectTeddyBear())
                {
                    state = AIState.getTeddyBear;
                }
                else
                {
                    state = AIState.walkRandomly;
                }
            }
        }

        //check if the mover is close enough to an object to pick it up
        if (state == AIState.moveToObject)
        {
            if (Vector3.Distance(transform.position, selectedObject.transform.position) < pickupDistance)
            {
                //Debug.Log(selectedObject);
                dropObjectReminder = Instantiate(selectedObject);
                dropObjectReminder.transform.SetParent(transform);
                dropObjectReminder.SetActive(false);

                controller.placedObjects.Remove(selectedObject);
                controller.UnsubscribeObject(selectedObject);
                
                //set the sprite of the picked up object to the carry object sprite renderer
                carryObjectRenderer.sprite = selectedObject.GetComponent<Collectible>().spriteRenderer.sprite;
                Destroy(selectedObject);
                
                pickedUpObject = true;
                agent.SetTarget(exit.position);
                state = AIState.moveToExit;

                GetComponent<NavMeshAgent>().speed = (((origWalkingSpeed / walkingFactor)) - (selectedObject.GetComponent<Collectible>().weight / strengthAdjustment)) * walkingFactor;
            }
        }

        //check if the mover is close enough to the exit to drop off 
        if (state == AIState.moveToExit)
        {
            if (pickedUpObject && Vector3.Distance(transform.position, exit.position) < pickupDistance * 3)
            {
                pickedUpObject = false;

                /*SpriteRenderer droppedObjectRenderer = Instantiate(PrefabController.Instance.droppedExitObject).GetComponent<SpriteRenderer>();
                droppedObjectRenderer.sprite = carryObjectRenderer.sprite;
                droppedObjectRenderer.transform.position = exit.transform.position + new Vector3(Random.Range(-1, 1), 0.5f, Random.Range(-1, 1));
                droppedObjectRenderer.transform.localScale = transform.localScale;*/

                Destroy(dropObjectReminder);
                dropObjectReminder = null;

                carryObjectRenderer.sprite = null;
                state = AIState.idle;
            }
        }

        if (state == AIState.getTeddyBear)
        {
            if (controller.teddyBear != null)
            {
                controller.teddyBear.GetComponent<TeddyBear>().SetAnimClose();
            }

            if (Vector3.Distance(transform.position, controller.teddyBear.transform.position) < pickupDistance)
            {
                controller.teddyBear.transform.SetParent(transform);
                agent.SetTarget(exit.transform.position);
                pickedUpObject = true;
                GetComponent<NavMeshAgent>().speed = (((origWalkingSpeed / walkingFactor)) - controller.teddyBear.GetComponent<TeddyBear>().weight) * walkingFactor;
                state = AIState.hasTeddyBear;
            }
        }

        if (state == AIState.hasTeddyBear)
        {

            if (controller.teddyBear != null)
            {
                controller.teddyBear.GetComponent<TeddyBear>().SetAnimPickup();
            }

            if (pickedUpObject && Vector3.Distance(transform.position, exit.position) < pickupDistance * 3)
            {
                print("teddy bear was killed! :o");
                pickedUpObject = false;
                Destroy(controller.teddyBear.gameObject);
                
                gm.gameOverText.text = "They have kidnapped your teddybear...";
                gm.GameOver();
                //UIController.Instance.gameOverScreen.SetActive(true);
                state = AIState.idle;
            }
        }

        if (state == AIState.walkRandomly)
        {
            if (walkToTeddyRoutine == null && controller.teddyBear != null)
            {
                walkToTeddyRoutine = StartCoroutine(WaitForTeddyPos());
            }

            if (!controller.IsReserved(controller.teddyBear))
            {
                state = AIState.idle;
            }
        }

        //update the sorting layer of the sprites to get correct depth
        spriteRenderer.sortingOrder = (int)(-transform.position.z * 100);
        carryObjectRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }
    
    /// <summary>
    /// select the nearest object to grab
    /// </summary>
    /// <returns></returns>
    bool SelectNewObject()
    {
        if (controller.placedObjects.Count > 0)
        {
            float shortestDistance = float.MaxValue;

            for (int i = 0; i < controller.placedObjects.Count; i++)
            {
                if (!controller.IsReserved(controller.placedObjects[i]))
                {
                    if (Vector3.Distance(transform.position, controller.placedObjects[i].transform.position) < shortestDistance)
                    {
                        shortestDistance = Vector3.Distance(transform.position, controller.placedObjects[i].transform.position);
                        selectedObject = controller.placedObjects[i];
                    }
                }
            }

            if (selectedObject == null)
            {
                return false;
            }

            controller.SubscribeObject(selectedObject);

            agent.SetTarget(selectedObject.transform.position);

            return true;
        }

        return false;
    }

    bool SelectTeddyBear()
    {
        if (!controller.IsReserved(controller.teddyBear))
        {
            controller.reservedObjects.Add(controller.teddyBear);

            agent.SetTarget(controller.teddyBear.transform.position);

            selectedObject = controller.teddyBear;

            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDeath()
    {
        if (state == AIState.getTeddyBear || state == AIState.hasTeddyBear)
        {
            if (controller.teddyBear != null)
            {
                controller.teddyBear.transform.SetParent(null);

                controller.teddyBear.GetComponent<TeddyBear>().SetAnimIdle();

                controller.UnsubscribeObject(controller.teddyBear);
            }
        }

        if (dropObjectReminder != null)
        {
            dropObjectReminder.SetActive(true);
            dropObjectReminder.transform.SetParent(null);
            controller.placedObjects.Add(dropObjectReminder);
        }

        GetComponent<MoverFightBack>().canHitGun = false;

        carryObjectRenderer.sprite = null;
        
        MoverController.Instance.movers.Remove(this);

        state = AIState.dying;
    }

    IEnumerator WaitForTeddyPos()
    {
        if (controller.teddyBear != null)
        {
            Vector3 teddyPos = controller.teddyBear.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            agent.SetTarget(teddyPos);
        }

        yield return new WaitForSeconds(3);

        walkToTeddyRoutine = null;
    }
}