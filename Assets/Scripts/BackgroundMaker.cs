    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMaker : MonoBehaviour
{

    public GameObject[] BackgroundObj;
    public GameObject[] ForegroundObj;
    

    // Start is called before the first frame update
    void Start()
    {
        BackgroundObjectScript.newBackground += MakeBackground;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeBackground(bool isBackground){
        GameObject obj;
        if(isBackground){
            obj = BackgroundObj[Random.Range(0,BackgroundObj.Length)];
        }else{
            obj = ForegroundObj[Random.Range(0,ForegroundObj.Length)];
        }
        Instantiate(obj,new Vector3(80,1,10),Quaternion.identity);
    }
}
