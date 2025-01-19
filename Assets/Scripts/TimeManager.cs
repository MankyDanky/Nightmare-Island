using System;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{

    public Transform lightTransform;
    public Material skyboxMaterial;
    public float dayTime = 0f;
    public Transform orbit;
    public Gradient transitionGradient;
    public UnityEvent nightfall;
    public UnityEvent dawn;
    GameManager gameManager;
    bool nightfallInvoked = false;
    bool dawnInvoked = false;

    Light mainLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainLight = this.GetComponent<Light>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        skyboxMaterial.SetFloat("_DayTime", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.spawned) {
            dayTime = (dayTime + 0.01f*Time.deltaTime) % (MathF.PI * 2f);
            if (dayTime > Mathf.PI && !nightfallInvoked) {
                dawnInvoked = false;
                nightfallInvoked = true;
                nightfall.Invoke();
            } else if (dayTime < Math.PI && !dawnInvoked) {
                dawnInvoked = true;
                nightfallInvoked = false;
                dawn.Invoke();
            }
            
            float lightAngle = dayTime/MathF.PI/2*360;
            if (Mathf.Sin(dayTime) < 0) {
                lightAngle *= -1;
            }
            lightTransform.eulerAngles = new Vector3(
                    lightAngle,
                    0,
                    0
                );
            orbit.eulerAngles = new Vector3(
                0,
                -90,
                -dayTime/MathF.PI/2*360
            );
            skyboxMaterial.SetFloat("_DayTime", dayTime);
            float t = Mathf.Clamp(MathF.Sin(dayTime) * 10 + 0.5f, 0, 1);
            mainLight.color = transitionGradient.Evaluate(t);
        }
    }
}
