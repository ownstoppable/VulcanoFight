using UnityEngine;
using System.Collections;

public class MeteorBlastSkill : Skill {
    private float damage;
    private float force;
    private float knockback;

    public MeteorBlastSkill(float cd, float dmg, float fc, float kb)
    {
        cooldown = cd;
        lastUsed = Time.time - cd;
        damage = dmg;
        force = fc;
        knockback = kb;
    }

    public void Launch(GameObject character, Vector3 dest, float fcMod, float dmgMod)
    {
        GameObject meteor = GameObject.Instantiate(Resources.Load("Skills/Meteor"), new Vector3(dest.x, 6, dest.z), Quaternion.identity) as GameObject;
        meteor.GetComponent<MeteorBlastController>().InitValues(force + fcMod, damage + dmgMod, character, knockback);
    }
}
