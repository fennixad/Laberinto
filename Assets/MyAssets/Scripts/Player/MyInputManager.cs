using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class MyInputManager : MonoBehaviour
{
    public static MyInputManager instance;
    public Vector2 inputMove; // almacena los ejes virtuales "horizontal" y "vertical"
    public bool aiming;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetInputMove();
        SetAiming();
    }

    void SetInputMove()
    {
        inputMove.x = Input.GetAxisRaw("Horizontal");
        inputMove.y = Input.GetAxisRaw("Vertical");

    }

    void SetAiming()
    {
        aiming = Input.GetMouseButton(1);
    }

    public bool GetAiming()
    {
        return aiming;
    }

    /// <summary>
    /// Resumen: Esta funcion devuelve el valor Vector2 almacenado en la variable inputMove.
    /// IMPORTANTE! No esta normalizado.
    /// </summary>
    public Vector2 GetInputMove()
    {
        return inputMove;
    }
}
