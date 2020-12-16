using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject spawnPoint;
    private GameObject unpassed;
    private GameObject passed;
    private GameObject resetSpawn;

    public bool isFinal;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        unpassed = transform.GetChild(0).gameObject;
        passed = transform.GetChild(1).gameObject;
        resetSpawn = transform.GetChild(2).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spawnPoint.transform.position = new Vector3(transform.position.x + 3, 0, 0);
        unpassed.SetActive(false);
        passed.SetActive(true);

        if(isFinal)
        {
            gameManager.score += 100;
            gameManager.ChangeScene(3);
        }
    }
}
