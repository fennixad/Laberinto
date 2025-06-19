using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class Enemy : MonoBehaviour
{

    public States currentState;
    Transform player;
    Material eyesMat;
    NavMeshAgent agent;

    Coroutine myCoro;
    Vector3 posInitial, lastPlayerPos;

    float timerPlayerLost;

    public enum States
    {
        Idle, Suprised, FollowingPlayer, PlayerLost, PlayerCaught, ReturnToOrigin
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // se almacena en la variable el "Transform" del objeto asignado con etiqueta "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // se almacena en la variable el material asignado al segundo hijo
        eyesMat = transform.GetChild(1).GetComponent<Renderer>().material;

        posInitial = transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(States.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == States.PlayerLost)
        {
            Vector3 dirToDestiny = lastPlayerPos - transform.position;
            dirToDestiny.y = 0f;

            Debug.DrawRay(transform.position, dirToDestiny, Color.pink);
            if (dirToDestiny.magnitude < 0.025f)
            {
                if (timerPlayerLost < 5f) timerPlayerLost += Time.deltaTime;
                else
                {
                    ChangeState(States.Idle);
                }
            }
        }

        if (currentState == States.FollowingPlayer)
        {
            Vector3 dirToPlayer = player.transform.position - transform.position;
            dirToPlayer.y = 0f;

            if (dirToPlayer.magnitude < 0.5f) ChangeState(States.PlayerCaught);
        }
        
    }

    private void FixedUpdate()
    {
        ForwardRaycast();
    }

    void ChangeState (States newState)
    {
        currentState = newState;
        Debug.Log($"Estado enemigo <color=green>{currentState}</color>");

        switch (currentState)
        {
            case States.Idle:

                eyesMat.color = Color.green;

                agent.isStopped = false;
                agent.SetDestination(posInitial);
                break;
            case States.Suprised:
                break;
            case States.FollowingPlayer:

                eyesMat.color = Color.red;

                agent.isStopped = false;
                StartMyCoro();
                break;
            case States.PlayerLost:

                eyesMat.color = Color.yellow;
                StopMyCoro();

                agent.isStopped = false;
                lastPlayerPos = player.position;
                agent.SetDestination(lastPlayerPos);
                break;
            case States.PlayerCaught:

                agent.isStopped = true;
                break;

        }
    }

    void ForwardRaycast()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 1.5f, transform.forward);
        bool result = Physics.Raycast(ray, out RaycastHit hitInfo, 5f);

        if (result)
        {
            bool isPlayerDetected = hitInfo.collider.CompareTag("Player");
            Color raycastColor = isPlayerDetected ? Color.red : Color.yellow;

            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, raycastColor);

            if (isPlayerDetected)
            {
                if (currentState != States.FollowingPlayer) ChangeState(States.FollowingPlayer);

            }
            else
            {
                if (currentState == States.FollowingPlayer) ChangeState(States.PlayerLost);
            }            
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.green);

            if (currentState == States.FollowingPlayer) ChangeState (States.PlayerLost);
        }
    }

    void StartMyCoro()
    {
        if (myCoro == null)
        {
            myCoro = StartCoroutine(MyCoro());

        }
    }

    void StopMyCoro()
    {
        if (myCoro != null)
        {
            StopCoroutine(myCoro);
            myCoro = null;
        }
    }

    IEnumerator MyCoro()
    {
        while (true)
        {
            agent.SetDestination(player.position);
            yield return new WaitForSeconds (0.25f);
        }
    }
}
