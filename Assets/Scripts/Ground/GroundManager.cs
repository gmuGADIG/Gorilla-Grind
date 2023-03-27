using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public static GroundManager groundManager;
    public GameObject startSection;
    public GameObject[] sectionPrefabs;
    public LinkedList<GroundSection> activeSections;
    public float spawnOffset;
    // Start is called before the first frame update
    void Start()
    {
        if (!groundManager)
        {
            groundManager = this;
        }
        activeSections = new LinkedList<GroundSection>();
        if(sectionPrefabs.Length == 0)
        {
            Debug.LogError("No prefabs loaded in GroundManager");
        }
        CreateFirstSection();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Z))
        //{
        //    if(sectionPrefabs.Length != 0)
        //    {
        //        CreateNextSection(sectionPrefabs[0]);
        //    }
        //}
        if(sectionPrefabs.Length != 0)
        {
            DespawnCheck();
            SpawnCheck();
        }

    }
    
    /**
     * Two assumptions are made here
     * The first is that the camera and player don't deviate too far from the origin
     * The second is the player can't traverse backwards
     */

    void DespawnCheck()
    {
        if (activeSections.First != null)
        {
            if (activeSections.First.Value.endPoint.x + transform.position.x < -spawnOffset)
            {
                Destroy(activeSections.First.Value.gameObject);
                activeSections.RemoveFirst();
            }
        }
    }
    void SpawnCheck()
    {
        
        if(activeSections.Last == null || activeSections.Last.Value.endPoint.x + transform.position.x < spawnOffset)
        {
            GenerateRandomSection();
        }
    }
    void GenerateRandomSection()
    {
        if(sectionPrefabs.Length != 0)
        {
            CreateNextSection(sectionPrefabs[Random.Range(0, sectionPrefabs.Length - 1)]);
        }
    }
    void CreateNextSection(GameObject sectionPrefab) 
    { 
    
        GameObject instance = Instantiate(sectionPrefab,Vector3.zero,Quaternion.identity);
        if(activeSections.Count != 0)   
        {
            instance.transform.position = ((Vector3)activeSections.Last.Value.endPoint - ((Vector3)instance.GetComponent<GroundSection>().startPoint- instance.transform.position));
            //Aligning height for now since there is lack of specs for mismatches
            //instance.transform.position += Vector3.up * (activeSections.Last.Value.startPoint.y - instance.GetComponent<GroundSection>().startPoint.y);
        }
        activeSections.AddLast(instance.GetComponent<GroundSection>());
    }

    void CreateFirstSection(){
        GameObject instance = Instantiate(startSection);
        activeSections.AddLast(instance.GetComponent<GroundSection>());
    }
}
