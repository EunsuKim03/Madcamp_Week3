using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UserObject", order = 1)]
public class UserObject : ScriptableObject {
    public string id;
    public int solo;
}