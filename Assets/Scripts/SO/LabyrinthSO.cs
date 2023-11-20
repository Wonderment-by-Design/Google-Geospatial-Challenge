using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Labyrinth", menuName = "ScriptableObjects/Labyrinth", order = 1)]
public class LabyrinthSO : ScriptableObject
{
    public string LabyrinthName;
    public Sprite MapIcon;
}
