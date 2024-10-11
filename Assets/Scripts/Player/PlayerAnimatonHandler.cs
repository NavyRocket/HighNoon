using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimatonHandler : MonoBehaviour
{
    PlayerController controller;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.roll)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !animator.IsInTransition(0))
            {
                controller.roll = false;
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    animator.SetTrigger(Input.GetKey(KeyCode.LeftShift) ? "Run" : "Walk");
                }
                else
                {
                    animator.SetTrigger("Idle");
                }
            }
        }
        else if (controller.draw)
        {
            if (Input.GetMouseButtonUp(1))
            {
                animator.SetTrigger("Draw_R");
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                bool faceRight = transform.position.x <= GameInstance.Instance.CursorWorldPosition().x;
                transform.localRotation = Quaternion.Euler(0f, faceRight ? 0f : 180f, 0f);
                controller.draw = true;
                animator.SetTrigger("Draw");
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                animator.SetBool("Exit", false);
                animator.SetTrigger(Input.GetKey(KeyCode.LeftShift) ? "Run" : "Walk");
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    animator.SetTrigger("Run");
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    animator.SetTrigger("Walk");
                }
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    controller.roll = true;
                    animator.SetTrigger("Roll");
                }
            }
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if ((Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.D)) || (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.A)))
            {
                animator.SetTrigger("Walk");
                transform.localRotation = Quaternion.Euler(0f, Input.GetKey(KeyCode.A) ? 180f : 0f, 0f);
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                animator.SetBool("Exit", true);
            }
        }
        else
        {
            if ((Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.D)) || (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.A)))
            {
                animator.SetTrigger("Run");
                transform.localRotation = Quaternion.Euler(0f, Input.GetKey(KeyCode.A) ? 180f : 0f, 0f);
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                animator.SetBool("Exit", true);
            }
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }

    public void Undraw()
    {
        controller.draw = false;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetTrigger(Input.GetKey(KeyCode.LeftShift) ? "Run" : "Walk");
        }
    }

    public void Aim()
    {
        controller.aim = true;
        controller.gun.SetActive(true);
    }

    public void Unaim()
    {
        controller.aim = false;
        controller.gun.SetActive(false);
    }

    public void ResetExit()
    {
        animator.SetBool("Exit", false);
    }
}
