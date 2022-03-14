using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class Player : MonoBehaviour
{
    public GameObject bullet;

    public int speed = 1;

    public Transform shottingOffset;
    Animator m_Animator;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    public void gameEnd()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Animator.SetTrigger("Shoot");
            GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
            Debug.Log("Bang!");

            Destroy(shot, 3f);
            m_Animator.ResetTrigger("Shoot");

        }

        float horizontal = Input.GetAxis("Horizontal");
        transform.position += new Vector3(horizontal, 0, 0) * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {
            m_Animator.SetTrigger("Explode");
            StartCoroutine("diePlayer");
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameManager.GM.playerHit();
            Destroy(this.gameObject, 0.7f);
        }
    }

    IEnumerator diePlayer()
    {
        yield return new WaitForSeconds(5f);
    }
}
