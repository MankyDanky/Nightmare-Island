using Unity.VisualScripting;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{

    public float viewDistance;
    public Transform player;
    Camera mainCamera;
    Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        mainCamera = Camera.main;
        canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {  
        float d = (transform.position - player.position).magnitude;
        if (d > viewDistance) {
            canvas.enabled = false;
        } else {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
            canvas.enabled = true;
        }
        
    }
}
