using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;

public class HandRecorder : MonoBehaviour
{
    public AnimationClip clip;

    private GameObjectRecorder m_Recorder;

    public enum RECORDINGSTATUS {IDLE,START_RECORDING,RECRODING,STOPED};
    private RECORDINGSTATUS m_recordingStatus = RECORDINGSTATUS.IDLE;

    private string animation_name = "swing_animation";
    private int clipCount = 0;

    void Start()
    {
                // Create recorder and record the script GameObject.
                StartCoroutine(waitAndSetstatus());
                

    }

    // Update is called once per frame
    void Update()
    {
        if (clip == null)
            return;

        switch(m_recordingStatus)
        {
            case RECORDINGSTATUS.IDLE:
            break;
            case RECORDINGSTATUS.START_RECORDING:
                m_Recorder = new GameObjectRecorder(gameObject);
                // Bind all the Transforms on the GameObject and all its children.
                m_Recorder.BindComponentsOfType<Transform>(gameObject, true);  
                clip = new AnimationClip();
                m_recordingStatus = RECORDINGSTATUS.RECRODING;        
            m_recordingStatus = RECORDINGSTATUS.RECRODING;
            break;
            case RECORDINGSTATUS.RECRODING:
                m_Recorder.TakeSnapshot(Time.deltaTime);
            break;
            case RECORDINGSTATUS.STOPED:
                if (m_Recorder.isRecording)
                {
                    // Save the recorded session to the clip.
                    m_Recorder.SaveToClip(clip);
                    saveAnimation(clip,animation_name);
                    m_recordingStatus = RECORDINGSTATUS.IDLE;
                }
            break;
        }
    }

    public void saveAnimation(AnimationClip clip, string clipName) {
        clipName = "Assets/" + clipName + " - " + clipCount + ".anim";
        AssetDatabase.CreateAsset(clip, clipName);
        AssetDatabase.SaveAssets();
        clipCount++;
    }
    private void setRecordingStatus(RECORDINGSTATUS status)
    {
        m_recordingStatus = status;
    }

    public void startRecord()
    {   
        setRecordingStatus(RECORDINGSTATUS.START_RECORDING);
    }

    public void stopRecord()
    {
        setRecordingStatus(RECORDINGSTATUS.STOPED);
    }

    IEnumerator waitAndSetstatus()
    {
        yield return new WaitForSeconds(5);
        setRecordingStatus(RECORDINGSTATUS.START_RECORDING);
        yield return new WaitForSeconds(10);
        setRecordingStatus(RECORDINGSTATUS.STOPED);
    }
}
