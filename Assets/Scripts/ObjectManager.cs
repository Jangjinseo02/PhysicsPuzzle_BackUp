using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingleTon<ObjectManager>
{
    [Header("---------------------Dongle")]
    public List<Dongle> donglesPool;
    public GameObject donglePrefab;
    public Transform donglePos;

    [Header("---------------------Effect")]
    public List<ParticleSystem> effectPool;
    public GameObject effectPrefab;
    public Transform effectPos;

    [Header("---------------------Pool")]
    [Range(0,10)]
    public int poolSize;
    public int poolCursor;

    private void Awake()
    {
        donglesPool = new List<Dongle>();
        effectPool = new List<ParticleSystem>();

        for (int i = 0; i < poolSize; i++) 
            MakeDongle();
    }

    public Dongle MakeDongle()
    {
        //이펙트 생성
        GameObject instantEffect = Instantiate(effectPrefab, effectPos);
        instantEffect.name = "Effect " + effectPool.Count;
        ParticleSystem effect = instantEffect.GetComponent<ParticleSystem>();
        effectPool.Add(effect);

        //동글 생성
        GameObject instant = Instantiate(donglePrefab, donglePos);
        instant.name = "Dongle " + donglesPool.Count;
        Dongle dongle = instant.GetComponent<Dongle>();
        dongle.effect = effect;
        donglesPool.Add(dongle);

        return dongle;
    }

    public void NextCursor()
    {
        poolCursor = (poolCursor + 1) % donglesPool.Count;
    }

    public Dongle isPoolDongle()
    {
        for (int i = 0; i < donglesPool.Count; i++)
        {
            NextCursor();
            if (!donglesPool[poolCursor].gameObject.activeSelf)
            {
                return donglesPool[poolCursor];
            }
        }
        
        return MakeDongle();
    }


}
