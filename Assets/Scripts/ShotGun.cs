using UnityEngine;
using System.Collections;

public class ShotGun : Gun {
    public Transform muzzleR;
    public Transform muzzleL;

    public override void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;

            Projectile b1 = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            b1.SetSpeed(muzzleVelocity);

            Projectile b2 = Instantiate(projectile, muzzleR.position, muzzleR.rotation) as Projectile;
            b2.SetSpeed(muzzleVelocity);

            Projectile b3 = Instantiate(projectile, muzzleL.position, muzzleL.rotation) as Projectile;
            b3.SetSpeed(muzzleVelocity);

        }
    }

}
