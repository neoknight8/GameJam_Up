using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageElement : MonoBehaviour
{
    private bool isDestroyStarted;

    // Use this for initialization
    void Start()
    {
        isDestroyStarted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator DestroyInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isDestroyStarted){
            isDestroyStarted = true;
            StartCoroutine(DestroyInSeconds(1f));
        }
    }
}
