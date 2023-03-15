using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public Rigidbody rig;

    private void Update() {
        Move();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
    }

    private void Move() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * x + transform.forward * z;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;

        rig.velocity = dir;
    }

    private void Jump() {
        Debug.Log("pulou?");
        if (CanJump()) {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool CanJump() {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.1f)) {
            Debug.Log(hit.collider);
            return hit.collider != null;
        }

        return false;
    }
}
