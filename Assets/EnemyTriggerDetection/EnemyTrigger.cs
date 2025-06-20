using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public bool playerInsideTrigger;

    public Transform player;

    [Range (1, 5)]
    public int rayMax;

    [Range(0.1f, 0.5f)]
    public float distanceBetweenRays;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInsideTrigger)
        {

            for (int i = 0; i < rayMax; i++)
            {
                Vector3 dirToPlayer = player.transform.position - transform.position;
                dirToPlayer.Normalize();

                Ray ray = new Ray(transform.position + new Vector3 (0f, distanceBetweenRays * (i + 1f), 0f), dirToPlayer);

                bool result = Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);

                if (result)
                {

                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
                    }
                    else
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.yellow);
                    }
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
        }
    }
}
