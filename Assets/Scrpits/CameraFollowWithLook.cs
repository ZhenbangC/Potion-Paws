using UnityEngine;

public class CameraFollowWithLook : MonoBehaviour
{
    public Rigidbody2D targetRb;
    public Vector2 baseOffset = Vector2.zero;
    public float lookOffsetY = 2f;
    public float smoothSpeed = 5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (targetRb == null) return;

        // 使用 Rigidbody2D.position，而不是 Transform.position
        Vector2 targetPosition2D = targetRb.position;
        Vector3 offset = new Vector3(baseOffset.x, baseOffset.y, 0f);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            offset.y += lookOffsetY;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            offset.y -= lookOffsetY;
        }

        Vector3 targetPos = new Vector3(targetPosition2D.x, targetPosition2D.y, transform.position.z) + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1f / smoothSpeed);
    }
}