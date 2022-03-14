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

    Animator m_Animator;

    private void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ouch!");
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            life--;
            if(!this.gameObject.tag.Equals("rare")){
                m_Animator.SetTrigger("Explode");
            }
            
            StartCoroutine("dieEnemy");
        }

        if (life == 0)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            EnemyManager.enemyManager.RemoveEnemy(this);
        }
    }

    IEnumerator dieEnemy()
    {
        yield return new WaitForSeconds(5f);
    }

    public void Attack()
    {
        if (checkPath() || !this.gameObject.tag.Equals("canFire"))
        {
            return;
        }
        m_Animator.SetTrigger("Shoot");
        GameObject shot = Instantiate(bullet, shootingOffset.position, Quaternion.identity);
        shot.GetComponent<Bullet>().speed *= -1;
        Destroy(shot, 3f);
        m_Animator.ResetTrigger("Shoot");
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
