using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public void GameStart()
    {
        SoundManager.Instance.SfxPlay(SoundManager.Sfx.Button);
        StartCoroutine("Load");
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("PlayGround");
    }
}
