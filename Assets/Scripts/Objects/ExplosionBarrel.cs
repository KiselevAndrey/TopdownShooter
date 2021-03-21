using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : MonoBehaviour
{
    [Header("Основные параметры взрыва")]
    [SerializeField] float maxDamage = 100f;
    [SerializeField] float explosionForce = 1000f;
    [SerializeField] float explosionRadius = 5f;

    [Header("Визуализация")]
    [SerializeField] ParticleSystem preparationParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] AudioSource explosionAudio;

    #region Explosion
    void Preparation()
    {
        preparationParticles.Play();
    }

    void Explosion()
    {
        Collider2D selfCollider = GetComponent<Collider2D>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == selfCollider) continue;

            Rigidbody2D targetRigidbody = colliders[i].GetComponent<Rigidbody2D>();

            if (!targetRigidbody)
                continue;

            AddExplosionForce(targetRigidbody, explosionForce, transform.position, explosionRadius);

            Health targetHealth = targetRigidbody.GetComponent<Health>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);
            targetHealth.Hit(damage);
        }

        explosionParticles.transform.parent = null;

        explosionParticles.Play();
        //explosionAudio.Play();

        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector2 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        float damage = relativeDistance * maxDamage;
        damage = Mathf.Max(0f, damage);

        return damage;
    }

    public static void AddExplosionForce(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
            explosionDir /= explosionDistance;
        else
        {
            // From Rigidbody.AddExplosionForce doc:
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        Vector2 force = Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir;
        rb.AddForce(force, mode);
    }
    #endregion
}
