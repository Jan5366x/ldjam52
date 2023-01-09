using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterAdjuster : MonoBehaviour
{
    private const string MASTER = "Master";
    public AudioMixer mixer;

    private void Start()
    {
        SetMaster();
    }

    private void SetMaster(float value)
    {
        mixer.SetFloat(MASTER, value);
        float outValue;
        mixer.GetFloat(MASTER, out outValue);
        Debug.Log(outValue);
    }

    public void SetMaster()
    {
        SetMaster(GetComponent<Slider>().value);
    }
}