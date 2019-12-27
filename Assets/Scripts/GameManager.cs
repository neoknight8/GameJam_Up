using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField]
    TextMeshProUGUI scoreText, startText;
    [SerializeField]
    GameObject[] elsas;
    private int HP;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartTextAnimation());
        //HP = 3;
        //StaircaseManager.Instance.SetOnCurrentStairChanged(
        //    (currentBlockPosition) =>
        //    {
        //    ChangeScoreText(currentBlockPosition.y);
        //    }
        //);
        //SetScoreImage();
    }
	// Update is called once per frame
	void Update () {
		
	}

    private void ChangeScoreText(float currentHeight){
        scoreText.SetText("CurrentScore:" + ((int)(currentHeight * 100)).ToString());
    }

    public void DecreaseHP(){
        HP--;
        if(HP <=0){
            GameOver();
        }
        SetScoreImage();
    }


    private void SetScoreImage(){
        for (int i = 1; i <= 3;i++){
            if(i <= HP){
                elsas[i - 1].SetActive(true);
            }else{
                elsas[i - 1].SetActive(false);
            }
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Start");
    }

    public void StartGame(){
        HP = 3;
        StaircaseManager.Instance.SetOnCurrentStairChanged(
            (currentBlockPosition) =>
            {
                ChangeScoreText(currentBlockPosition.y);
            }
        );
        SetScoreImage();
        StaircaseManager.Instance.StartGame();
        GameObject.FindWithTag("Player").GetComponent<Player>().StartGame();
    }

    private IEnumerator StartTextAnimation(){
        yield return new WaitForSeconds(1f);
        startText.SetText(2.ToString());
        yield return new WaitForSeconds(1f);
        startText.SetText(1.ToString());
        yield return new WaitForSeconds(1f);
        startText.SetText("");
        StartGame();

    }
}
