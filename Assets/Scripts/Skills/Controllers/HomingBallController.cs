using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingBallController : MonoBehaviour {
    private Transform myTransform;
    private Vector3 destination;
    private GameObject owner;
    private float moveSpeed;
    private float force;
    private float damage;
    private float homingDistance;
    private float chaseSmooth;
    private float currentSpeed;
    private float knockback;

    private Vector3 initialPosition;
    private float launchDistance;
    private Transform targetCharacter;

    private enum status { 
        Launch,
        Search,
        Chase,
        Move
    }
    private status currentStatus;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        initialPosition = myTransform.position;
        launchDistance = 3;
        chaseSmooth = 4;
        currentSpeed = moveSpeed * 2;
        myTransform.rotation = Quaternion.LookRotation(new Vector3(destination.x, myTransform.position.y, destination.z) - myTransform.position);
        myTransform.parent = GameObject.FindGameObjectWithTag("SkillsGroup").transform;
        currentStatus = status.Launch;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStatus)
        {
            case status.Launch:
                if (Vector3.Distance(myTransform.position, initialPosition) >= launchDistance) {
                    currentStatus = status.Search;
                }
                break;
            case status.Search:
                List<GameObject> characters = new List<GameObject>(GameManager.Instance.GetAliveCharacters());
                characters.Remove(owner);
                List<GameObject> nearCharacters = new List<GameObject>();
                for (int i = 0; i < characters.Count; i++)
                {
                    if (Vector3.Distance(myTransform.position, characters[i].transform.position) <= homingDistance) {
                        nearCharacters.Add(characters[i]);
                    }
                }
                if (nearCharacters.Count > 0)
                {
                    targetCharacter = nearCharacters[Random.Range(0, nearCharacters.Count)].transform;
                    currentStatus = status.Chase;
                }
                else currentStatus = status.Move;
                break;
            case status.Chase:
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(new Vector3(targetCharacter.position.x, myTransform.position.y, targetCharacter.position.z) - myTransform.position), Time.deltaTime * chaseSmooth);
                if (Vector3.Distance(myTransform.position, targetCharacter.position) > homingDistance || !targetCharacter.gameObject.activeSelf) {
                    currentStatus = status.Move;
                }
                break;
            case status.Move:
                break;
            default:
                break;
        }
        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime);
        myTransform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner)
        {
            if (other.tag != "Skillshot" && other.tag != "TerrainLimit")
            {

                Vector3 direction = other.transform.position - myTransform.position;
                BaseCharacter bC = other.GetComponent<BaseCharacter>();
                bC.AddImpact(direction, force);
                bC.ReceiveDamage(damage, knockback, owner, false);
                owner.GetComponent<BaseCharacter>().HitGold(SkillName.Homingball);
            }
            Destroy(gameObject);
        }
    }

    public void InitValues(float fc, float dmg, Vector3 dest, GameObject own, float spd, float homingD, float kb)
    {
        force = fc;
        damage = dmg;
        destination = dest;
        owner = own;
        moveSpeed = spd;
        homingDistance = homingD;
        knockback = kb;
    }
}
