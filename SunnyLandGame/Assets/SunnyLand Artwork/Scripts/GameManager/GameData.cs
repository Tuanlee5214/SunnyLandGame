using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/Data")]
public class GameData : ScriptableObject
{
    public int score1; // cherry
    public int score2; // Kim cương
    public int health; // Máu

    public void AddScore1(int points)
    {
        score1 += points;
    }

    public void AddScore2(int points)
    {
        score2 += points;
    }

    public void MinusHealth(int amount)
    {
        health -= amount;
    }

    public void AddHealth(int amount)
    {
        health += amount;
    }

    public int GetHealth()
    {
        return health;
    }
    public void ResetData()
    {
        score1 = 0;
        score2 = 0;
        health = 8;
    }
}
