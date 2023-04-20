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
    public int spawnAmount;


    //Gets the associated ground edge
    public GroundEdge? groundEdge => transform.parent.GetComponent<GroundEdge>();

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

    //Returns the projection of the point downward to meet the line specified by the edge collider
    public Vector2 PositionOnLine(Vector2 point)
    {
        RaycastHit2D temp = Physics2D.Raycast(point, Vector2.down);
        if (!temp) return WRONG_HIT;
        if (temp.collider.GetComponent<GroundEdge>() != groundEdge) return WRONG_HIT;

        return temp.point;
    }
 
    public float HeightOffset()
    {
        return transform.position.y - PositionOnLine(transform.position).y;
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

        return new Vector2[] { this.transform.position };
    }
}
