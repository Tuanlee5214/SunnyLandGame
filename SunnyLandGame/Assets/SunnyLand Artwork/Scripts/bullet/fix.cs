using UnityEngine;

public class RemoveAnimationEvents : MonoBehaviour
{
    public AnimationClip animationClip;

    void Start()
    {
        if (animationClip != null)
        {
            animationClip.events = new AnimationEvent[0]; // Xóa toàn bộ sự kiện trong animation
            Debug.Log("Đã xóa tất cả Animation Events!");
        }
    }
}
