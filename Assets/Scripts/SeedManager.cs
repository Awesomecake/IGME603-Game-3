using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seed Manager Instance", menuName = "New Seed Manager Instance")]
public class SeedManager : ScriptableObject
{
    public uint storedSeed = 0;
    public bool useSeed = false;
}
