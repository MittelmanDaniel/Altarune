using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Shinobi_ZigHitbox : MonoBehaviour
{
    [SerializeField] private float detonateDuration = 0.3f;
    [SerializeField] private Material flashMat;

    private float elapsedTime;
    private bool shouldDmg = false;
    private HashSet<Entity> damagedEntities = new HashSet<Entity>();

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 2f)
        {
            Destroy(gameObject);
        }
    }

    public void Detonate()
    {
        StartCoroutine(IDetonate());
    }

    private IEnumerator IDetonate()
    {
        gameObject.GetComponent<Renderer>().material = flashMat;
        shouldDmg = true;

        yield return new WaitForSeconds(detonateDuration);

        Destroy(gameObject);
    }

    private void DamageEntity(Collider other)
    {
        if (shouldDmg && other.TryGetComponent(out Entity entity) && entity.Faction != EntityFaction.Hostile && !damagedEntities.Contains(entity))
        {
            entity.TryDamage(4);
            damagedEntities.Add(entity);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        DamageEntity(other);
    }
}
