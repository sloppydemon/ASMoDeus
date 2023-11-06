using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AsteroidMazeGenerator : MonoBehaviour
{
    List<Vector2> lineStarts;
    List<Vector2> lineEnds;
    public int numberOfLevels;
    public float coreVicinityFactor;
    public int openings;
    public int cells;
    public float radiusPerLevel;
    float radius;
    float arcAngle;

    // Start is called before the first frame update
    void Start()
    {
        for (int level = 0; level <= numberOfLevels; level++)
        {
            radius = level * radiusPerLevel;
            arcAngle = 360 / (cells ^ level);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
