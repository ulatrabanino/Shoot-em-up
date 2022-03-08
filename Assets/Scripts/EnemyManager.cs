using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class EnemyManager : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemyR;
    private List<Enemy> enemies = new List<Enemy>();
    public float speed = 1;
    private int x = 1;
    private int y = 0;

    public static EnemyManager enemyManager;


    void Awake()
    {
        enemyManager = this;
    }

    public void Instantiate()
    {
        Debug.Log("enemies spawn");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                spawnEnemy(i, new Vector3(j * -2f, -i * -1.5f, -1));
                //spawnEnemy(i, new Vector3(j * -3.68f, -i * 1.75f, -1));
            }
        }
        StartCoroutine("TryFire");
    }

    IEnumerator TryFire()
    {
        while (enemies.Count > 0)
        {
            int index = Random.Range(0, enemies.Count - 1);
            enemies[index].Attack();

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void spawnEnemy(int type, Vector3 positionToSpawn)
    {
        GameObject toSpawn;
        switch (type)
        {
            case 0: toSpawn = enemy3; break;
            case 1: toSpawn = enemy2; break;
            case 2: toSpawn = enemy1; break;
            default: return;
        }

        toSpawn = GameObject.Instantiate(toSpawn, transform);
        toSpawn.transform.localPosition = positionToSpawn;
        enemies.Add(toSpawn.GetComponent<Enemy>());
    }

    public void spawnRare()
    {
        Debug.Log("rare!");
        GameObject rare = GameObject.Instantiate(enemyR, transform);
        rare.transform.position = new Vector3(-9.63f, 4.61f, 0);
        rare.GetComponent<Rigidbody2D>().velocity = new Vector3(2, 0, 0);
        Destroy(rare, 20f);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        //gm is gamemanager
        if (enemies.Contains(enemy)) enemies.Remove(enemy);

        GameManager.GM.Scoring(enemy.points);
        Destroy(enemy.gameObject, 0.8f);
        speed += 0.1f;

        if (enemies.Count == 0)
        {
            GameManager.GM.gameOver();
        }
    }

    void FixedUpdate()
    {
        foreach (var enemy in enemies)
        {
            enemy.moveForward(x, y, speed);
            if (enemy.transform.position.x > 10)
            {
                x = -1;
                Directions();
            }
            if (enemy.transform.position.x < -10)
            {
                x = 1;
                Directions();
            }
        }
    }

    public void Directions()
    {
        transform.position += new Vector3(0, -1, 0);
    }

    public void Clear()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        enemies.Clear();
        transform.position = new Vector3(-5, 6, 0);
        StopCoroutine("TryFire");
    }
}
