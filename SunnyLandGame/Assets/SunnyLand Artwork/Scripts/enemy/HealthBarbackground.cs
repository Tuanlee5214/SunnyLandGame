using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill; // Kéo Image `HealthBarFill` vào đây

    public void SetMaxHealth(float health)
    {
        healthBarFill.fillAmount = 1; // Lúc đầu đầy thanh máu
    }

    public void SetHealth(float health)
    {
        healthBarFill.fillAmount = health / 100f; // Cập nhật thanh máu dựa trên % máu còn lại
    }
}
