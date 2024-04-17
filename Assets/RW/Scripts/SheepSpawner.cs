using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    public bool canSpawn = true;
    public GameObject sheepPrefab;
    public List<Transform> sheepSpawnPositions = new List<Transform>();
    public float timeBetweenSpawns;
    private List<GameObject> sheepList = new List<GameObject>();
    private float increasedRunSpeed;
    private float baseRunSpeed;
    private float speedIncreaseRate = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
        baseRunSpeed = sheepPrefab.GetComponent<Sheep>().runSpeed;
        increasedRunSpeed = baseRunSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnSheep()
    {
        Vector3 randomPosition = sheepSpawnPositions[Random.Range(0, sheepSpawnPositions.Count)].position;
        GameObject sheep = Instantiate(sheepPrefab, randomPosition, sheepPrefab.transform.rotation);
        sheepList.Add(sheep);
        Sheep sheepComponent = sheep.GetComponent<Sheep>();
        baseRunSpeed = sheepComponent.runSpeed;

        if (sheepComponent != null)
        {
            sheepComponent.SetSpawner(this);
            sheepComponent.runSpeed = increasedRunSpeed;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (canSpawn)
        {
            SpawnSheep();
            yield return new WaitForSeconds(timeBetweenSpawns);
            increasedRunSpeed += speedIncreaseRate;
            if(timeBetweenSpawns >= 1f)
            {
                timeBetweenSpawns -= 0.01f;
            }
        }
    }

    public void RemoveSheepFromList(GameObject sheep)
    {
        sheepList.Remove(sheep);
    }
}
