using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : BaseCharacter {

    private float destinationDistance;
 
    public GameObject AimUI;

    public enum PlayerMode { 
        Movement,
        Casting,
        Idle
    }

    public PlayerMode currentStatus;
 
	void Start () {
		myTransform = transform;
        characterC = GetComponent<CharacterController>();
        lastLavaDamage = Time.time;

        inventory = new Dictionary<ItemName, Item>();
        AimUI = transform.FindChild("AimUI").gameObject;

        InitializeStats();

        InitializeSkills();

        InitRound();

        characterStats[StatName.Armor].ChangeCurTotal(0.5f);
	}
 
	void Update () {

        #region SkillInput
        //Skills
        if (HasSkill(SkillName.Fireball) && attackPossible && Input.GetButtonDown("Skill 1"))
        {
            PrepSkill(SkillName.Fireball);
        }
        if (HasSkill(SkillName.SelfExplosion) && attackPossible && Input.GetButtonDown("Skill 2"))
        {
            PrepSkill(SkillName.SelfExplosion);
        }
        if (HasSkill(SkillName.Teleport) && attackPossible && Input.GetButtonDown("Skill 3"))
        {
            PrepSkill(SkillName.Teleport);
        }
        if (HasSkill(SkillName.Homingball) && attackPossible && Input.GetButtonDown("Skill 4"))
        {
            PrepSkill(SkillName.Homingball);
        }


        if (skillBeingCast != SkillName.None && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)))
        {
            skillBeingCast = SkillName.None;
            AimUI.SetActive(false);
        }
        #endregion SkillInput

 
		// Moves the Player if the Left Mouse Button was clicked
		if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl ==0) {
 
			Plane playerPlane = new Plane(Vector3.up, myTransform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hitdist = 0.0f;

			if (playerPlane.Raycast(ray, out hitdist)) {
                switch (currentStatus)
                {
                    case PlayerMode.Movement:
                    case PlayerMode.Idle:
                        if (skillBeingCast == SkillName.None)
                        {
                            destinationPosition = ray.GetPoint(hitdist);
                            Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
                            myTransform.rotation = targetRotation;
                            currentStatus = PlayerMode.Movement;
                        }
                        break;
                    case PlayerMode.Casting:
                        break;
                    default:
                        break;
                }

                if (skillBeingCast != SkillName.None)
                {
                    myTransform.rotation = Quaternion.LookRotation(ray.GetPoint(hitdist) - transform.position);
                    AimUI.SetActive(false);
                    StartCoroutine(LaunchSkill(skillBeingCast, ray.GetPoint(hitdist)));
                    skillBeingCast = SkillName.None;
                    currentStatus = PlayerMode.Casting;
                }
			}
		}
 
		// Moves the player if the mouse button is hold down
		/*else if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0 && currentStatus == PlayerSkill.Movement) {
			Plane playerPlane = new Plane(Vector3.up, myTransform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hitdist = 0.0f;
 
			if (playerPlane.Raycast(ray, out hitdist)) {
				destinationPosition = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
				myTransform.rotation = targetRotation;
			}

		}
         */

        Vector3 moveDir = Vector3.zero;
        if (currentStatus == PlayerMode.Movement)
        {
            // keep track of the distance between this gameObject and destinationPosition
            destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

            if (destinationDistance > .5f)
            {
                moveDir = transform.TransformDirection(Vector3.forward);
                moveDir *= characterStats[StatName.Speed].CurValue;

            }
            else {
                currentStatus = PlayerMode.Idle;
            }
        }
        //Apply gravity
        moveDir = new Vector3(moveDir.x, -9.8f, moveDir.z);
        characterC.Move(moveDir* Time.deltaTime);

        #region UncontrollableBehaviours
        // apply the impact force:
        if (impact.magnitude > 0.2) characterC.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        // Name Label rotation towards camera
        Vector3 v = camTransform.position - nameLabel.position;
        v.x = v.z = 0.0f;
        nameLabel.LookAt(camTransform.position - v);
        nameLabel.rotation = (camTransform.rotation); 

        // Lava Damage
        if (onLava && Time.time - lastLavaDamage > 1.0f)
        {
            ReceiveDamage(lavaDamage, 1, "Lava");
            lastLavaDamage = Time.time;
        }
        #endregion UncontrollableBehaviours
    }

    public void PrepSkill(SkillName skill)
    {
        switch (skill)
        {
            case SkillName.Fireball:
            case SkillName.Teleport:
            case SkillName.Homingball:
                Skill skll = skills[skill];
                if (Time.time - skll.LastUsed > skll.getCooldown * characterStats[StatName.CDR].CurValue)
                {
                    AimUI.GetComponent<Aim>().ShowUI();
                    //AimUI.SetActive(true);
                    skillBeingCast = skill;
                }
                else {
                    Debug.Log(skill.ToString() + " in cooldown");
                }
                break;
            case SkillName.SelfExplosion:
                SelfExplosionSkill se = skills[skill] as SelfExplosionSkill;
                if (Time.time - se.LastUsed > se.getCooldown * characterStats[StatName.CDR].CurValue)
                {
                    if (characterStats[StatName.HP].CurValue > se.selfDamage)
                    {
                        characterStats[StatName.HP].CurValue -= se.selfDamage;
                        StartCoroutine(LaunchSkill(SkillName.SelfExplosion, Vector3.zero));
                        currentStatus = PlayerMode.Casting;
                    }
                    else {
                        Debug.Log("Not enough HP for SelfExplosion");
                    }
                }
                else {
                    Debug.Log("Self Explosion in cooldown");
                }
                break;
            default:
                break;
        }
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
        hitsReceived = new Dictionary<float, GameObject>();

        //Regen
        InvokeRepeating("HPRegeneration", 1, 1);

        currentStatus = PlayerMode.Idle;
        skillBeingCast = SkillName.None;

        AimUI.transform.parent = null;
    }

    private IEnumerator LaunchSkill(SkillName skill, Vector3 skillDir)
    {
        switch (skill)
        {
            case SkillName.Fireball:
                FireballSkill fb = skills[skill] as FireballSkill;
                fb.LastUsed = Time.time;
                yield return new WaitForSeconds(fb.CastTime * characterStats[StatName.CSpeed].CurValue);
                fb.Launch(gameObject, skillDir, characterStats[StatName.KBPower].CurValue, characterStats[StatName.Damage].CurValue);
                break;
            case SkillName.Teleport:
                TeleportSkill tp = skills[skill] as TeleportSkill;
                tp.LastUsed = Time.time;
                yield return new WaitForSeconds(tp.CastTime * characterStats[StatName.CSpeed].CurValue);
                tp.Launch(gameObject, skillDir);
                break;
            case SkillName.Homingball:
                HomingBallSkill hb = skills[skill] as HomingBallSkill;
                hb.LastUsed = Time.time;
                yield return new WaitForSeconds(hb.CastTime * characterStats[StatName.CSpeed].CurValue);
                hb.Launch(gameObject, skillDir, characterStats[StatName.KBPower].CurValue, characterStats[StatName.Damage].CurValue);
                break;
            case SkillName.SelfExplosion:
                SelfExplosionSkill se = skills[skill] as SelfExplosionSkill;
                se.LastUsed = Time.time;
                yield return new WaitForSeconds(se.CastTime * characterStats[StatName.CSpeed].CurValue);
                se.Launch(gameObject, characterStats[StatName.KBPower].CurValue, characterStats[StatName.Damage].CurValue);
                break;
            case SkillName.None:
                break;
            default:
                break;
        }
        currentStatus = PlayerMode.Idle;
    }

    [ContextMenu("TestSkills")]
    public void TestFunc() {
        Debug.Log("skill count: " + skills.Count);
        Debug.Log("has homing ball: " + skills.ContainsKey(SkillName.Homingball));
    }
}
