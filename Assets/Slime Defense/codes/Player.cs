using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    private Vector3 moveVector = Vector3.zero;
    public float speed;

    Rigidbody2D rigid;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        moveVector.x = inputVec.x;
        moveVector.y = inputVec.y;
        
        if (inputVec.x > 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (inputVec.x < 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        if (inputVec.x != 0 || inputVec.y != 0)
        {
            animator.SetFloat("RunState", 0.5f);
        }
        else
        {
            animator.SetFloat("RunState", 0);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Die");
        }
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
}
