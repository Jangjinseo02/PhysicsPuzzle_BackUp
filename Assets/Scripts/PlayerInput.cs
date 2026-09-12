using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool isDrag { get; private set; }
    public Rigidbody2D rigid { get; private set; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        isDrag = false;
        rigid.simulated = false;
    }

    private void OnDisable()
    {
        isDrag = false;
    }

    public void Drag()
    {
        isDrag = true;
        rigid.simulated = false;
    }

    public void Drop()
    {
        isDrag = false;
        rigid.simulated = true;
    }

}
