using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour {
    private Transform myTransform;
    private Vector3 destination;
    private GameObject owner;
    private float moveSpeed;
    private float force;
    private float damage;
    private float knockback;

	// Use this for initialization
	void Start () {
        myTransform = transform;
        myTransform.rotation = Quaternion.LookRotation(new Vector3(destination.x, myTransform.position.y, destination.z) - myTransform.position);
        myTransform.parent = GameObject.FindGameObjectWithTag("SkillsGroup").transform;
	}
	
	// Update is called once per frame
	void Update () {
        myTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner)
        {
            if (other.tag == "Enemy" || other.tag == "Player")
            {

                Vector3 direction = other.transform.position - myTransform.position;
                BaseCharacter bC = other.GetComponent<BaseCharacter>();
                if (bC.IsHollow) return;
                bC.AddImpact(direction, force);
                bC.ReceiveDamage(damage, knockback, owner, false);
                owner.GetComponent<BaseCharacter>().HitGold(SkillName.Fireball);
            }
            if(other.tag != "InvisibleSkill")Destroy(gameObject);
        }
    }

    public void InitValues(float fc, float dmg, Vector3 dest, GameObject own, float spd, float kb) {
        force = fc;
        damage = dmg;
        destination = dest;
        owner = own;
        moveSpeed = spd;
        knockback = kb;
    }
}
