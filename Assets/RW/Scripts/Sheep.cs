using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float runSpeed;
    public float gotHayDestroyDelay;
    private bool hitByHay;
    public float dropDestroyDelay;
    public float heartOffset;
    public GameObject heartPrefab;
    private Collider myCollider;
    private Rigidbody myRigidbody;
    private SheepSpawner sheepSpawner;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * -runSpeed * Time.deltaTime);
    }

    private void HitByHay()
    {
        hitByHay = true;
        runSpeed = 0;
        GameStateManager.Instance.SavedSheep();
        Destroy(gameObject, gotHayDestroyDelay);
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ;
        tweenScale.targetScale = 0;
        tweenScale.timeToReachTarget = gotHayDestroyDelay;
        sheepSpawner.RemoveSheepFromList(gameObject);
        SoundManager.Instance.PlaySheepHitClip();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay") && !hitByHay)
        {
            Destroy(other.gameObject);
            HitByHay();
        }
        else if (other.CompareTag("DropSheep"))
        {
            Drop();
        }
    }

    private void Drop()
    {
        myRigidbody.isKinematic = false;
        myCollider.isTrigger = false;
        GameStateManager.Instance.DroppedSheep();
        Destroy(gameObject, dropDestroyDelay);
        sheepSpawner.RemoveSheepFromList(gameObject);
        SoundManager.Instance.PlaySheepDroppedClip();
    }

    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }
}
