using System.Collections;
using UnityEngine;

public class PlataformaVelocidad : MonoBehaviour
{
    [Header("Configuración de Empuje")]
    [SerializeField] private float pushForce = 50f;          // Fuerza del empuje (boost)
    [SerializeField] private float boostDuration = 0.5f;      // Duración de la fuerza extra
    [SerializeField] private float cooldownDuration = 1.0f;   // Tiempo para regresar a la velocidad normal

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody carRb = other.GetComponent<Rigidbody>();

            if (carRb != null)
            {
                // Inicia la secuencia completa: Empuje -> Recuperación
                StartCoroutine(AplicarBoostYRecuperar(carRb, other.transform));
            }
        }
    }

    private IEnumerator AplicarBoostYRecuperar(Rigidbody carRb, Transform carTransform)
    {
        // 1. Guardamos la magnitud de velocidad que traía el carro antes del boost
        float velocidadInicial = carRb.velocity.magnitude;

        // FASE 1: Empuje (Boost)
        float tiempoEmpuje = 0f;
        while (tiempoEmpuje < boostDuration)
        {
            if (carRb != null)
            {
                // Aplica la fuerza de empuje hacia adelante
                carRb.AddForce(carTransform.forward * pushForce, ForceMode.Impulse);
            }

            tiempoEmpuje += Time.deltaTime;
            yield return null;
        }

        // FASE 2: Recuperación de la velocidad original
        if (carRb != null)
        {
            // Guardamos la velocidad máxima alcanzada tras el boost
            Vector3 velocidadAlcanzada = carRb.velocity;
            float tiempoRecuperacion = 0f;

            while (tiempoRecuperacion < cooldownDuration)
            {
                if (carRb != null)
                {
                    // Calculamos la velocidad objetivo conservando la dirección actual pero reduciendo la magnitud al valor inicial
                    Vector3 velocidadObjetivo = carRb.velocity.normalized * velocidadInicial;

                    // Interpolamos la velocidad suavemente de vuelta al valor base
                    carRb.velocity = Vector3.Lerp(velocidadAlcanzada, velocidadObjetivo, tiempoRecuperacion / cooldownDuration);
                }

                tiempoRecuperacion += Time.deltaTime;
                yield return null;
            }
        }
    }
}