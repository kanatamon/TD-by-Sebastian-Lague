using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public Transform muzzle;
    public Transform injectionT;
    public Projectile projectile;
    public Shell shell;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;

    protected float nextShotTime;

    public virtual void Shoot(){

        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;

            // Fire a bullet
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);

            // Inject a shell
            //Shell newShell = Instantiate(shell, injectionT.position, injectionT.rotation) as Shell;
            Instantiate(shell, injectionT.position, injectionT.rotation);

            // Animate gun using script controll
            StartCoroutine(React());
        }
    }

    IEnumerator React(){
        Vector3 maxPoint = transform.forward * -.2f;
        float percent = 0;
        float time = msBetweenShots / 1000f;
        float speed = 1 / time;

        Vector3 from = Vector3.zero;
        Vector3 to = Vector3.zero + maxPoint;

        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;
            float interpolation = (-Mathf.Pow(percent,2) + percent)*4;
            transform.localPosition = Vector3.Lerp(from, to, interpolation);

            yield return null;
        }
    }

}
