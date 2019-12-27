using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CurrentStairChangedHandler(Vector3 currentBlockPosition);
public class StaircaseManager : SingletonMonoBehaviour<StaircaseManager>
{
    public const float BLOCK_GENERATION_INTERVAL = 0.15f;

    private int count;
    bool isRandomStepCreating;
    Vector3 initialPostion;
    GameObject lastBlock;
    Vector3 currentBlockPosition;

    CurrentStairChangedHandler OnCurrentStairChanged;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator GenerateStaircase()
    {
        while (true)
        {
            if (isRandomStepCreating)
            {
                Vector3 nextBlockPostion;
                if(lastBlock == null){
                    nextBlockPostion = initialPostion;
                }else{
                    nextBlockPostion = lastBlock.transform.position + new Vector3(0,0.1f,0.35f);
                }
                int randomNumber = Random.Range(1, 100);
                if(randomNumber >=1 && randomNumber <=4){
                    isRandomStepCreating = false;
                    StaircaseGenerator.Instance.StartRoundStairGeneration(nextBlockPostion,
                                                                                      StaircaseGenerator.StairRotation.LEFT,
                                                                                      (block) =>
                                                                                      {
                                                                                          lastBlock = block;
                                                                                          isRandomStepCreating = true;
                                                                                      });
                }else if(randomNumber >= 5 && randomNumber <= 8){
                    isRandomStepCreating = false;
                    StaircaseGenerator.Instance.StartRoundStairGeneration(nextBlockPostion,
                                                                                      StaircaseGenerator.StairRotation.RIGHT,
                                                                                      (block) =>
                                                                                      {
                                                                                          lastBlock = block;
                                                                                          isRandomStepCreating = true;
                                                                                      });
                }else{
                    lastBlock = StaircaseGenerator.Instance.GenerateSingleStep(nextBlockPostion, Vector3.zero);
                }
                count++;
            }
            yield return new WaitForSeconds(BLOCK_GENERATION_INTERVAL);
        }
    }

    public void SetOnCurrentStairChanged(CurrentStairChangedHandler handler){
        OnCurrentStairChanged += handler;
    }

    public void SetCurrentBlock(GameObject gameObject){
        if(gameObject.CompareTag("Staircase")){
            currentBlockPosition = gameObject.transform.position;
            OnCurrentStairChanged(currentBlockPosition);
        }
    }

    public IEnumerator ResetStaircase(){
        isRandomStepCreating = false;
        StaircaseGenerator.Instance.StopRoundStairGeneration();
        foreach (StageElement stageElement in FindObjectsOfType<StageElement>())
        {
            Destroy(stageElement.gameObject);
        }

        Vector3 playerPosition = SetFirstStep(currentBlockPosition) + new Vector3(0,1,0);
        isRandomStepCreating = true;
        yield return new WaitForSeconds(1);
        SetPlayer(playerPosition);
    }

    private Vector3 SetFirstStep(Vector3 currentPosition){
        Vector3 position = new Vector3(0, currentPosition.y, currentPosition.z);
        lastBlock = StaircaseGenerator.Instance.GenerateSingleStep(position, Vector3.zero);
        return position;
    }

    private void SetPlayer(Vector3 position){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = position + new Vector3(0, 1, 0);
        player.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void StartGame(){
        count = 0;
        isRandomStepCreating = true;
        initialPostion = new Vector3(0, 0, 0);
        StartCoroutine(GenerateStaircase());
    }
}
