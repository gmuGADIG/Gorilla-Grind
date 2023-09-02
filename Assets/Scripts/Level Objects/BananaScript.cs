using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    public BoxCollider2D bananaCollider;
    public CapsuleCollider2D playerCollider;
    public ParticleSystem collectionParticles;

    int BananaSoundID;

    // Start is called before the first frame update
    void Start()
    {
        BananaSoundID = SoundManager.Instance.GetSoundID("Banana_Pickup");
        playerCollider = GameObject.Find("Player").transform.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            SoundManager.Instance.PlaySoundGlobal(BananaSoundID);
            RunController.Current.AddBananas(1);
            var particles = Instantiate(collectionParticles, transform.parent);
            particles.transform.position = this.transform.position;
            Destroy(gameObject);
        }
    }
}
