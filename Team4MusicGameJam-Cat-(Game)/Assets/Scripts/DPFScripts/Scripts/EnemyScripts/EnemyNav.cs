using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol,
    PlayerSighted,
    PlayerAttack
}

public class EnemyNav : MonoBehaviour
{

    public Transform[] patrolPoints;
    private NavMeshAgent agent;
    private int currentDestination;
    private int nextLocation;
    private GameObject player;

    public float distanceReachedThreshold;
    public float playerChaseThreshold;
    public float attackRange;
    public float sightRange;
    public float sightRangeCrouched;
    public float sightRangeStanding;

    public EnemyState enemyState;
    private Vector3 playerSightedPosition;

    public EnemyCombat enemyCombat;
    public GameObject lookAtObject;

    private CharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        //Assigning the nav Mesh agent,enemy combat and the player to their veriables
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyState = EnemyState.Patrol;
        _characterController = player.GetComponent<CharacterController>();
        //Giving the enemy a first location patrol
        nextLocation = 0;
        currentDestination = -1;
        SetAgentPatrolDestination();
        enemyCombat = GetComponent<EnemyCombat>();
    }

    bool CanSeePlayer()
    {
        RaycastHit PatrolHit;
        if (_characterController.wasCrouched)
        {
            sightRange = sightRangeCrouched;
        }
        else
        {
            sightRange = sightRangeStanding;
        }
        if(Physics.Raycast(transform.position, transform.forward, out PatrolHit, sightRange) && enemyState == EnemyState.Patrol)
        {
            Debug.DrawRay(transform.position, transform.forward * sightRange, Color.blue);
            if(PatrolHit.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        if(enemyState == EnemyState.PlayerSighted)
        {
            lookAtObject.transform.LookAt(player.transform.position);
        }
        RaycastHit AttackHit;
        if (Physics.Raycast(transform.position, lookAtObject.transform.forward, out AttackHit, sightRange) && enemyState == EnemyState.PlayerAttack || enemyState == EnemyState.PlayerSighted)
        {
            Debug.DrawRay(transform.position, lookAtObject.transform.forward * sightRange, Color.red);
            if (AttackHit.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        /*var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
       if(Physics.Raycast(ray, out hit, sightRange))
        {
            hit.transform.gameObject.
        }*/

        //every frame we are checking the distance between the enemy and its patrol point
        float distanceToTarget = Vector3.Distance(transform.position, patrolPoints[currentDestination].position);

        //every frame we are checking the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //checking wether the distance from the player is less than the assigned threshold for the enemy to begin chasing
        if(CanSeePlayer())
        {
            //change enemy state to player sighted
            enemyState = EnemyState.PlayerSighted;

            //gets player position and stores in playersightedPosision variable to use later.
            playerSightedPosition = player.transform.position;
        }

        //rest of update dictates what the enemy will do depending on its state.
        if (enemyState == EnemyState.Patrol)
        {
            //checkng if the enemy has reached its patrol point with some threshold allowance for distance.
            if (distanceToTarget <= distanceReachedThreshold)
            {
                //confirming thw enemy is at the location and is selling to the next patrol point in the array
                SetAgentPatrolDestination();
            }
        }

        //We checked earler if the player is close enough to be sighted
        if (enemyState == EnemyState.PlayerSighted)
        {
            //settin ght eenemy destination to the last seen player posisition rather than a patrol point.
            SetPlayerSightedDestination();

            //attack range should not be reached before player sighted so these if satements are nested
            //if the player is within attack range, setting the state to attack
            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange && CanSeePlayer())
            {
                enemyState = EnemyState.PlayerAttack;
            }

            //if the player outruns the enemys sight range will return to patrol mode
            if(Vector3.Distance(transform.position, playerSightedPosition) <= distanceReachedThreshold)
            {
                enemyState = EnemyState.Patrol;
                //manuly assigning the current patrol point as the enemy destination
                agent.SetDestination(patrolPoints[currentDestination].position);
            }
        }

        //checking if the enemy state is to attack
        if(enemyState == EnemyState.PlayerAttack)
        {

            //check to see if the player is out of attack range, if it is we set state to player sighted so enemy chases player
            if(Vector3.Distance(transform.position, player.transform.position) > attackRange)
            {
                enemyState = EnemyState.PlayerSighted;
            }
            //otherwise the player is in range
            else
            {
                //setting the enemy destanation to its own position to make it stop moving
                agent.SetDestination(transform.position);
                //make the enemy look at the player so that it fires projectioles in the corect derection
                agent.transform.LookAt(player.transform.position);

                //inititing our attack function making sure we attack x secons
                //(assigned by the attackSpeed verable on the EnemyCombat script)
                if (enemyCombat.canAttack)
                {
                    StartCoroutine(enemyCombat.Attack());
                }
            }
        }
    }
    

    void SetAgentPatrolDestination()
    {
        //Checking to make sure that we have a patrol point to move to, that the array has not run out.
        if (patrolPoints.Length > nextLocation)
        {
             //if we do have the next posion avaliable in the array, set the enemy destination to that point
            agent.SetDestination(patrolPoints[nextLocation].position);
            // adding 1 to the nextLocation so that next time we run through the function we are checking the next point in the array
            currentDestination = nextLocation;
            nextLocation++;
        }
        //Otherwise we are out of patrol points so go back to the first one and start the loop again.
        else
        {
            nextLocation = 0;
            currentDestination = - 1;
            SetAgentPatrolDestination();
        }
    }
    // this function is caled when the player is sighted and the enemy destinain is set to the last seen player position
    void SetPlayerSightedDestination()
    {
        agent.SetDestination(playerSightedPosition);
    }
}
