using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager groundManager;
    public GameObject[] sectionPrefabs;
    public LinkedList<GroundSection> activeSections;
    // Start is called before the first frame update
    void Start()
    {
        if (!groundManager)
        {
            groundManager = this;
        }
        activeSections = new LinkedList<GroundSection>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(sectionPrefabs.Length != 0)
            {
                CreateNextSection(sectionPrefabs[0]);
            }
        }
    }

    void CreateNextSection(GameObject sectionPrefab) 
    { 
    
        GameObject instance = Instantiate(sectionPrefab);
        if(activeSections.Count != 0)
        {
            instance.transform.position += (Vector3)(activeSections.Last.Value.endPoint - instance.GetComponent<GroundSection>().startPoint);
        }
        activeSections.AddLast(new LinkedListNode<GroundSection>(instance.GetComponent<GroundSection>()));
    }
}
