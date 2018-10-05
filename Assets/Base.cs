using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public GameObject Player;
    public NPC[] NPC;
    public GameObject[] SpawnPoints;
    public float SpawnRate = 4f;
    public int MaxNPCs = 8;

    int NPCsActive = 0;
    int NPCsKilled = 0;
    int PlayerHealth = 20;

    void SpawnNewNPC()
    {
        var pos = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        var npcPrefab = NPC[Random.Range(0, NPC.Length)];

        var npc = Instantiate(npcPrefab, pos, Quaternion.identity);
        npc.Activate(Player, gameObject, NPCKilled, PlayerHit);

        NPCsActive += 1;
    }

    int MaxActiveNPCs()
    {
        var toSpawn = NPCsKilled / 4 + 1;

        return System.Math.Min(toSpawn, MaxNPCs);
    }


    IEnumerator Spawn()
    {
        while (true)
        {
            if (NPCsActive < MaxActiveNPCs())
            {
                SpawnNewNPC();
            }
            yield return new WaitForSeconds(Random.value * SpawnRate);
        }
    }

    void NPCKilled()
    {
        NPCsActive -= 1;
        NPCsKilled += 1;
    }

    void PlayerHit()
    {
        PlayerHealth -= 1;

        Debug.LogFormat("health {0}", PlayerHealth);

        if (PlayerHealth <= 0)
        {
            print("game over");
        }
    }

	void Start()
    {
        StartCoroutine(Spawn());
	}
}
