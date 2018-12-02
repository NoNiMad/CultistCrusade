using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionSpawner : MonoBehaviour {
    public Transform SpawnArea;
    public GameObject MinionPrefab;
    public float delay;

    float timeElapsed;
    bool hasDest;
    Vector3 dest;

	// Use this for initialization
	void Start () {
        hasDest = false;
        timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > delay)
        {
            SpawnMinion();
            timeElapsed = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Targetable target = hit.transform.GetComponent<Targetable>();
                if (target != null)
                {
                    if (target.GetSide() == EntitySide.ENNEMY)
                    {
                        hasDest = true;
                        dest = target.transform.position;
                    }
                } else if (hit.transform.CompareTag("Navigable"))
                {
                    hasDest = true;
                    dest = hit.point;
                }
            }
        }

        foreach (NavMeshAgent agent in GetComponentsInChildren<NavMeshAgent>())
        {
            if (!agent.isActiveAndEnabled) continue;
            if (!hasDest)
            {
                agent.destination = agent.transform.localPosition;
            } else
            {
                agent.destination = dest;
            }
        }
    }

    public void SpawnMinion()
    {
        GameObject newMinion = Instantiate(MinionPrefab, transform);
        newMinion.transform.position = SpawnArea.position + Vector3.up * 0.5f;
    }
}
