using System.Collections;
using uf2;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private RondaManager ronda;
    [SerializeField] private EnemyStateMachine[] pool;
    [SerializeField] private EnemySO[] enemySOs;
    [SerializeField] private int enemicsSpawnTotals;
    [SerializeField] private RondaText rndtxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemicsSpawnTotals = ronda.rnd.enemicsActuals;
        StartCoroutine(rndtxt.changeText());
        StartCoroutine(spawnear());
    }

    IEnumerator spawnear()
    {
        while (true)
        {
            if (ronda.rnd.rondaActual == 1)
            {
                if (enemicsSpawnTotals > 0)
                {
                    for(int i = 0; i < pool.Length; i++)
                    {
                        if (!pool[i].gameObject.activeSelf)
                        {
                            enemicsSpawnTotals--;
                            pool[i].transform.position = new Vector3(Random.Range(-11.75f, 11.75f), Random.Range(-4.20f,1f),0);
                            pool[i]._enemySO = enemySOs[0];
                            pool[i].gameObject.SetActive(true);
                            break;
                        }
                    }
                }
                else if(ronda.rnd.enemicsActuals == 0)
                {
                    StartCoroutine(rndtxt.changeText());
                    ronda.Reset(10);
                    StartCoroutine(rndtxt.changeText());
                    enemicsSpawnTotals = ronda.rnd.enemicsActuals;
                }
            }
            else if(ronda.rnd.rondaActual == 2)
            {
                if (enemicsSpawnTotals > 0)
                {
                    for (int i = 0; i < pool.Length; i++)
                    {
                        if (!pool[i].gameObject.activeSelf)
                        {
                            enemicsSpawnTotals--;
                            pool[i].transform.position = new Vector3(Random.Range(-11.75f, 11.75f), Random.Range(-4.20f, 1f), 0);
                            pool[i]._enemySO = enemySOs[1];
                            pool[i].gameObject.SetActive(true);
                            break;
                        }
                    }
                }
                else if(ronda.rnd.enemicsActuals == 0)
                {
                    ronda.Reset(15);
                    StartCoroutine(rndtxt.changeText());
                    enemicsSpawnTotals = ronda.rnd.enemicsActuals;
                }
            }
            else
            {
                if (enemicsSpawnTotals > 0)
                {
                    for (int i = 0; i < pool.Length; i++)
                    {
                        if (!pool[i].gameObject.activeSelf)
                        {
                            enemicsSpawnTotals--;
                            pool[i].transform.position = new Vector3(Random.Range(-11.75f, 11.75f), Random.Range(-4.20f, 1f), 0);
                            pool[i]._enemySO = enemySOs[Random.Range(0,2)];
                            pool[i].gameObject.SetActive(true);
                            break;
                        }
                    }
                }
                else if (ronda.rnd.enemicsActuals == 0)
                {
                    ronda.Reset(20);
                    StartCoroutine(rndtxt.changeText());
                    enemicsSpawnTotals = ronda.rnd.enemicsActuals;
                }
            }
            yield return new WaitForSeconds(7);
        }
       
    }
}
