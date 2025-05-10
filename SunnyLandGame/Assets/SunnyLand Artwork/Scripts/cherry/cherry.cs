using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cherry : MonoBehaviour
{
    public GameObject fx;
    private GameManager gameManager;
    private AudioManager audioManager;
    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject fx1 = Instantiate(fx, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            audioManager.PlayItemSound();
            Destroy(fx1, 0.5f);
            gameManager.AddScore1(1);
            Debug.Log("chạm vào cherry");
        }
    }
}
