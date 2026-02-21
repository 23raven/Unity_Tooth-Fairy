using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // сюда перетаскиваешь Player в Inspector
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z // ⭐ чтобы камера не меняла глубину
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }
}