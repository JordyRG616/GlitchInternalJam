using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemyTemplates;
    [SerializeField] private Vector2Int waveAmountMinMax;
    [SerializeField] private Vector2 intervalBetweenWaves;
    [Space]
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnCircleRadius;
    
    private UnityPool<Enemy> pool;
    private WaitForSeconds spawnWait = new WaitForSeconds(.1f);

    private IEnumerator Start()
    {
        yield return null;
        pool = new UnityPool<Enemy>(enemyTemplates, 20);

        StartCoroutine(ManageWaves());
    }

    private IEnumerator ManageWaves()
    {
        
        while (gameObject.activeInHierarchy)
        {
            yield return StartCoroutine(SpawnWave());
            
            var interval = Random.Range(intervalBetweenWaves.x, intervalBetweenWaves.y);
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator SpawnWave()
    {
        var waveAmount = Random.Range(waveAmountMinMax.x, waveAmountMinMax.y);
        var point = Random.insideUnitCircle.normalized * spawnRadius;
        point += (Vector2)transform.position;

        for (int i = 0; i < waveAmount; i++)
        {
            var rdmPos = Random.insideUnitCircle * spawnCircleRadius;
            var pos = point + rdmPos;
            
            var enemy = pool.Get();
            enemy.transform.position = pos;

            enemy.OnDeathAnimationDone += ReturnToPool;

            enemy.gameObject.SetActive(true);

            yield return spawnWait;
        }
    }

    private void ReturnToPool(Enemy enemy)
    {
        enemy.OnDeathAnimationDone -= ReturnToPool;        
        enemy.gameObject.SetActive(false);
        pool.Return(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
