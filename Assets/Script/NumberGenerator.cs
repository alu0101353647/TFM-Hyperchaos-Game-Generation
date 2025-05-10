using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public struct ReturnValues
{
    public double x;
    public double y;
    public double z;
    public double w;

    public ReturnValues(double x, double y, double z, double w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public ReturnValues(ReturnValues other)
    {
        this.x = other.x;
        this.y = other.y;
        this.z = other.z;
        this.w = other.w;
    }

    public override string ToString() => $"({x}, {y}, {z}, {w})";
}

public class NumberGenerator : MonoBehaviour
{
    double a = 35;
    double b = 3;
    [Tooltip("Should be in the range [9,26.17]. Recommended at 15")]
    [SerializeField] double c = 12; // or 21
    double d = 7;
    double x;
    double y;
    double z;
    double step = 0.001;

    double w;
    [Tooltip("Should be in the range [-2000, 1], recommended in range [0.085,0.798]")]
    [SerializeField] double r = 0.79;

    bool paramsSetFlag = false;

    public void SetParams(double x = 1, double y = 1, double z = 1, double d = 7, double w = 1)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.d = d;
        this.w = w;
        paramsSetFlag = true;
    }

    // Returns three random numbers. The amount is how many times you iterate upon the system
    public ReturnValues GiveValues(int amount = 1)
    {
        if (!paramsSetFlag) throw new System.Exception("You haven't set params");
        if (amount < 1) throw new System.Exception("At least amount of 1 for params");
        double Xdot = 0, Ydot = 0, Zdot = 0, Wdot = 0;
        for (int i = 0; i < amount; i++)
        {
            //Xdot = a * (y - x); // VER 1
            //Ydot = (c - a) * x - x * z + c * y;
            //Xdot = x * y - b * z;
            //Xdot = a * (y - x); // VER 2
            //Ydot = d * x - x * z + c * y;
            //Zdot = x * y - b * z;
            Xdot = a * (y - x) + w; // VER 3
            Ydot = d * x - x * z + c * y;
            Zdot = x * y - b * z;
            Wdot = y * z + r * w;
            this.x = this.x + Xdot * step;
            this.y = this.y + Ydot * step;
            this.z = this.z + Zdot * step;
            this.w = this.w + Wdot * step;
        }
        return new ReturnValues(x, y, z, w);
    }
}
