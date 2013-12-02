using UnityEngine;
using System.Collections;

public class FireballSkill : Skill {
    private float damage;
    private float force;
    private float speed;
    private float knockback;

    public FireballSkill(float cd, float dmg, float fc, float spd, float kb) {
        cooldown = cd;
        lastUsed = Time.time - cd;
        damage = dmg;
        force = fc;
        speed = spd;
        knockback = kb;
    }

    public void Launch(GameObject character, Vector3 dest, float fcMod, float dmgMod) {
        GameObject fireball = GameObject.Instantiate(Resources.Load("Skills/Fireball"), character.transform.position, Quaternion.identity) as GameObject;
        fireball.GetComponent<FireballController>().InitValues(force + fcMod, damage + dmgMod, dest, character, speed, knockback);
    }
}
