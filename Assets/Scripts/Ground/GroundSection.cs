using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
[ExecuteInEditMode]
public class GroundSection : MonoBehaviour
{
    [HideInInspector]
    public Subsection[] subsections;

    public Vector2 startPoint { get => subsections[0].startPoint; }

    public Vector2 endPoint { get => subsections[0].endPoint; }


    /**
     * Reconnects every ground edge to the next one
     */
    public void VerifySubsections()
    {
        subsections = GetComponentsInChildren<Subsection>();



    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += VerifySubsections;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= VerifySubsections;
    }
#endif


    void Update()
    {
        
    }


}
