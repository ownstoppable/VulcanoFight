using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : BaseCharacter {

	// Use this for initialization
	void Start () {
        nameLabel.GetComponent<TextMesh>().text = gameObject.name;
        characterC = GetComponent<CharacterController>();
        lastLavaDamage = Time.time;
        myTransform = transform;
        _inventory = new Dictionary<ItemName, Item>();

        InitializeStats();

        InitializeSkills();

        InitRound();
	}
	
	// Update is called once per frame
	void Update ()
    {

        #region AI

        #region Movement
        float objectiveDistance = Vector3.Distance(myTransform.position, destinationPosition);

        if (objectiveDistance < 0.5f)
        {
            GameObject platform = GameObject.FindGameObjectWithTag("Platform");
            switch (platform.GetComponent<Platform>().scaleLvl)
            {
                case 0:
                    destinationPosition = new Vector3(Random.Range(-20, 21), myTransform.position.y, Random.Range(-20, 21));
                    break;
                case 1:
                    destinationPosition = new Vector3(Random.Range(-15, 16), myTransform.position.y, Random.Range(-15, 16));
                    break;
                case 2:
                    destinationPosition = new Vector3(Random.Range(-10, 11), myTransform.position.y, Random.Range(-10, 11));
                    break;
                case 3:
                    destinationPosition = new Vector3(Random.Range(-5, 6), myTransform.position.y, Random.Range(-5, 6));
                    break;
                default:
                    break;
            }
            

        }
        else {
            if (!animation.IsPlaying("run")) animation.CrossFade("run");
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(new Vector3(destinationPosition.x, myTransform.position.y, destinationPosition.z) - myTransform.position), Time.deltaTime * 10);

            Vector3 moveDir = myTransform.TransformDirection(Vector3.forward);
            moveDir *= characterStats[StatName.Speed].CurValue;
            moveDir = new Vector3(moveDir.x, -9.8f, moveDir.z);
            characterC.Move(moveDir * Time.deltaTime);
        }
        #endregion Movement

        #region Attack
        if (attackPossible)
        {
            FireballSkill fb = skills[SkillName.Fireball] as FireballSkill;
            if (Time.time - fb.LastUsed > (fb.getCooldown * characterStats[StatName.CDR].CurValue + Random.Range(0, 0.5f)))
            {
                List<GameObject> enemies = new List<GameObject>(GameManager.Instance.GetAliveCharacters());
                enemies.Remove(gameObject);
                fb.LastUsed = Time.time;
                fb.Launch(gameObject, enemies[Random.Range(0, enemies.Count)].transform.position, characterStats[StatName.KBPower].CurValue, characterStats[StatName.Damage].CurValue);
            }
        }
        #endregion Attack

        #endregion AI

        #region UncontrolledBehavior
        // apply the impact force:
        if (impact.magnitude > 0.2) characterC.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        if (onLava && Time.time - lastLavaDamage > 1.0f)
        {
            ReceiveDamage(lavaDamage, 1, "Lava", true);
            lastLavaDamage = Time.time;
        }
        #endregion UncontrolledBehavior

        #region NameLabelUpdate
        Vector3 v = camTransform.position - nameLabel.position;
        v.x = v.z = 0.0f;
        nameLabel.LookAt(camTransform.position - v);
        nameLabel.rotation = (camTransform.rotation); // Take care about camera rotation
        #endregion NameLabelUpdate

    }

    public void InitRound()
    {
        onLava = false;
        camTransform = Camera.main.transform;
        destinationPosition = myTransform.position;
        characterStats[StatName.HP].ResetCurValue();
        characterStats[StatName.Mass].ResetCurValue();
        impact = Vector3.zero;

        //Initialize hit
        hitsReceived = new List<CharacterHit>();

        //Regen
        InvokeRepeating("HPRegeneration", 1, 1);
    }
}
