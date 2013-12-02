using UnityEngine;
using System.Collections;

public class HomingBallSkill : Skill {
    private float damage;
    private float force;
    private float speed;
    private float homingDistance;
    private float knockback;

    public HomingBallSkill(float cd, float dmg, float fc, float spd, float hD, float kb)
    {
        cooldown = cd;
        lastUsed = Time.time - cd;
        damage = dmg;
        force = fc;
        speed = spd;
        homingDistance = hD;
        knockback = kb;
    }

    public void Launch(GameObject character, Vector3 dest, float fcMod, float dmgMod)
    {
        GameObject homingball = GameObject.Instantiate(Resources.Load("Skills/Homingball"), character.transform.position, Quaternion.identity) as GameObject;
        homingball.GetComponent<HomingBallController>().InitValues(force + fcMod, damage + dmgMod, dest, character, speed, homingDistance, knockback);
    }
    
}
