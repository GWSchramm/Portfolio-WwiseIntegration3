using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawController : MonoBehaviour
{
    public GameObject jaw;
    public float maxJawRotation = 42f;
    public float jawSpeed = 50f;
    public float minDb = -38f; //dont exceed -48dB
    public int maxDb = 0;

    private float currentRotation = 0f;


    void Update()
    {
        if (AkMicrophone.Instance == null) return;

        float micLevel = AkMicrophone.Instance.GetMicLevel(); //get the current mic level from the AkMicrophone script and set micLevel to it's value

        //convert dB (-48 to 0) → normalized (0 to 1)
        float normalized = Mathf.Clamp01(Mathf.InverseLerp(minDb, maxDb, micLevel));
        normalized = 1f - normalized;

        //the target rotation of the jaw is based on the micLevel float above
        float targetRotation = normalized * maxJawRotation;

        //makes the rotation smooth
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, jawSpeed * Time.deltaTime);

        //applies the rotation to the jaw game object
        jaw.transform.localRotation = Quaternion.Euler(currentRotation, 0f, 0f);

        if(micLevel > minDb)
        {
            Debug.Log(micLevel);
        }
        

    }

}
