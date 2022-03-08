using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootingOffset;

    public int life = 1;
    public int points = 10;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            life--;
        }

        if (life == 0)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            EnemyManager.enemyManager.RemoveEnemy(this);
        }
    }

    public void Attack()
    {
        if (checkPath() || !this.gameObject.tag.Equals("canFire"))
        {
            return;
        }

        GameObject shot = Instantiate(bullet, shootingOffset.position, Quaternion.identity);
        shot.GetComponent<Bullet>().speed *= -1;
        Destroy(shot, 1f);
    }

    public void moveForward(int x, int y, float speed)
    {
        if (this.gameObject.name.Equals("rare"))
        {
            x = 1;
        }
        transform.position += new Vector3(x, y, 0) * speed * Time.deltaTime;
    }

    bool checkPath()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingOffset.position, Vector2.down);

        if (hit)
        {
            if (hit.collider.gameObject.tag.Equals("Enemy")) return true;
        }

        return false;
    }
}
