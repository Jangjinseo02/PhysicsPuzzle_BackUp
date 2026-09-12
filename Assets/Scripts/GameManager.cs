using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    [Header("---------------------Core")]
    public bool IsGameOver;
    public int score;
    public int maxLevel;


    [Header("---------------------Dongle")]
    public Dongle lastDongle;

   

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        StartCoroutine("GameStart");
    }
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1.5f);
        NextDongle();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
    }

    Dongle GetDongle()
    {
        return ObjectManager.Instance.isPoolDongle();
    }

    void NextDongle()
    {
        if (IsGameOver)
            return;

        Dongle newDongle = GetDongle();
        lastDongle = newDongle;

        lastDongle.level = Random.Range(0, maxLevel);
        lastDongle.gameObject.SetActive(true);
        SoundManager.Instance.SfxPlay(SoundManager.Sfx.Next);

        StartCoroutine("WaitDongle");
    }

    IEnumerator WaitDongle()
    {
        while (lastDongle != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);

        NextDongle();
    }


    public void Drag()
    {
        if (lastDongle == null)
            return;

        lastDongle.playerinput.Drag();
    }

    public void Drop()
    {
        if (lastDongle == null)
            return;

        lastDongle.playerinput.Drop();
        lastDongle = null;
    }

    public void AddScore(int level)
    {
        score += (int)Mathf.Pow(2f, level);
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        StartCoroutine("GameOverRoutine");
    }

    IEnumerator GameOverRoutine()
    {
        Dongle[] dongles = GameObject.FindObjectsOfType<Dongle>();

        for (int i = 0; i < dongles.Length; i++)
        {
            dongles[i].playerinput.rigid.simulated = false;
        }

        for (int i = 0; i < dongles.Length; i++)
        {
            dongles[i].Hide(Vector3.up * 100);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        UIManager.Instance.SetMaxScore();
        UIManager.Instance.GameEndUI();

        SoundManager.Instance.BGMStop();
        SoundManager.Instance.SfxPlay(SoundManager.Sfx.Over);
    }
}
