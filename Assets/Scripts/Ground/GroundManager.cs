using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GroundManager : MonoBehaviour
{
    public static GroundManager groundManager;
    public GameObject startSection;
    public GameObject[] sectionPrefabs;
    public LinkedList<GroundSection> activeSections;
    // The how far right must the player be to the end of the latest ground section before we spawn another
    public float spawnOffset; 
    // Limit to how low a spawned ground section can be
    public float bottomSpawnLimit;

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
            if (activeSections.First.Value.endPoint.x < transform.position.x -spawnOffset)
            {
                Destroy(activeSections.First.Value.gameObject);
                activeSections.RemoveFirst();
            }
        }
    }
    void SpawnCheck()
    {
        
        if(activeSections.Last == null || activeSections.Last.Value.endPoint.x  < transform.position.x  + spawnOffset)
        {
            GenerateRandomSection();
        }
    }
    void GenerateRandomSection()
    {
        if(sectionPrefabs.Length != 0)
        {

            List<GroundSection> spawnable = GetValidSections();
            if (spawnable.Count == 0)
            {
                //uhh
                Debug.Log("no chunks valid");
            }
            else
            {
                CreateNextSection(spawnable[Random.Range(0, spawnable.Count)].gameObject);
            }
        }
    }

    List<GroundSection> GetValidSections()
    {
        //List of groundsections from prefabs
        List<GroundSection> validSections = sectionPrefabs.ToList().Select(gs => gs.GetComponent<GroundSection>()).ToList();
        // Make sure none of the ground edges dip below the kill box
        return validSections.Where(
                groundSection => {
                    if (groundSection == null) { return false; }
                    //groundSection = Instantiate(groundSection);
                    foreach (Subsection subsection in groundSection.subsections) {
                        if (subsection == null) { return false; }
                        foreach (GroundEdge groundEdge in subsection.groundEdges) {
                            foreach (Vector2 point in groundEdge.edgeCollider.points) {
                                print(activeSections.Last.Value.endPoint);
                                if (point.y + groundEdge.transform.position.y + activeSections.Last.Value.endPoint.y - groundSection.startPoint.y < -bottomSpawnLimit) {
                                    return false;
                                }
                            }
                        }
                    } return true;
                }
        ).ToList();
    }
    void CreateNextSection(GameObject sectionPrefab) 
    { 
    
        GameObject instance = Instantiate(sectionPrefab,Vector3.zero,Quaternion.identity);
        NoSubsectionCheck(instance.GetComponent<GroundSection>());
        if(activeSections.Count != 0)   
        {
            instance.transform.position = ((Vector3)activeSections.Last.Value.endPoint - ((Vector3)instance.GetComponent<GroundSection>().startPoint));
            //Aligning height for now since there is lack of specs for mismatches
            //instance.transform.position += Vector3.up * (activeSections.Last.Value.startPoint.y - instance.GetComponent<GroundSection>().startPoint.y);
        }
        activeSections.AddLast(instance.GetComponent<GroundSection>());
    }

    void CreateFirstSection(){
        GameObject instance = Instantiate(startSection);
        NoSubsectionCheck(instance.GetComponent<GroundSection>());
        //If there is a player, move the section so the player lands on it
        PlayerMovement player = (PlayerMovement)FindObjectOfType(typeof(PlayerMovement));
        if (player)
        {
            Vector3 dest = player.transform.position + new Vector3(-3, -3);
            instance.transform.position += dest - (Vector3)instance.GetComponent<GroundSection>().startPoint;
        }
        activeSections.AddLast(instance.GetComponent<GroundSection>());
        //CreateNextSection(startSection);
    }

    void NoSubsectionCheck(GroundSection toCheck)
    {
        if(toCheck.subsections.Length == 0 || toCheck.mainsection == null)
        {
            GameObject temp = new GameObject("Main subsection");
            toCheck.mainsection = temp.AddComponent<Subsection>();

            toCheck.subsections = new Subsection[] { toCheck.mainsection };
            toCheck.mainsection.groundEdges = toCheck.transform.GetComponentsInChildren<GroundEdge>();
            foreach (GroundEdge i in toCheck.mainsection.groundEdges)
            {
                i.transform.SetParent(temp.transform);
            }
            temp.transform.SetParent(toCheck.transform);
            Debug.Log("Initialized temp subsection");
        }
    }
}
