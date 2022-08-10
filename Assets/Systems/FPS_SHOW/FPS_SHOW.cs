using UnityEngine.UI;
using UnityEngine;

public class FPS_SHOW : MonoBehaviour
{
    public Text fpsText;
    private float poolingTime = 1f;
    private float time;
    private int frameCount;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        if(time >= poolingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount/time);
            fpsText.text = frameRate.ToString() + " FPS";

            time -= poolingTime;
            frameCount = 0;
        }
    }
}
