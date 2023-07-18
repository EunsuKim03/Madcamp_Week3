using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DuoObject", order = 1)]
public class DuoObject : ScriptableObject {
    public string id1;
    public string id2;
    public int duoScore;
}