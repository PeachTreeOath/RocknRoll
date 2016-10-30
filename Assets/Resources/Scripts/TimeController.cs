using UnityEngine;
using System.Collections;

public class TimeController : Singleton<TimeController>
{

    private float currSpeed;
    private float defaultFixedDeltaTime;

    // Use this for initialization
    void Start()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Time.timeScale = TimeSpeed.PAUSED;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = TimeSpeed.VSLOW;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = TimeSpeed.SLOW;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = TimeSpeed.NORMAL;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = TimeSpeed.FAST;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Time.timeScale = TimeSpeed.VFAST;
        }

        Time.fixedDeltaTime = Mathf.Clamp(defaultFixedDeltaTime * Time.timeScale, defaultFixedDeltaTime * TimeSpeed.VSLOW, defaultFixedDeltaTime);
    }
}
