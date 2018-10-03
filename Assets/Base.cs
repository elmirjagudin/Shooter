using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public GameObject Player;
    public NPC NPC;
    public GameObject[] SpawnPoints;
    public float SpawnRate = 4f;
    public int MaxNPCs = 8;
    public int NPCsSpawned = 0;

    void SpawnNewNPC()
    {
        var pos = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        var npc = Instantiate(NPC, pos, Quaternion.identity);
        npc.Activate(Player, gameObject, NPCKilled);

        NPCsSpawned += 1;
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (NPCsSpawned < MaxNPCs)
            {
                SpawnNewNPC();
            }
            yield return new WaitForSeconds(SpawnRate);
        }
    }

    void NPCKilled()
    {
        print("fatality");
        NPCsSpawned -= 1;
    }

	void Start()
    {
        StartCoroutine(Spawn());
	}
}
