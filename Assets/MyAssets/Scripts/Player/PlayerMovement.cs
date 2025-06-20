using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    CharacterController cc;
    Transform cam;

    [Range(0f, 5f)]
    public float speed;

    [Tooltip ("TRUE rotacion suave, FALSE rotacion directa")]
    public bool modoRotacion;


    public LayerMask layerMask;

    private void Awake()
    {
        instance = this;
        cc = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleRotation();
    }

    void HandleMove()
    {
        Vector3 dirMov = GetDirMovement();
        Debug.DrawRay(transform.position, dirMov, Color.green);
        cc.Move(dirMov * speed * Time.deltaTime);
    }

    void HandleRotation()
    {
        if (!MyInputManager.instance.GetAiming())
        {
            Vector2 input = MyInputManager.instance.GetInputMove();
            if (input.magnitude > 0f) transform.rotation = CalculateRotationNoAiming(modoRotacion);
        }
        else transform.rotation = CalculateRotationAiming (modoRotacion);
    }

    public Vector3 GetDirMovement()
    {
        Vector2 input = MyInputManager.instance.GetInputMove();
        input.Normalize();
        
        // el resultado se asigna segun los ejes locales de la camara de la escena
        Vector3 result = cam.right * input.x + cam.up * input.y;

        // el resultado se asigna segun los ejes globales de la escena
        //Vector3 result = new Vector3(input.x, 0f, input.y);

        return result;
    }

    Quaternion CalculateRotationNoAiming (bool _smoothMode)
    {
        Vector3 dirMov = GetDirMovement();

        Quaternion dirRotation = Quaternion.LookRotation(dirMov);
        Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, dirRotation, 10f * Time.deltaTime);

        return _smoothMode ? smoothRotation : dirRotation;       
    }

    Quaternion CalculateRotationAiming(bool _smoothMode)
    {
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Hacemos raycast contra el suelo
        if (Physics.Raycast(rayo, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 objetivo = hit.point;

            // Dibujar rayo en la escena
            Debug.DrawRay(rayo.origin, rayo.direction * hit.distance, Color.red);

            // Obtener dirección horizontal hacia el punto (sin altura)
            Vector3 dirToMouse = objetivo - transform.position;
            dirToMouse.y = 0f;

            // Solo rotar si hay dirección (evitar errores con Vector3.zero)
            if (dirToMouse.magnitude > 0f)
            {
                Quaternion dirRotation = Quaternion.LookRotation(dirToMouse);
                Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, dirRotation, 10f * Time.deltaTime);

                return _smoothMode ? smoothRotation : dirRotation;
            }
            else return transform.rotation;

        } 
        else return transform.rotation;
    }
}