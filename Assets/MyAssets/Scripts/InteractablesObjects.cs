using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// RESUMEN: Script comun de todos los tipos de puertas que guardan relacion con un interruptor...
/// </summary>

public class InteractablesObjects : MonoBehaviour
{
    Animator anim;
    Material mat;

    public enum InteractableTypes
    {
        Door_A, Door_B, Door_C, Door_D, Door_E
    }

    public InteractableTypes interactable;

    public bool isDoorOpen;


    private void Awake()
    {
        anim = transform.parent.GetChild(0).GetComponent<Animator>();

        if (interactable != InteractableTypes.Door_A)
        {
            mat = transform.GetChild(0).GetComponent<Renderer>().material;
            mat.color = Color.red;
        }
    }

    public void Interact()
    {
        Debug.Log($"Interactua con elemento: {interactable}");

        switch (interactable)
        {
            case InteractableTypes.Door_A:
                break;
            case InteractableTypes.Door_B:

                if (!isDoorOpen)
                {
                    anim.SetBool("isOpen", true);
                    ChangeSwitchColor(Color.green);
                    isDoorOpen = true;
                }
                break;
            case InteractableTypes.Door_C:

                if (!isDoorOpen)
                {
                    transform.parent.gameObject.SetActive(false);
                    mat.color = Color.green;
                    isDoorOpen = true;
                }
                break;
            case InteractableTypes.Door_D:

                if (!isDoorOpen) StartCoroutine(TimerCoro());
                //GetComponent<InteractableObject_Timer>().StartTimer();
                break;
            case InteractableTypes.Door_E:
                break;
        }
    }

    // Cambiar el color del interruptor
    public void ChangeSwitchColor(Color _newColor)
    {
        mat.color = _newColor;
    }

    IEnumerator TimerCoro()
    {
        anim.SetBool("isOpen", true);
        ChangeSwitchColor(Color.green);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isOpen", false);
        ChangeSwitchColor(Color.red);
        isDoorOpen = false;
    }
}
