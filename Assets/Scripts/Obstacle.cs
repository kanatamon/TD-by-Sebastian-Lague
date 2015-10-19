using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour, IDamageable {

    public ParticleSystem bulletCollisionEffect;

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        Material mat = GetComponent<Renderer>().material;

        ParticleSystem effect = Instantiate(bulletCollisionEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, -1 * hitDirection)) as ParticleSystem;
        effect.GetComponent<Renderer>().material.color = mat.color;

        Destroy(effect.gameObject, bulletCollisionEffect.startLifetime);
        
    }

    public void TakeDamage(float damage){}

}
