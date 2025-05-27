using UnityEngine;

public class PlayerCamTracker : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    
    private void Update()
    {
        transform.position = cameraPos.position;
    }
}
