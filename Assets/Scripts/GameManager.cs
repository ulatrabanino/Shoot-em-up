using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
//
public class GameManager : MonoBehaviour
{
    public GameObject ship;
    public GameObject barricade;
    private GameObject respawn;

    public TMP_Text scoretxt;
    public TMP_Text livestxt;
    public TMP_Text hiscoretxt;

    private int score = 0;
    private static int highscore = 0;
    private int lives = 3;

    private bool gameStarting = false;

    public static GameManager GM;
    public EnemyManager enemyManager;

    void Awake()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (GM == null)
        {
            DontDestroyOnLoad(this.gameObject);
            GM = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this == null)
        {
            return;
        }

        if (scene.name.Equals("Game"))
        {

            enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

            lives = 3;
            score = 0;

            this.livestxt.text = lives.ToString();
            this.scoretxt.text = score.ToString("D4");

            //InstantiateBarricade();

            enemyManager.Instantiate();
            StartCoroutine("spawnRare");
        }

        if (scene.name.Equals("Menu"))
        { 

            if (score > highscore)
            {
                highscore = score;
            }
            this.hiscoretxt.text = highscore.ToString("D4");
        }
    }

    private void InstantiatePlayer()
    {
        respawn = GameObject.Instantiate(ship, transform);
        respawn.transform.localPosition = new Vector3(10, -1, 0);
    }
    /*
    private void InstantiateBarricade()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject newBarricade = GameObject.Instantiate(barricade, transform);
            newBarricade.transform.localPosition = new Vector3(i * 5, 3, 0);
        }
    }*/

    IEnumerator spawnRare()
    {
        enemyManager.spawnRare();
        yield return new WaitForSeconds(30f);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !gameStarting) gameStart();
    }

    void gameStart()
    {
        gameStarting = true;
        SceneManager.LoadScene("Game");
        Debug.Log("game start");
    }

    public void Scoring(int points)
    {
        score += points;
        this.scoretxt.text = score.ToString("D4");
    }

    public void playerHit()
    {
        lives--;
        this.livestxt.text = lives.ToString();
        if (lives == 0)
        {
            gameOver();
        }
        else InstantiatePlayer();

    }

    public void gameOver()
    {
        lives = 3;
        Debug.Log("game over");
        enemyManager.Clear();
        /*var items = GameObject.FindGameObjectsWithTag("Barricade");
        foreach (var item in items)
        {
            Destroy(item);
        }*/


        StopCoroutine("spawnRare");

        if (respawn != null)
        {
            Destroy(respawn);
        }
    }
}
