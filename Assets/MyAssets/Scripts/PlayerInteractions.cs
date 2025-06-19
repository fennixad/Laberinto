using System.IO.Pipes;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public static PlayerInteractions instance;

    Transform cam;

    public bool interactableDetected;
    public InteractablesObjects currentInteractable;

    public bool detectDoorsByTriggers;
    public bool detectDoorsByRaycast;

    public LayerMask interactRaycastMask;

    private void Awake()
    {
        instance = this;
        cam = Camera.main.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // PRESIONAS LA TECLA DE INTERACCION..?
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (detectDoorsByRaycast)
            {
                Ray ray = new Ray(transform.position + Vector3.up * 1.5f, transform.forward);

                bool frontalRaycastResult = Physics.Raycast(ray, out RaycastHit hitInfo, 2f, interactRaycastMask);

                if (frontalRaycastResult)
                {
                    if (hitInfo.collider.CompareTag("Interactables"))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red, 1f);
                        hitInfo.collider.GetComponent<InteractablesObjects>().Interact();
                    }
                    else
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.yellow, 1f);
                    }
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * 2f, Color.green, 1f);
                }

            }


            if (interactableDetected)
            {
                currentInteractable.Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player entra en trigger {other.name}");

        if (detectDoorsByTriggers && other.CompareTag("Interactables"))
        {
            interactableDetected = true;
            currentInteractable = other.GetComponent<InteractablesObjects>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Player entra en trigger {other.name}");

        if (detectDoorsByTriggers && other.CompareTag("Interactables")) 
        {
            interactableDetected = false;
            currentInteractable = null;
        }
    }

    void CamRaycast()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        bool result = Physics.Raycast(ray, out RaycastHit hit, 50f);

        if (result)
        {
            Debug.Log($"Rayo interactivo detecta: {hit.collider.name}");

            if (hit.collider.CompareTag("NPCs"))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 1f);
            }
        }
        else
        {
            Debug.Log("Rayo interactivo no detecta nada");
            Debug.DrawRay(ray.origin, ray.direction * 50f, Color.blue, 1f);
        }
    }
}
