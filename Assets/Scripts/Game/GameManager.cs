using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject enemiesGroup;
    private GameOptions gOptions;
    private float timeGameStart;

    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    void Awake() {
        instance = this;
        timeGameStart = -1;
        Application.runInBackground = true;
        if (GameObject.FindGameObjectWithTag("EnemiesGroup") == null)
        {
            enemiesGroup = new GameObject("EnemiesGroup") { tag = "EnemiesGroup" };
            DontDestroyOnLoad(enemiesGroup);
        }
        else enemiesGroup = GameObject.FindGameObjectWithTag("EnemiesGroup");

        
        if (!GameObject.FindGameObjectWithTag("GameOptions"))
        {
            GameObject gO = new GameObject();
            gOptions = gO.AddComponent<GameOptions>();
            gO.tag = "GameOptions";
            gO.name = "GameOptions";
        }
        else gOptions = GameObject.FindGameObjectWithTag("GameOptions").GetComponent<GameOptions>();


    }
    
	// Use this for initialization
	void Start () {
        List<BaseCharacter> characters = new List<BaseCharacter>();
        //Only create bots on the first round
        if (gOptions.currentRound == 0)
        {
            for (int i = 0; i < gOptions.nEnemies; i++)
            {
                GameObject bot = Instantiate(enemyPrefab, new Vector3(Random.Range(-20, 21), 0.5f, Random.Range(-20, 21)), Quaternion.identity) as GameObject;
                bot.transform.parent = enemiesGroup.transform;
                bot.name = "Bot " + (i + 1);
                bot.GetComponent<BaseCharacter>().attackPossible = false;
                characters.Add(bot.GetComponent<BaseCharacter>());
            }

            GameObject player = Instantiate(playerPrefab, new Vector3(Random.Range(-20, 21), 0.5f, Random.Range(-20, 21)), Quaternion.identity) as GameObject;
            player.name = "Player";
            gOptions.playerObj = player;
            DontDestroyOnLoad(player);
            player.GetComponent<BaseCharacter>().attackPossible = false;
            characters.Add(player.GetComponent<BaseCharacter>());
        }
        else
        {
            for (int i = 0; i < enemiesGroup.transform.childCount; i++)
            {
                AI botAI = enemiesGroup.transform.GetChild(i).GetComponent<AI>();
                botAI.gameObject.transform.position = new Vector3(Random.Range(-20, 21), 0.5f, Random.Range(-20, 21));
                botAI.gameObject.SetActive(true);
                botAI.InitRound();
                botAI.attackPossible = false;
                characters.Add(botAI);
            }

            PlayerController playerC = GameOptions.Instance.playerObj.GetComponent<PlayerController>();
            playerC.gameObject.transform.position = new Vector3(Random.Range(-20, 21), 0.5f, Random.Range(-20, 21));
            playerC.gameObject.SetActive(true);
            playerC.InitRound();
            playerC.attackPossible = false;
            characters.Add(playerC);
        }

        StartCoroutine(RoundInitCD(characters));
	}

    void Update() {
        if (GetTimePassed() != -1 && GetTimePassed() >= GameOptions.Instance.nMins * 60) {

            List<GameObject> characters = new List<GameObject>(GetAliveCharacters());

            Time.timeScale = 1;
            GameObject victTextObj = Resources.Load("UI/VictoriousText") as GameObject;
            GameObject victText = Instantiate(victTextObj, victTextObj.transform.position, Quaternion.identity) as GameObject;
            victText.GetComponent<GUIText>().text = "Round ended in a draw";
            Destroy(victText, 4);
            if (characters.Contains(GameOptions.Instance.playerObj)) GameOptions.Instance.playerObj.GetComponent<PlayerController>().AimUI.transform.parent = GameOptions.Instance.playerObj.transform;
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].GetComponent<BaseCharacter>().GoIdle();
            }
            FinishRoundI();
            timeGameStart = -1;
        }
    }

    public float GetTimePassed()
    {
        if (timeGameStart == -1) return -1;
        return Time.time - timeGameStart;
    }

    private IEnumerator RoundInitCD(List<BaseCharacter> characters)
    {
        GameObject cdTextObj = Resources.Load("UI/CDText") as GameObject;
        GameObject cdText = Instantiate(cdTextObj, cdTextObj.transform.position, Quaternion.identity) as GameObject;
        cdText.GetComponent<GUIText>().text = "3";
        yield return new WaitForSeconds(1);
        cdText.GetComponent<GUIText>().text = "2";
        yield return new WaitForSeconds(1);
        cdText.GetComponent<GUIText>().text = "1";
        yield return new WaitForSeconds(1);
        cdText.GetComponent<GUIText>().text = "GO";
        timeGameStart = Time.time;
        Destroy(cdText, 1);
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].attackPossible = true;
        }
    }

    public void FinishRoundI() {
        GameOptions.Instance.currentRound++;
        if (GameOptions.Instance.LastRound()) Invoke("FinishGame", 5);
        else Invoke("FinishRound", 5);
    }

    private void FinishRound() {
        Application.LoadLevel(2);
    }

    private void FinishGame() {
        List<BaseCharacter> characters = new List<BaseCharacter>();

        for (int i = 0; i < enemiesGroup.transform.childCount; i++)
        {
            characters.Add(enemiesGroup.transform.GetChild(i).GetComponent<BaseCharacter>());
        }
        characters.Add(GameOptions.Instance.playerObj.GetComponent<BaseCharacter>());

        GameObject victTextObj = Resources.Load("UI/VictoriousText") as GameObject;
        GameObject victText = Instantiate(victTextObj, victTextObj.transform.position, Quaternion.identity) as GameObject;
        victText.GetComponent<GUIText>().text = characters.OrderByDescending(x => x.GetBaseStat(StatName.Kills).CurValue).First().name + " wins the game";
        Destroy(victText, 4);
    }

    public List<GameObject> GetAliveCharacters() {
        List<GameObject> characters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        if (GameObject.FindGameObjectWithTag("Player") != null)
            characters.Add(GameObject.FindGameObjectWithTag("Player"));

        return characters;
    }
}
