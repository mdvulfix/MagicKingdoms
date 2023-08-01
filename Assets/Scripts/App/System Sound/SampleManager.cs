using System;
using System.Collections.Generic;

using Randomizer = System.Random;

using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;

[Serializable, ExecuteAlways]
public class SampleManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Serializable]
    public struct SampleInfo
    {
        public SampleInfo(AudioSource sample, float chance)
        {
            Sample = sample;
            Chance = chance;
        }


        public AudioSource Sample;

        [Range(0, 1)]
        public float Chance;

    }

    [SerializeField] private float m_Chance;
    [SerializeField] private SampleInfo[] Samples;


    private List<SampleInfo> m_SamplesInPlayList;
    private List<SampleInfo> m_SampleIsPlaying;

    private Randomizer m_Randomizer;


    private void OnEnable()
    {
        if (Samples.Length == 0)
            throw new Exception("Sample list is ampty. Add a new sample to continue!");

        m_Randomizer = new Randomizer();

        foreach (var sample in Samples)
            m_SamplesInPlayList.Add(sample);


    }


    private void Start()
    {

    }



    private void Play()
    {

        m_Chance = m_Randomizer.Next(0, 1);





    }


}



[CustomEditor(typeof(SampleManager))]
public class EditorSampleManager : Editor
{

    private SampleManager m_SamleInfo => (SampleManager)target;


    public override void OnInspectorGUI()
    {
        var instance = (SampleManager)target;

        var isUpdate = DrawDefaultInspector();

        if (isUpdate)
            Draw();



    }

    private void OnEnable()
    {

    }


    private void Draw()
    {

        GUILayout.BeginHorizontal();





        GUILayout.EndHorizontal();


    }

}

#endif