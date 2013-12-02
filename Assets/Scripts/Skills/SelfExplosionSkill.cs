using UnityEngine;
using System.Collections;

public class SelfExplosionSkill : Skill {
    private float damage;
    private float force;
    private float radius;
    public float selfDamage;
    private float knockback;

    public SelfExplosionSkill(float cd, float dmg, float fc, float rad, float cast, float selfD, float kb)
    {
        cooldown = cd;
        lastUsed = Time.time - cd;
        damage = dmg;
        force = fc;
        radius = rad;
        castTime = cast;
        selfDamage = selfD;
        knockback = kb;
    }

    public void Launch(GameObject character, float fcMod, float dmgMod)
    {
        GameObject selfExplosion = GameObject.Instantiate(Resources.Load("Skills/SelfExplosion"), character.transform.position, Quaternion.identity) as GameObject;
        selfExplosion.GetComponent<SelfExplosionController>().InitValues(force + fcMod, damage + dmgMod, radius, character, knockback);
    }
}
