using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelfExplosionController : MonoBehaviour {
    private Transform myTransform;
    private GameObject owner;
    private float force;
    private float damage;
    private float radius;
    private float knockback;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        myTransform.parent = GameObject.FindGameObjectWithTag("SkillsGroup").transform;
        GetComponent<SphereCollider>().radius = radius;
        Destroy(gameObject, myTransform.FindChild("Particles").GetComponent<ParticleSystem>().duration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner)
        {
            if (other.tag != "Skillshot")
            {
                Vector3 direction = other.transform.position - myTransform.position;
                BaseCharacter bC = other.GetComponent<BaseCharacter>();
                if (bC.IsHollow) return;
                bC.AddImpact(direction, force);
                bC.ReceiveDamage(damage, knockback, owner, false);
                owner.GetComponent<BaseCharacter>().HitGold(SkillName.SelfExplosion);
            }
        }
    }

    public void InitValues(float fc, float dmg, float rad, GameObject own, float kb)
    {
        force = fc;
        damage = dmg;
        owner = own;
        radius = rad;
        knockback = kb;
    }
}
