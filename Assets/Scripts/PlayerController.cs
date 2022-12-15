using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    Vector3 right = new Vector3(-0.5f, -0.3f, 0);
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("state", 0);
        if (Input.GetKey(KeyCode.D) && this.transform.position != right)
        {
            animator.SetInteger("state", 1);
            this.transform.position += new Vector3(speed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.A) && this.transform.position == right)
        {
            animator.SetInteger("state", 1);
            this.transform.position += new Vector3(-speed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.F))
        {
            animator.SetInteger("state", 2);
        }
    }
}
