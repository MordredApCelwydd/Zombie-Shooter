using Unity.VisualScripting;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static GameObject Instance { get; private set; }
    
    public void Awake()
    {
        if (Instance != null && Instance != this.GameObject())
        {
            Destroy(this.GameObject());
        }
        else
        {
            Instance = this.GameObject();
        }
    }
}
