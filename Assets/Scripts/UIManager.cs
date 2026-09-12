using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : SingleTon<UIManager>
{
    public Text scoreText;
    public Text maxScoreText;
    public Text subScoreText;

    public GameObject EndGroup;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);

        maxScoreText.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }

    private void LateUpdate()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }

    public void SetMaxScore()
    {
        int maxScore = Mathf.Max(GameManager.Instance.score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);
    }

    public void GameEndUI()
    {
        subScoreText.text = "Á¡¼ö : " + scoreText.text;
        EndGroup.SetActive(true);
    }

    public void Rtry()
    {
        SoundManager.Instance.SfxPlay(SoundManager.Sfx.Button);

        StartCoroutine("RtryRoutine");
    }

    IEnumerator RtryRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("PlayGround");
    }
}
