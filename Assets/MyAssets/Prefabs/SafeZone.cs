using UnityEngine;

public class SafeZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemies"))
        {
            Debug.Log("Player has entered the safe zone.");
            EnemyEjercicio enemy = other.GetComponent<EnemyEjercicio>();
            if (enemy != null)
            {
                enemy.ChangeState(EnemyEjercicio.EnemyStates.Safe);
            }
        }
    }
}
