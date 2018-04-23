using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : BulletController {

    public override void OnBulletHit(EnemyController enemy)
    {
        enemy.isSlowed = true;
        enemy.slowUntil = Time.time + 3f;

        base.OnBulletHit(enemy);


    }
}
