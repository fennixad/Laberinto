using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public static PlayerAnimator instance;
    Animator anim;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        RefreshAnimator();

        if (Input.GetMouseButtonDown(0)) RefreshShot();
    }

    [Range (-1f, 1f)]
    public float sideOffset;

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtPosition(transform.position + transform.forward + Vector3.up * 1.75f + transform.right * sideOffset);
        anim.SetLookAtWeight(1f, 0.5f, 1f, 0f, 0.5f);
    }

    /// <summary>
    /// Funcion encargada de interpretar que valores de "x" y "z" se van a usar en la invocacion de la funcion "RefreshMovement (float, float)"
    /// </summary>
    void RefreshAnimator()
    {
        bool aiming = MyInputManager.instance.GetAiming();       

        // PLAYER APUNTA...
        if (aiming)
        {
            Vector3 dirMove = PlayerMovement.instance.GetDirMovement();
            dirMove.Normalize();

            Vector3 localMoveDirection = transform.InverseTransformDirection(dirMove);
            localMoveDirection.Normalize();

            RefreshMovement(localMoveDirection.x, localMoveDirection.z);
        }
        // PLAYER NO APUNTA...
        else
        {
            Vector2 inputMove = MyInputManager.instance.GetInputMove();

            float _animParamX = 0f;
            float _animParamZ = 0f;

            _animParamX = 0f;

            // LA MAGNITUD DEL VECTOR "inputMove" tiene un valor superior a 0? Significa que estoy
            // haciendo uso de alguna combinacion de teclas WASD
            if (inputMove.magnitude > 0f) _animParamZ = 1f;
            else _animParamZ = 0f;
            RefreshMovement(_animParamX, _animParamZ);
        }
    }

    /// <summary>
    /// Funcion encargada de modificar el valor de los parametros "x" y "z" del animator controller del personaje
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_z"></param>
    public void RefreshMovement (float _x, float _z)
    {
        anim.SetFloat("x", _x, 0.15f, 1f);
        anim.SetFloat("z", _z, 0.15f, 1f);
    }

    public void RefreshShot()
    {
        anim.SetTrigger("shot");
    }
}