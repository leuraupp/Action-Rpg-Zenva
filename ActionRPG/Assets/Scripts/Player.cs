using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int maxHp;
    public int curHp;

    public float moveSpeed;
    public float jumpForce;

    public float attackRange;
    public int damage;
    private bool isAttacking;

    public Rigidbody rig;
    public Animator anim;

    private void Update() {
        Move();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        if (Input.GetMouseButton(0) && !isAttacking) {
            Attack();
        }

        if (!isAttacking) {
            UpdateAnimations();
        }
    }

    private void UpdateAnimations() {
        anim.SetBool("MovingForwards", false);
        anim.SetBool("MovingBack", false);
        anim.SetBool("MovingLeft", false);
        anim.SetBool("MovingRight", false);

        Vector3 localVel = transform.InverseTransformDirection(rig.velocity);

        if (localVel.z > 0.1) {
            anim.SetBool("MovingForwards", true);
        } else if (localVel.z < -0.1) {
            anim.SetBool("MovingBack", true);
        } else if (localVel.x > 0.1) {
            anim.SetBool("MovingRight", true);
        } else if (localVel.x < -0.1) {
            anim.SetBool("MovingLeft", true);
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

    public void TakeDamage(int damage) {
        curHp -= damage;

        if (curHp <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Attack() {
        isAttacking = true;
        anim.SetTrigger("Attack");

        Invoke("TryDamage", 0.7f);
        Invoke("DisableIsAttacking", 1.5f);
    }

    private void TryDamage() {
        Ray ray = new Ray(transform.position + transform.forward, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, attackRange, attackRange, 1 << 6);

        foreach (RaycastHit hit in hits) {
            hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }

    private void DisableIsAttacking() {
        isAttacking = false;
    }
}
