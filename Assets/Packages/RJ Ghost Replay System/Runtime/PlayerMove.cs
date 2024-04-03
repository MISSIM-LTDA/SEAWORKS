using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float m_speed = 5f;
    public float m_gravity=10f;
    private CharacterController m_character;
    // Start is called before the first frame update
    void Start()
    {
        m_character = GetComponent<CharacterController>();
    }
    //Move移动控制函数 角色控制器
    void MoveControlByMove()
    {
        float horizontal = Input.GetAxis("Horizontal"); //A D 左右
        float vertical = Input.GetAxis("Vertical"); //W S 上 下
        float moveY = 0;
        moveY -= m_gravity * Time.deltaTime;//重力
        m_character.Move(new Vector3(horizontal, moveY, vertical) * m_speed * Time.deltaTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveControlByMove();
    }

    public LayerMask pushLayers;
    public bool canPush;
    [Range(0.5f, 5f)] public float strength = 1.1f;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (canPush) PushRigidBodies(hit);
    }
    private void PushRigidBodies(ControllerColliderHit hit)
    {
        // make sure we hit a non kinematic rigidbody
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;
        // make sure we only push desired layer(s)
        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & pushLayers.value) == 0) return;
        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f) return;
        // Calculate push direction from move direction, horizontal motion only
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
        // Apply the push and take strength into account
        body.AddForce(pushDir * strength, ForceMode.Impulse);
    }
}
