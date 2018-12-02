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

	void Start () {
        hasDest = false;
        timeElapsed = 0;
	}
	
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

                foreach (NavMeshAgent agent in GetComponentsInChildren<NavMeshAgent>())
                {
                    if (!agent.isActiveAndEnabled) continue;
                    if (!hasDest)
                    {
                        agent.destination = agent.transform.localPosition;
                    }
                    else
                    {
                        agent.destination = GetRandomDest();
                    }
                }
            }
        }
    }

    public void SpawnMinion()
    {
        GameObject newMinion = Instantiate(MinionPrefab, transform);
        newMinion.transform.position = SpawnArea.position + Vector3.up * 0.5f;
        newMinion.GetComponent<NavMeshAgent>().destination = GetRandomDest();
    }

    public Vector3 GetRandomDest()
    {
        Vector3 inUnitSphere = Random.insideUnitSphere;
        inUnitSphere.y = 0;
        return dest + inUnitSphere * Mathf.Log10(transform.childCount) * 2;
    }

    /*public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(dest, Mathf.Log10(transform.childCount) * 2);
        foreach (NavMeshAgent agent in GetComponentsInChildren<NavMeshAgent>())
        {
            Gizmos.DrawLine(agent.transform.position, agent.destination);
        }
    }*/
}
