using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    [Header("---------------------Audio")]
    public AudioSource BGM_Player;
    public AudioSource[] SFX_Player;
    public AudioClip[] sfx;

    public enum Sfx { LevelUp, Next, Attach, Button, Over };
    int sfxCursor;



    void Start()
    {
        BGM_Player.Play();
    }

    public void SfxPlay(Sfx type)
    {
        switch (type)
        {
            case Sfx.LevelUp:
                SFX_Player[sfxCursor].clip = sfx[Random.Range(0, 3)];
                break;
            case Sfx.Next:
                SFX_Player[sfxCursor].clip = sfx[3];
                break;
            case Sfx.Attach:
                SFX_Player[sfxCursor].clip = sfx[4];
                break;
            case Sfx.Button:
                SFX_Player[sfxCursor].clip = sfx[5];
                break;
            case Sfx.Over:
                SFX_Player[sfxCursor].clip = sfx[6];
                break;
        }

        SFX_Player[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % SFX_Player.Length;
    }

    public void BGMStop()
    {
        BGM_Player.Stop();
    }

}
