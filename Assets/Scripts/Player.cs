using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const float SPEED = 0.1f;
    private bool isStarted;

    // Use this for initialization
    void Start()
    {
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted){
            Move();
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.Translate(new Vector3(0, 0, SPEED));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.Rotate(new Vector3(0, -3, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.Rotate(new Vector3(0, 3, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isStarted){
            if (collision.gameObject.CompareTag("Staircase"))
            {
                StaircaseManager.Instance.SetCurrentBlock(collision.gameObject);
            }
            if (collision.gameObject.CompareTag("Ground"))
            {
                GameManager.Instance.DecreaseHP();
                StartCoroutine(StaircaseManager.Instance.ResetStaircase());
            }
        }
    }

    public void StartGame(){
        isStarted = true;
    }
}
