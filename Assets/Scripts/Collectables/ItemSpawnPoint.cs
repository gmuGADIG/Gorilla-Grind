using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    [SerializeField]
    public GameObject itemToSpawn;

    [SerializeField]
    public float spawnDelta;

    [SerializeField]
    [Range(1, 10)]
    public int spawnAmount;


    //Gets the associated ground edge
    public GroundEdge groundEdge => transform.parent ? transform.parent.GetComponent<GroundEdge>() : null;

    public static readonly Vector2 WRONG_HIT = new Vector2(-123, 456); //dummy vec;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        foreach (Vector2 point in GetSpawnPoints())
        {
            Instantiate(itemToSpawn, point, Quaternion.identity, transform);
        }
    }
    //gets the index of the point in the collider that is just before the given point. Returns -1 if out of range.
    private int PrevPointIndex(Vector2 point)
    {
        if (!groundEdge) return -1;
        for(int i = 0; i < groundEdge.edgeCollider.pointCount; i++)
        {
            //If edge point is ahead of given point, return the previous point
            if(Utils.GetWorldPoint(
                groundEdge.edgeCollider.points[i], groundEdge.gameObject).x > point.x)
            {
                return i - 1;
            }
        }
        return -1;
    }

    private Vector2 GetSubEdgeVector(int firstInd)
    {
        return groundEdge.edgeCollider.points[firstInd+1] - groundEdge.edgeCollider.points[firstInd];
    }
    //Returns the projection of the point downward to meet the line specified by the edge collider
    public Vector2 PositionOnLine(Vector2 point)
    {
        //TODO change this from a raycast to vector math
        RaycastHit2D temp = Physics2D.Raycast(point, Vector2.down);
        if (!temp) return WRONG_HIT;
        if (temp.collider.GetComponent<GroundEdge>() != groundEdge) return WRONG_HIT;

        //Debug.DrawLine(point, temp.point);
        return temp.point;
    }
 
    public float HeightOffset()
    {
        Vector2 vp = PositionOnLine(transform.position) - (Vector2)transform.position;
        return vp.magnitude;
        //Vector2 v = GetSubEdgeVector(PrevPointIndex(transform.position));
        //return (vp - (Vector2)Vector3.Project(vp, v)).magnitude;
    }

    

    public bool InRange(Vector2 point)
    {
        
        return (groundEdge != null) && PositionOnLine(point) != WRONG_HIT;
    }

    public Vector2 OffsetOnLine(Vector2 start, float offset)
    {
        groundEdge.edgeCollider.ClosestPoint(start);
        throw new System.Exception();
    }

 
    public Vector2[] GetSpawnPoints()
    {
        //first spawn is at pos
        //subsequent spawns are at an offset
        Vector2[] targets = new Vector2[spawnAmount];
        targets[0] = transform.position;
        float offset = HeightOffset();
        for(int i = 1; i < spawnAmount; i++)
        {
            //find the subedge of the next target
            int ind = PrevPointIndex(targets[i - 1]);
            Vector2 subEdge = GetSubEdgeVector(ind);
            float dist = spawnDelta - (PositionOnLine(targets[i - 1]) - Utils.GetWorldPoint(groundEdge.edgeCollider.points[ind+1], groundEdge.gameObject)).magnitude;
            Debug.DrawRay(transform.position, -(PositionOnLine(targets[i - 1]) - Utils.GetWorldPoint(groundEdge.edgeCollider.points[ind + 1], groundEdge.gameObject)));
            while(dist > 0)
            {
                ind++;
                if (ind > groundEdge.edgeCollider.edgeCount)
                {
                    //shrink array and break
                    Vector2[] temp = new Vector2[i];
                    System.Array.Copy(targets, temp, i);
                    return temp;
                }
                dist -= GetSubEdgeVector(ind).magnitude;

            }
            //Debug.DrawRay()
            targets[i] = (GetSubEdgeVector(ind).magnitude + dist) * GetSubEdgeVector(ind).normalized +
                Utils.PerpendicularCounterClockwise(GetSubEdgeVector(ind)).normalized * offset +
                Utils.GetWorldPoint(groundEdge.edgeCollider.points[ind], groundEdge.gameObject);
            //targets[i] = targets[i] + subEdge.normalized * (subEdge.magnitude + dist);

        }

        return targets;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, GetSubEdgeVector(PrevPointIndex(transform.position))*spawnDelta + (Vector2)transform.position);
        Gizmos.color = Color.red;
        foreach (Vector2 item in GetSpawnPoints())
        {
            Gizmos.DrawSphere(item, 0.5f);
            Gizmos.DrawLine(item, PositionOnLine(item));
        }
    }
}
