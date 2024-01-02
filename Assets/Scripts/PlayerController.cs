using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{  
    public Rigidbody rb; 
    public float maxAngularVelocity = 100f;
    public float rotationRecoilForce = 2f;
    public float maxSpeed = 5f;
    private float lastAbilityUseTime = 0f;
    public float abilityCooldown = 2f;
    public float rotationSpeed = 360f;
    private Vector3 fixedZPosition; // Posizione Z fissa

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedZPosition = transform.position; // Imposta la posizione Z iniziale
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastAbilityUseTime >= abilityCooldown)
        {
            FlipPlayerInstantly();
            lastAbilityUseTime = Time.time;
        }

        // Limita la posizione Z del player
        transform.position = new Vector3(transform.position.x, transform.position.y, fixedZPosition.z);
    }


        void FixedUpdate()
    {
        // Controlla e limita la velocità del player
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    // Limita la posizione del player rispetto alle coordinate desiderate o alla vista della camera principale
    LimitPlayerPosition();


     }
    public void FlipButtonPressed()
{
    if (Time.time - lastAbilityUseTime >= abilityCooldown)
    {
        FlipPlayerInstantly();
        lastAbilityUseTime = Time.time;
    }
}
void LimitPlayerPosition()
{
    Vector2 clampedPosition = transform.position;

    // Imposta le coordinate desiderate o usa i limiti della vista della camera principale
    float minX = -5f;  // Modifica con il valore minimo desiderato o lascia -Mathf.Infinity per nessun limite
    float maxX = 5f;   // Modifica con il valore massimo desiderato o lascia Mathf.Infinity per nessun limite
    float minY = -10f;  // Modifica con il valore minimo desiderato o lascia -Mathf.Infinity per nessun limite
    float maxY = 8.2f;   // Modifica con il valore massimo desiderato o lascia Mathf.Infinity per nessun limite

    // Limita la posizione del player
    clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
    clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

    // Applica la posizione limitata al player
    transform.position = clampedPosition;
}
void FlipPlayerInstantly()
{
    
    // Calcola la nuova rotazione come rotazione attuale + 180 gradi sull'asse z
    Quaternion additionalRotation = Quaternion.Euler(0, 0f, 180);
    rb.MoveRotation(rb.rotation * additionalRotation);
        

}
    public void AddRecoilAndRotation(float recoilForce, Vector3 direction)
    {
        // Applica la forza e aggiunge torque in un ambiente 3D
        rb.AddForce(-direction.normalized * recoilForce, ForceMode.Impulse);
        AddTorqueBasedOnDirection(direction);
    }

void AddTorqueBasedOnDirection(Vector3 direction)
{
    float rotationDirection = direction.x > 0 ? -1f : 1f;
    float torque = rotationDirection * rotationRecoilForce;

    // Applica il torque sull'asse Z
    rb.AddTorque(new Vector3(0, 0, torque), ForceMode.Impulse);
    rb.angularVelocity = new Vector3(rb.angularVelocity.x, rb.angularVelocity.y, Mathf.Clamp(rb.angularVelocity.z, -maxAngularVelocity, maxAngularVelocity));
}
}
