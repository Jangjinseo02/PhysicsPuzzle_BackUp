using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public PlayerInput playerinput;
    public ParticleSystem effect;
    public int level;
    public bool isMerge;
    public bool isAttach;

    public float deadTime;

    Animator anim;
    CircleCollider2D circle;
    SpriteRenderer sprite;

    private void Awake()
    {
        playerinput = GetComponent<PlayerInput>();
        circle = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        anim.SetInteger("Level", level);
    }
    private void OnDisable()
    {
        ReSpawn();
    }

    void Update()
    {
        if (playerinput.isDrag)
        {
            Move();
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine("AttachRoutine");
    }

    IEnumerator AttachRoutine()
    {
        if (isAttach)
            yield break;

        isAttach = true;
        SoundManager.Instance.SfxPlay(SoundManager.Sfx.Attach);

        yield return new WaitForSeconds(0.4f);

        isAttach = false;
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Dongle")
        {
            Dongle other = collision.gameObject.GetComponent<Dongle>();

            if(level == other.level && !isMerge && !other.isMerge && level < 7)
            {
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = collision.transform.position.x;
                float otherY = collision.transform.position.y;

                if (meY < otherY || (meY == otherY && meX > otherX))
                {
                    //상대 숨기기
                    other.Hide(transform.position);
                    //나의 레벨 증가
                    LevelUp();
                }
            }
            
        }
        
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            deadTime += Time.deltaTime;

            if(deadTime >= 2)
            {
                sprite.color = new Color(0.7f, 0.2f, 0.2f); 
            }
            if (deadTime >= 5)
            {
                GameManager.Instance.GameOver();
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            deadTime = 0;
            sprite.color = Color.white;
        }
    }

    void Move()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float leftPos = -4f + (transform.localScale.x / 2);
        float rightPos = 4f - (transform.localScale.x / 2);

        if (mousePos.x > rightPos)
            mousePos.x = rightPos;
        else if (mousePos.x < leftPos)
            mousePos.x = leftPos;

        mousePos.y = 8f;
        mousePos.z = 0;
        transform.position = Vector3.Lerp(transform.position, mousePos, 0.5f);
    }

    public void Hide(Vector3 target)
    {
        isMerge = true;

        playerinput.rigid.simulated = false;
        circle.enabled = false;
        StartCoroutine(HideRoutine(target));
    }

    IEnumerator HideRoutine(Vector3 target)
    {
        int frameCount = 0;
        while (frameCount < 20)
        {
            frameCount++;

            if(target == Vector3.up * 100)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target, 0.5f);
            }
            yield return null;
        }

        isMerge = false;
        if (target == Vector3.up * 100)
            EffectPlay();
        gameObject.SetActive(false);
        
    }

    void LevelUp()
    {
        isMerge = true;

        playerinput.rigid.velocity = Vector2.zero;
        playerinput.rigid.angularVelocity = 0;

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);
        EffectPlay();
        SoundManager.Instance.SfxPlay(SoundManager.Sfx.LevelUp);

        yield return new WaitForSeconds(0.2f);

        GameManager.Instance.AddScore(level);
        GameManager.Instance.maxLevel = Mathf.Max(++level, GameManager.Instance.maxLevel);
        isMerge = false;
    }

    void EffectPlay()
    {
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;
        effect.Play();
    }

    void ReSpawn()
    {
        //속성 초기화
        level = 0;
        isMerge = false;
        isAttach = false;

        //벡터
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        //물리
        playerinput.rigid.simulated = false;
        playerinput.rigid.velocity = Vector2.zero;
        playerinput.rigid.angularVelocity = 0;
        circle.enabled = true;

    }
}
