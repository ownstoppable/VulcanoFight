using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteorBlastController : MonoBehaviour {
    public GameObject meteorBall;
    public GameObject meteorBlast;
    private Transform myTransform;
    private GameObject owner;
    private float force;
    private float damage;
    private float knockback;

	// Use this for initialization
	void Start () {
        myTransform = transform;
        myTransform.parent = GameObject.FindGameObjectWithTag("SkillsGroup").transform;
	}

    void OnTriggerEnter(Collider other)
    {

        if (!rigidbody.isKinematic && (other.tag == "Platform" || other.tag == "Lava"))
        {
            rigidbody.isKinematic = true;
            meteorBall.SetActive(false);
            meteorBlast.SetActive(true);
            List<Collider> colls = new List<Collider>(Physics.OverlapSphere(transform.position, 3));
            for (int i = 0; i < colls.Count; i++)
            {
                if (colls[i].gameObject != owner && (colls[i].tag == "Enemy" || colls[i].tag == "Player")) {
                    Vector3 direction = colls[i].transform.position - myTransform.position;
                    BaseCharacter bC = colls[i].GetComponent<BaseCharacter>();
                    if (bC.IsHollow) return;
                    bC.AddImpact(direction, force);
                    bC.ReceiveDamage(damage, knockback, owner, false);
                    owner.GetComponent<BaseCharacter>().HitGold(SkillName.MeteorBlast);
                }
            }
            Destroy(gameObject, 1);
        }
    }

    public void InitValues(float fc, float dmg, GameObject own, float kb)
    {
        force = fc;
        damage = dmg;
        owner = own;
        knockback = kb;
    }
}
