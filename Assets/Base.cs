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

    int NPCsSpawned = 0;
    int PlayerHealth = 20;


    void SpawnNewNPC()
    {
        var pos = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        var npc = Instantiate(NPC, pos, Quaternion.identity);
        npc.Activate(Player, gameObject, NPCKilled, PlayerHit);

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
        NPCsSpawned -= 1;
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
