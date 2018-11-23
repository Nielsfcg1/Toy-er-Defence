using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Parent : MonoBehaviour {

    //current state. Decides what the AI is currently doing
    public enum AIState { idle, patrolling, chasingKid, mopping, chasingKidAngrily };
    public AIState state;

    //square radius from 0,0,0 where the parents can walk (only in a square map)
    public float mapSize;

    //distance from targets until a new target is chosen
    public float minTargetDistance;

    //distance from child until the aprent starts hitting
    public float childHitDistance;

    //controller for pathfinding, set targets here
    NavMeshUser agent;

    public SpriteRenderer spriteRenderer;

    //timer for the hits on the player
    Coroutine playerHitTimerRoutine;
    Coroutine calmDownRoutine;

    //pool to be mopped
    Moppability poolToMop;
    public GameObject mop;
    float mopTimer;
    float previousMopTimer;
    bool isMopping;
    float mopSpeed = 15;

    public GameObject exclamation;

    //hit timer
    public float playerHitTimerTime;
    float playerHitTimer;
    //--

    //damage the parent does
    public float damage;

    public float movementSpeed;
    float initSpeed;

    void Start() {
        agent = GetComponent<NavMeshUser>();
        state = AIState.idle;

        initSpeed = movementSpeed;
        GetComponent<NavMeshAgent>().speed = initSpeed;
    }

    void Update() {
        if (state == AIState.idle) {
            FindPatrollPosition();
            mop.SetActive(false);
            exclamation.SetActive(false);
        }

        if (state == AIState.patrolling) {
            //id reached the target position
            if (Vector3.Distance(transform.position, agent.target) < minTargetDistance) {
                state = AIState.idle;
            }
        }

        // Mop pool, added by Oane, unless player is angry
        if (state == AIState.chasingKidAngrily) {
            mop.SetActive(false);
            exclamation.SetActive(true);

            agent.SetTarget(PlayerChar.Instance.transform.position);

            GetComponent<NavMeshAgent>().speed = initSpeed * 2;

            //start the hit timer if the parent is close to the player
            if (Vector3.Distance(transform.position, PlayerChar.Instance.transform.position) < childHitDistance) {
                HitPlayer();
                /*if (playerHitTimerRoutine == null) {
                    playerHitTimerRoutine = StartCoroutine(PlayerHitTimer());
                }*/
            }
        }
        else if (state == AIState.mopping) {
            if (poolToMop != null) {
                agent.SetTarget(poolToMop.transform.position);

                poolToMop.mopProgress += Time.deltaTime;
                if (poolToMop.mopProgress > poolToMop.mopFinishedAt) {
                    Destroy(poolToMop.gameObject);
                    poolToMop = null;
                    state = AIState.idle;
                    mop.SetActive(false);
                }
            }
            else {
                poolToMop = null;
                state = AIState.idle;
                mop.SetActive(false);
            }

        }
        //chase the child if it has the gun equiped. Oane: But not if the parent is mopping.
        else if (PlayerChar.Instance.gunEquipped) {
            state = AIState.chasingKid;
        }

        if (state == AIState.chasingKid) {
            agent.SetTarget(PlayerChar.Instance.transform.position);

            //start the hit timer if the parent is close to the player
            if (Vector3.Distance(transform.position, PlayerChar.Instance.transform.position) < childHitDistance) {
                /*if (playerHitTimerRoutine == null) {
                    playerHitTimerRoutine = StartCoroutine(PlayerHitTimer());
                }*/
                HitPlayer();
            }

            if (!PlayerChar.Instance.gunEquipped) {
                state = AIState.idle;

                if (playerHitTimerRoutine != null) {
                    StopCoroutine(playerHitTimerRoutine);
                    playerHitTimerRoutine = null;
                }
            }
        }

        spriteRenderer.sortingOrder = (int)(-transform.position.z * 100);
    }

    //Parent has walked over a pool of blood or a candy, and will clean it up
    public void StartMopping(Moppability pool) {
        if (state != AIState.chasingKidAngrily && state != AIState.chasingKid) {
            state = AIState.mopping;
            poolToMop = pool;
            mop.SetActive(true);
            if (!isMopping) {
                StartCoroutine(MopBackAndForth());
            }
        }
    }

    public void BecomeAngry() {
        state = AIState.chasingKidAngrily;
        if (calmDownRoutine == null) {
            calmDownRoutine = StartCoroutine(CalmDown());
        }
    }

    //select a new position on the map
    void FindPatrollPosition() {
        Vector3 randPos = new Vector3(Random.Range(-mapSize, mapSize), transform.position.y, Random.Range(-mapSize, mapSize));
        agent.SetTarget(randPos);
        state = AIState.patrolling;
    }

    public void CalmedDown() {
        state = AIState.idle;
        calmDownRoutine = null;
        exclamation.SetActive(false);

        if (playerHitTimerRoutine != null) {
            StopCoroutine(playerHitTimerRoutine);
            playerHitTimerRoutine = null;
        }
    }

    IEnumerator CalmDown() {
        yield return new WaitForSeconds(5);
        state = AIState.idle;
        calmDownRoutine = null;
        CalmedDown();
        state = AIState.idle;
        calmDownRoutine = null;
        GetComponent<NavMeshAgent>().speed = initSpeed;
    }

    void HitPlayer() {
        CalmedDown();
        PlayerChar.Instance.ApplyDamage();
    }

    /*
    IEnumerator PlayerHitTimer()
    {
        while (true)
        {

            if (playerHitTimer <= 0)
            {
                CalmedDown();
                playerHitTimer = playerHitTimerTime;
                PlayerChar.Instance.ApplyDamage();
            }

            // Oane toevoeging: Als ze een mep gegeven hebben, houden ze op met meppen.
            // Oane: Dus nu defeat dit het punt van het hebben van een coroutine. 
            // Oane: Maar misschien ook niet, not sure, dus ik laat de coroutine er maar in voor de zekerheid
            // Het is alleen ook echt fucking glitchy, ouders doen double damage de eerste keer en whatnot 
            playerHitTimer -= Time.deltaTime;

            yield return null;
        }
    }
    */

    IEnumerator MopBackAndForth() {
        isMopping = true;
        while (state == AIState.mopping) {
            mop.transform.Rotate(0, 0, (Mathf.Sin(mopTimer * mopSpeed) - Mathf.Sin(previousMopTimer * mopSpeed)) * 20);
            previousMopTimer = mopTimer;
            mopTimer += Time.deltaTime;
            yield return null;
        }
        isMopping = false;
    }
}