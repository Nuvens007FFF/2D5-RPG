using System.Collections;
using UnityEngine;

public class HatTung : MonoBehaviour
{
    public float KnockForce;
    public float KnockTime;
    public bool KnockFormRight = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Knock(Vector3 knockDirection)
    {
        if (!KnockFormRight)
        {
            KnockFormRight = true;
            Rigidbody characterRigidbody = GetComponent<Rigidbody>();
            characterRigidbody.velocity = Vector3.zero; // reset vận tốc của nhân vật về 0
            characterRigidbody.AddForce(knockDirection * KnockForce, ForceMode.Impulse);
            StartCoroutine(ResetTimeKnock());
        }

    }

    private IEnumerator ResetTimeKnock()
    {
        Rigidbody characterRigidbody = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(KnockTime);
        KnockFormRight = false;
        Debug.Log("Run off");
        characterRigidbody.velocity = Vector3.zero;
    }
}
