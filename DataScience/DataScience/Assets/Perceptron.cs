using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class TrainingSet
{
    public double[] input;
    public double output;
}

public class Perceptron : MonoBehaviour
{
    // public TextMeshProUGUI text;
    public TrainingSet[] ts;
    private readonly double[] weights = { 0, 0 };
    private double bias;
    private double totalError;

    private double DotProductBias(double[] v1, double[] v2)
    {
        if (v1 == null || v2 == null)
            return -1;

        if (v1.Length != v2.Length)
            return -1;

        var d = v1.Select((t, x) => t * v2[x]).Sum();

        d += bias;

        return d;
    }

    public int CalcOutput(int i1, int i2)
    {
        double[] inp = { i1, i2 };
        var dp = DotProductBias(weights, inp);
        return dp > 0 ? 1 : 0;
    }

    private int CalcOutput(int i)
    {
        var dp = DotProductBias(weights, ts[i].input);
        return dp > 0 ? 1 : 0;
    }

    private void InitialiseWeights()
    {
        for (var i = 0; i < weights.Length; i++)
            weights[i] = Random.Range(-1.0f, 1.0f);
        
        bias = Random.Range(-1.0f, 1.0f);
    }


    private void UpdateWeights(int j)
    {
        var error = ts[j].output - CalcOutput(j);
        totalError += Mathf.Abs((float)error);
        
        for (var i = 0; i < weights.Length; i++)
            weights[i] += error * ts[j].input[i];
        
        bias += error;
    }

    private void Train(int epochs)
    {
        InitialiseWeights();

        for (var e = 0; e < epochs; e++)
        {
            totalError = 0;
            for (var t = 0; t < ts.Length; t++)
                UpdateWeights(t);
            /*
                Debug.Log("W1: " + (weights[0]) + " W2: " + (weights[1]) + " B: " + bias);
            */
            Debug.Log($"Total error: {totalError}, epoch {e}.");
        }
    }

    private void Awake()
    {
        Train(8);
    }

    private void Start()
    {
        // StartCoroutine(ChengeAguments());
    }

    // private IEnumerator ChengeAguments()
    // {
    //     while (true)
    //     {
    //         ts[0].output = 0;
    //         ts[1].output = 1;
    //         ts[2].output = 1;
    //         ts[3].output = 1;
    //         text.text = "OR";
    //         Train(8);
    //         Debug.Log("Test 0 0: " + CalcOutput(0,0));
    //         Debug.Log("Test 0 1: " + CalcOutput(0,1));
    //         Debug.Log("Test 1 0: " + CalcOutput(1,0));
    //         Debug.Log("Test 1 1: " + CalcOutput(1,1));		
    //         yield return new WaitForSeconds(5);
    //         ts[0].output = 0;
    //         ts[1].output = 0;
    //         ts[2].output = 0;
    //         ts[3].output = 1;
    //         text.text = "AND";
    //         Train(8);
    //         Debug.Log("Test 0 0: " + CalcOutput(0,0));
    //         Debug.Log("Test 0 1: " + CalcOutput(0,1));
    //         Debug.Log("Test 1 0: " + CalcOutput(1,0));
    //         Debug.Log("Test 1 1: " + CalcOutput(1,1));		
    //         yield return new WaitForSeconds(5);
    //         ts[0].output = 1;
    //         ts[1].output = 1;
    //         ts[2].output = 1;
    //         ts[3].output = 0;
    //         text.text = "NAND";
    //         Train(8);
    //         Debug.Log("Test 0 0: " + CalcOutput(0,0));
    //         Debug.Log("Test 0 1: " + CalcOutput(0,1));
    //         Debug.Log("Test 1 0: " + CalcOutput(1,0));
    //         Debug.Log("Test 1 1: " + CalcOutput(1,1));		
    //         yield return new WaitForSeconds(5);
    //         ts[0].output = 0;
    //         ts[1].output = 1;
    //         ts[2].output = 1;
    //         ts[3].output = 0;
    //         text.text = "XOR";
    //         Train(8);
    //         Debug.Log("Test 0 0: " + CalcOutput(0,0));
    //         Debug.Log("Test 0 1: " + CalcOutput(0,1));
    //         Debug.Log("Test 1 0: " + CalcOutput(1,0));
    //         Debug.Log("Test 1 1: " + CalcOutput(1,1));		
    //         yield return new WaitForSeconds(5);
    //     }
    // }
}