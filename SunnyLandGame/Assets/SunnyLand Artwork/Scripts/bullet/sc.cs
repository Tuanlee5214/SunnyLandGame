using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc : MonoBehaviour
{
    public GameObject fx2;
    private AudioManager audiomanager;
    private Playercontroller playercontroller;
    private float previousX;
    Vector3 direction;
    private void Start()
    {
        audiomanager = FindAnyObjectByType<AudioManager>();
        playercontroller = FindObjectOfType<Playercontroller>();
        direction = (playercontroller.transform.position - transform.position).normalized;
        Flip();
    }

    private void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
            GameObject fx1 = Instantiate(fx2, transform.position, Quaternion.identity);
            Destroy(fx1, 0.5f);
            audiomanager.PlayKillEnemySound();
        }
    }

    void Flip()
    {
        Vector3 flip = playercontroller.transform.position - this.transform.position;
        float angle = Mathf.Atan2(flip.y, flip.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle - 180f);
    }
    


}
