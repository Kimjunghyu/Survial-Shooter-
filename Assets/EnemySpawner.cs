using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Enemy prefab1;
    public Enemy prefab2;
    public Enemy prefab3;
    private Enemy[] enemy = new Enemy[3];
    public float spawnTimer = 0f;
    private List<Enemy> enemies = new List<Enemy>();
    public Transform playerTransform;

    public float damage = 20f;
    public float health = 100f;
    public float speed = 3f;

    public void Start()
    {
        enemy[0] = prefab1;
        enemy[1] = prefab2;
        enemy[2] = prefab3;
    }
    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > 1f)
        {
            Spawn();
            spawnTimer = 0f;
        }
    }
    public void Spawn()
    {
        if(enemies.Count <= 10)
        {
            int index = (int)UnityEngine.Random.Range(0, 3);
            var selectedEnemy = enemy[index];

            Vector3 Pos = SetPosition();
            Enemy go = Instantiate(selectedEnemy, Pos, selectedEnemy.transform.rotation);
            go.Setup(health, damage, speed);
            enemies.Add(go);
        }
        else
        {
            foreach(var enemy in enemies)
            {
                if(!enemy.isActiveAndEnabled)
                {
                    enemy.gameObject.transform.position = SetPosition();
                    enemy.Setup(health, damage, speed);
                    enemy.Respawn();
                }
            }
        }
    }  

    private Vector3 SetPosition()
    {
        Vector3 randPos = UnityEngine.Random.insideUnitSphere * 30f + playerTransform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, 30f, NavMesh.AllAreas);
        Vector3 Pos = new Vector3(hit.position.x, 0, hit.position.z);

        return Pos;
    }
}
