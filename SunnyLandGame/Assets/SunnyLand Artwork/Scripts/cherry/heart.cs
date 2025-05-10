using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class heart : MonoBehaviour
{
    public GameObject fx;
    public GameData gameData;
    private GameManager gameManager;
    private AudioManager audioManager;
    public TextMeshProUGUI healthText;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject fx1 = Instantiate(fx, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Destroy(fx1, 0.5f);
            audioManager.PlayItemSound();
            gameData.AddHealth(1);
            healthText.text = gameData.GetHealth().ToString();
            Debug.Log("Chạm vào tym");
        }
    }
}
