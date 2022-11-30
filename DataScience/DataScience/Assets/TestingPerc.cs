using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPerc : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [SerializeField] private float force = 10000f;
    [SerializeField] private Perceptron pr;
    private Vector3 pos;
    public int numb; 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pos = transform.position;
        StartCoroutine(ResetPosition());
    }

    private void OnCollisionEnter(Collision collision)
    {
        TestingPerc cube;
        if (collision.gameObject.TryGetComponent(out cube) && pr.CalcOutput(cube.numb, numb) == 1)
        {
            var impulse = (transform.position - collision.contacts[0].point).normalized * force;
            GetComponent<Rigidbody>().AddForce(impulse);
        }
    }

    private IEnumerator ResetPosition()
    {
        while (true)
        {
            transform.position = pos;
            yield return new WaitForSeconds(5);
        }
    }
}
