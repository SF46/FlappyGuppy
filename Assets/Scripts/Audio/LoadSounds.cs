using UnityEngine;
using UnityEngine.Networking;

public class LoadSounds : MonoBehaviour
{
    public static AudioClip SoundFromAssets(string soundPath)
    {
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(soundPath, AudioType.MPEG);
        uwr.SendWebRequest();
        while (!uwr.isDone) { }
        AudioClip tmp = DownloadHandlerAudioClip.GetContent(uwr);
        return tmp;
    }
}