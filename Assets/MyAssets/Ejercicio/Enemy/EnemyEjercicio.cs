using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEjercicio : MonoBehaviour
{
    public EnemyStates currentState;
    public Transform player;
    public Transform safeZone;
    NavMeshAgent agent;
    public enum EnemyStates
    {
        Idle,
        Flee,
        Safe,
        Saved
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        currentState = EnemyStates.Idle;
    }

    // Update is called once per frame  
    void Update()
    {
        if (currentState == EnemyStates.Idle)
        {
            IdleBehaviour();
        }

    }

    public void ChangeState(EnemyStates newState)
    {
        switch (newState)
        {
            case EnemyStates.Idle:
                Debug.Log("Enemy is now Idle.");
                IdleBehaviour();
                break;
            case EnemyStates.Flee:
                Debug.Log("Enemy is now Fleeing.");
                FleeBehaviour();
                break;
            case EnemyStates.Safe:
                Debug.Log("Enemy is now Safe.");
                SafeBehaviour();
                break;
            case EnemyStates.Saved:
                Debug.Log("Enemy has been Saved.");
                break;
        }
    }

    private void IdleBehaviour()
    {
        transform.Rotate(Vector3.up, 40 * Time.deltaTime);
        Ray ray = new Ray(transform.position, transform.forward);
        ray.origin += Vector3.up * 0.5f; 

        bool result = Physics.Raycast(ray, out RaycastHit hit, 10f);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        if (result)
        {
            if (hit.collider.CompareTag("Player"))
            {
                ChangeState(EnemyStates.Flee);
            }
        }
    }

    private void FleeBehaviour()
    {
        transform.LookAt(safeZone);
        agent.SetDestination(safeZone.position);
    }

    private void SafeBehaviour()
    {
        StartCoroutine(SafeZoneCo());
    }

    IEnumerator SafeZoneCo()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(EnemyStates.Saved);
    }
}
