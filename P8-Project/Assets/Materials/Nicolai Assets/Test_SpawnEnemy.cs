using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SpawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 spawnPoint;
    public GameObject _Enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) {
            Instantiate(_Enemy, spawnPoint, _Enemy.transform.rotation);
        }
        
    }
}
