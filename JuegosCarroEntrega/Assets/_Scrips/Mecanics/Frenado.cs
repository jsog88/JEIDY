using System.Collections;
using UnityEngine;

public class CharcoRalentizador : MonoBehaviour
{
    [Header("Impacto Inmediato (Fluido)")]
    // Porcentaje de velocidad física que conserva al entrar (0.45f = conserva el 45%, pierde el 55%)
    [Range(0.1f, 0.9f)]
    [SerializeField] private float retencionVelocidad = 0.45f;

    // Resistencia de arrastre que se añade al Rigidbody dentro del agua para absorber el movimiento
    [SerializeField] private float resistenciaArrastre = 2.5f;

    [Header("Potencia en el Pantano")]
    // Porcentaje de potencia de motor permitido dentro del charco
    [Range(0.1f, 1f)]
    [SerializeField] private float factorVelocidad = 0.25f;

    private float velocidadOriginalMotor;
    private float dragOriginal;
    private bool estaEnPantano = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarMovement car = other.GetComponent<CarMovement>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (car != null && rb != null && !estaEnPantano)
            {
                estaEnPantano = true;

                // 1. Guardamos la configuración original del carro
                velocidadOriginalMotor = car.speed;
                dragOriginal = rb.drag;

                // 2. FRENADO INMEDIATO: Conserva solo una parte del vector de velocidad actual.
                // Al conservar la dirección del vector pero reducir su magnitud, no hay choque brusco, solo una desaceleración violenta.
                rb.velocity = rb.velocity * retencionVelocidad;

                // 3. Añadimos resistencia de arrastre física (Drag) para simular la densidad del lodo
                rb.drag = resistenciaArrastre;

                // 4. Reducimos la potencia del motor mientras esté atrapado
                car.speed = velocidadOriginalMotor * factorVelocidad;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarMovement car = other.GetComponent<CarMovement>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (car != null && rb != null && estaEnPantano)
            {
                // Restauramos los valores originales al salir del charco
                rb.drag = dragOriginal;
                car.speed = velocidadOriginalMotor;
                estaEnPantano = false;
            }
        }
    }
}