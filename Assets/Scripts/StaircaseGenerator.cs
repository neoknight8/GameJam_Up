using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RoundStaircaseGenerationHander(GameObject lastBlock);
public class StaircaseGenerator : SingletonMonoBehaviour<StaircaseGenerator>
{
    public enum StairRotation
    {
        LEFT,
        RIGHT
    }

    private IEnumerator roundStairGeneration;

    [SerializeField]
    GameObject Prehab;

    public GameObject GenerateSingleStep(Vector3 position, Vector3 rotation)
    {
        return Instantiate(Prehab, position, Quaternion.Euler(rotation));
    }

    public void StartRoundStairGeneration(Vector3 startPosition, StairRotation rotation, RoundStaircaseGenerationHander onRoundStaircaseGenerationFinished){
        roundStairGeneration = GenerateRoundStaircase(startPosition, rotation, onRoundStaircaseGenerationFinished);
        StartCoroutine(roundStairGeneration);
    }

    public IEnumerator GenerateRoundStaircase(Vector3 startPosition,StairRotation rotation, RoundStaircaseGenerationHander onRoundStaircaseGenerationFinished)
    {
        float radius = 1.5f;
        GameObject lastBlock = null;
        for (int i = 0; i <= 24; i++)
        {
            if(rotation == StairRotation.LEFT){
                lastBlock = GenerateSingleStep(startPosition - new Vector3(radius, 0, 0) + new Vector3(radius * Mathf.Cos((Mathf.PI / 180) * i * 15), i * 0.1f, radius * Mathf.Sin((Mathf.PI / 180) * i * 15)), new Vector3(0, -i * 15, 0));
            }else if(rotation == StairRotation.RIGHT){
                lastBlock = GenerateSingleStep(startPosition + new Vector3(radius, 0, 0) + new Vector3(radius * Mathf.Cos((Mathf.PI / 180) * (180 - i * 15)), i * 0.1f, radius * Mathf.Sin((Mathf.PI / 180) * (180 - i * 15))), new Vector3(0, i * 15, 0));
            }
            yield return new WaitForSeconds(StaircaseManager.BLOCK_GENERATION_INTERVAL);
        }
        onRoundStaircaseGenerationFinished(lastBlock);
    }

    public void StopRoundStairGeneration(){
        if(roundStairGeneration != null){
            StopCoroutine(roundStairGeneration);
        }
    }
}
