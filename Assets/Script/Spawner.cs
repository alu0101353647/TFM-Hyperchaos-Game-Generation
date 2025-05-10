using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.Rendering;

public class Spawner : MonoBehaviour
{
    NumberGenerator generator;
    DrawInUnity draw;

    Vector3? lastPosition = null;
    [SerializeField] float amplitudeX;
    [SerializeField] float amplitudeY;
    [SerializeField] float amplitudeZ;
    float amplitudeW = 100;
    [SerializeField] int lengthFunction = 2500;

    [Tooltip("This one goes along the trayectory of the hyperchaotic function")]
    [SerializeField] GameObject flyingSpherePrefab;
    [Tooltip("This one goes along the trayectory of the hyperchaotic function but has a fixed height")]
    [SerializeField] GameObject shuntedFlyingSpherePrefab;
    [Tooltip("This one floats on the trayectory of the hyperchaotic function without moving and shoots at the player")]
    [SerializeField] GameObject shooterPrefab;
    [Tooltip("This one floats on the trayectory of the hyperchaotic function without moving at a fixed height and shoots at the player")]
    [SerializeField] GameObject overheadShooterPrefab;

    private List<Vector3> positions;
    private List<float> wCoord = new();

    private float minW = Mathf.Infinity;
    private float maxW = Mathf.NegativeInfinity;
    private int count = 0;

    [Tooltip("How high will overhead shooter shoot from")]
    [SerializeField] float height = 5f;

    void Start()
    {
        generator = GetComponent<NumberGenerator>();
        generator.SetParams();
        draw = GetComponent<DrawInUnity>();

        // Generate the path
        positions = new List<Vector3>();
        if (lastPosition != null)
        {
            positions.Add((Vector3)lastPosition);
        }
        for (int i = 0; i < lengthFunction; ++i)
        {
            ReturnValues values = generator.GiveValues(10);
            float x = (float)values.x * amplitudeX;
            float y = (float)values.y * amplitudeY;
            float z = (float)values.z * amplitudeZ;
            float w = Mathf.Abs((float)values.w * amplitudeW);
            if (w < minW)
            {
                minW = w;
            }
            if (w > maxW)
            {
                maxW = w;
            }
            positions.Add(new(x, y, z));
            wCoord.Add(w);
        }
        lastPosition = positions[positions.Count - 1];
        draw.Draw(positions);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            GameObject flyer = Instantiate(flyingSpherePrefab, positions[0], transform.rotation);
            flyer.GetComponent<FlyingTrap>().parentOfPath = this.transform;
            flyer.GetComponent<FlyingTrap>().flying = true;
        }

        if (Input.GetKeyUp(KeyCode.O))
        {
            Vector3 position = positions[0];
            position.y = shuntedFlyingSpherePrefab.GetComponent<ShuntedHeightLyingTrap>().GetHeight();
            GameObject shuntedFlyer = Instantiate(shuntedFlyingSpherePrefab, positions[0], transform.rotation);
            shuntedFlyer.GetComponent<ShuntedHeightLyingTrap>().parentOfPath = this.transform;
            shuntedFlyer.GetComponent<ShuntedHeightLyingTrap>().flying = true;
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            int actualPos = (int)Mathf.Round((wCoord[count] * (float)lengthFunction) / maxW);
            if (wCoord[count] > lengthFunction) // include hyperchaos into the count
            {
                count += (int)Mathf.Round(wCoord[count] % lengthFunction);
                count = count % lengthFunction;
            } else
            {
                count += (int)Mathf.Round(wCoord[count]);
                count = count % lengthFunction;
            }
            Vector3 position = positions[actualPos];
            GameObject shooter = Instantiate(shooterPrefab, position, transform.rotation);
            shooter.GetComponent<ShooterEnemy>().SetAngleOffset(wCoord[count]);
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            int actualPos = (int)Mathf.Round((wCoord[count] * (float)lengthFunction) / maxW);
            if (wCoord[count] > lengthFunction) // include hyperchaos into the count
            {
                count += (int)Mathf.Round(wCoord[count] % lengthFunction);
                count = count % lengthFunction;
            }
            else
            {
                count += (int)Mathf.Round(wCoord[count]);
                count = count % lengthFunction;
            }
            Vector3 position = positions[actualPos];
            position.y = height;
            GameObject shooter = Instantiate(shooterPrefab, position, transform.rotation);
            shooter.GetComponent<ShooterEnemy>().SetAngleOffset(wCoord[count]);
        }
    }
}
