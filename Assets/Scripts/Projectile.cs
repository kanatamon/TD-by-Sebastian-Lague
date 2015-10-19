using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    float damage = 1f;
    float speed = 10f;

    float lifeTime = 3f;
    float generalWidth = .1f;

    void Start(){
        Destroy(gameObject, lifeTime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);   
        }

    }

    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }

    void Update(){
        float moveDistance = Time.deltaTime * speed;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance){
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + generalWidth, collisionMask))
        {
            OnHitObject(hit.collider, hit.point);
        }

    }

    void OnHitObject(Collider other, Vector3 hitPoint){
        IDamageable damageableObject = other.GetComponent<IDamageable>();
        
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
        
        GameObject.Destroy(gameObject);
    }
}
