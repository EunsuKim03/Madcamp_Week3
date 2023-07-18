using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UrlObject", order = 1)]
public class UrlObject : ScriptableObject {
    public string host;
    public string urlGetAll_Id;
    public string urlGetAll_Duo;
    public string urlLogin;
    public string urlRegister;
    public string urlUpdateSolo;
    public string urlUpdateDuo;
}