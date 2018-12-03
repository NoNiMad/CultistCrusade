using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionSpawner : MonoBehaviour {
    public Transform SpawnArea;
    public GameObject MinionPrefab;
    public float delay = 5;
    public int howManyPerGroup = 5;
    public int maxCultists = 50;
    public GameObject recenserObject;
    public GameObject playerCharacter;
    public GameObject cemetery;
    public int bonusLife = 0;

    bool inCombat = false;
    bool concentrate = false;
    float timeElapsed;
    MinionRecenser recenser;
    Vector3 lastRayHit;

	void Start () {
        recenser = recenserObject.GetComponent<MinionRecenser>();
        lastRayHit = new Vector3(0, 0, 0);
        timeElapsed = 0;
	}
	
	void Update () {
        Vector3 relativeDest = new Vector3(0, 0, 0);

        timeElapsed += Time.deltaTime;

        if (Input.GetMouseButton(1))
            concentrate = true;
        else
            concentrate = false;
        if (Input.GetMouseButton(0))
        {
            relativeDest = GetGroundRaycast(lastRayHit) - playerCharacter.transform.position;
        }
        else if (inCombat == false)
        {
            if (concentrate)
                relativeDest = playerCharacter.transform.forward * GetCircleSize() * 2;
            else
                relativeDest = playerCharacter.transform.forward * -GetCircleSize();
        }

        foreach (NavMeshAgent agent in GetComponentsInChildren<NavMeshAgent>())
        {
            if (!agent.isActiveAndEnabled) continue;
            else
            {
                if (ShouldSetNewDest(agent.gameObject, agent.destination, relativeDest))
                {
                    agent.destination = relativeDest + GetRelativeRandomDest() + playerCharacter.transform.position;
                }
            }
        }
        CultistSpawning();
    }

    void CultistSpawning() {
        if (timeElapsed > delay && maxCultists - recenser.HowMany() > 0)
        {
            int i = 0;

            if (maxCultists - recenser.HowMany() <= 4)
                i = maxCultists - recenser.HowMany() - 1;
            while (i < howManyPerGroup)
            {
                SpawnMinion();
                i++;
            }
            timeElapsed = 0;
        }
    }

    Vector3 GetGroundRaycast(Vector3 onFail) {
        RaycastHit hit;
        Vector3 groundHit;

        groundHit = onFail;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Default")))
        {
            Targetable target = hit.transform.GetComponent<Targetable>();
            if (target != null)
            {
                if (target.GetSide() == EntitySide.ENNEMY)
                {
                    groundHit = target.transform.position;
                }
            }
            else if (hit.transform.CompareTag("Navigable"))
            {
                groundHit = hit.point;
            }
        }
        lastRayHit = groundHit;
        return groundHit;
    }

    public void SpawnMinion()
    {
        GameObject newMinion = Instantiate(MinionPrefab, transform);
        Minion minion = newMinion.GetComponent<Minion>();
        HealthSystem hs = minion.GetComponent<HealthSystem>();
        if (hs != null)
        {
            hs.maxHealth += bonusLife;
            hs.health = hs.maxHealth;
        }
        minion.recenserObject = this.gameObject;
        minion.cemetery = this.cemetery;
        newMinion.transform.position = SpawnArea.position + Vector3.up * 0.5f;
        newMinion.GetComponent<NavMeshAgent>().destination = GetRelativeRandomDest() + playerCharacter.transform.position;
    }

    public bool ShouldSetNewDest(GameObject minion, Vector3 destination, Vector3 relativeDest) {
        return ((destination - (relativeDest + playerCharacter.transform.position)).magnitude < GetCircleSize()) == false;
    }

    public Vector3 GetRelativeRandomDest()
    {
        Vector3 inUnitCircle = Random.insideUnitCircle;

        inUnitCircle.z = inUnitCircle.y;
        inUnitCircle.y = 0;
        return inUnitCircle * GetCircleSize();
    }

    float GetCircleSize()
    {
        float size = (Mathf.Log10(transform.childCount) * 2) + 0.5f + (0.05f * transform.childCount);
        if (concentrate)
            size = size / 2;
        return (size);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(lastRayHit, GetCircleSize());
        foreach (NavMeshAgent agent in GetComponentsInChildren<NavMeshAgent>())
        {
            Gizmos.DrawLine(agent.transform.position, agent.destination);
        }
    }
}
