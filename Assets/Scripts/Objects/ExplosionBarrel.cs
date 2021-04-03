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

    [Header("Доп переменные")]
    [SerializeField] int maxSoundsPlayed;

    #region Explosion
    void Preparation()
    {
        preparationParticles.Play();
    }

    void Explosion()
    {
        Collider2D selfCollider = GetComponent<Collider2D>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        int j = 0;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == selfCollider) continue;

            Rigidbody2D targetRigidbody = colliders[i].GetComponent<Rigidbody2D>();

            if (targetRigidbody)
                AddExplosionForce(targetRigidbody, explosionForce, transform.position, explosionRadius);

            Health targetHealth = colliders[i].GetComponent<Health>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(colliders[i].transform.position);
            bool playSound = j <= maxSoundsPlayed || targetHealth.GetComponent<Player>();
            targetHealth.Hit(damage, playSound);
            j++;
        }

        explosionParticles.transform.parent = transform.parent;
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();

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

    void AddExplosionForce(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, ForceMode2D mode = ForceMode2D.Force)
    {
        Vector2 explosionDir = rb.position - explosionPosition;
        float explosionDistance = explosionDir.magnitude;

        explosionDir /= explosionDistance;

        Vector2 force = Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir;
        rb.AddForce(force, mode);
    }

    void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }
    #endregion
}
