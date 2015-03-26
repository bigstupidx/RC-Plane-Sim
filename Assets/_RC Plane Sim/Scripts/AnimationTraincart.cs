using UnityEngine;
using System.Collections;

public class AnimationTraincart : MonoBehaviour 
{
    public float speed;
    public float frequency;

    private float zBasic = -38;
    private float zNeed = 38;

    void Start()
    {
        transform.localPosition = new Vector3(5f, -1.5f, zBasic);
    }


    void Update()
    {
        if (transform.localPosition.z == zBasic)
            StartCoroutine(Move());
    }

    IEnumerator Move() 
    {
        while (transform.localPosition.z <= zNeed)
        {
            transform.localPosition += Vector3.forward * Time.fixedDeltaTime * speed;

            yield return null;
        }

        yield return new WaitForSeconds(frequency);

        transform.localPosition = new Vector3(5f, -1.5f, zBasic);
    }
}
