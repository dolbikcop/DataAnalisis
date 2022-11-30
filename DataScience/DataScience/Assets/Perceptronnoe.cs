using UnityEngine;

public class Perceptronnoe : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private int firstValue;
    [SerializeField] private int secondValue;
    [SerializeField] private float force;
    [SerializeField] private Perceptron perceptron;
    private bool _goesFar;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _goesFar = perceptron.CalcOutput(firstValue, secondValue) != 0;
    }

    private void FixedUpdate()
    {
        if (_goesFar)
        {
            _rb.AddForce(_rb.transform.TransformDirection(Vector3.forward) * force, ForceMode.Impulse);
        }
    }
}
