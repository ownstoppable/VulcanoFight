using UnityEngine;
using System.Collections;

public class GameOptions : MonoBehaviour {

    public int nRounds;
    public int nEnemies;
    public int nMins;

    public int currentRound;

    public GameObject playerObj;

    private static GameOptions instance;

    public static GameOptions Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    void Awake() {
        nRounds = 3;
        nEnemies = 5;
        nMins = 5;
        currentRound = 0;
    }

	// Use this for initialization
	void Start () {
        instance = this;
        DontDestroyOnLoad(this);
	}

    public void GameOptionsInit(int nR, int nE, int nM) {
        nRounds = nR;
        nEnemies = nE;
        nMins = nM;
    }

    public bool LastRound() {
        return currentRound >= nRounds;
    }
}
