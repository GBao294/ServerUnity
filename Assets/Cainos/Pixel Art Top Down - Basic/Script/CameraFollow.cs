using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform target; // Tham chiếu đến transform của người chơi

    private void LateUpdate()
    {
        if (target != null)
        {
            // Cập nhật vị trí của camera theo vị trí của người chơi
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}