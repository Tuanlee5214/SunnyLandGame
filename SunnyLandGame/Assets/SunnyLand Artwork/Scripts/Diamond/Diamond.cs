using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public GameObject fx;
    private GameManager gameManager;
    private AudioManager audioManager;
    
    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameObject fx1 = Instantiate(fx, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Destroy(fx1, 0.5f);
            audioManager.PlayItemSound();
            gameManager.AddScore2(1);
            Debug.Log("Chạm vào diamond");
        }
    }
}
