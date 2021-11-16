using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSpectrum : MonoBehaviour
{
    public Image spectrumBarPrefab;
    [Header("2의 제곱수만 가능")]
    public int spectrumBarCount = 64;
    [Range(1, 5)]
    public int barDetail = 5;

    Image[,] spectrumBars;

    private float[] samples;

    private float barSize = 0f;

    void Start()
    {
        spectrumBars = new Image[spectrumBarCount, barDetail]; // 스펙트럼 막대기들의 배열 정의
        samples = new float[spectrumBarCount]; // GetSpectrumData 함수의 값을 받을 배열 정의

        barSize = 1 / (float)spectrumBarCount * 3; // 스펙트럼 막대기의 가로 크기

        for(int n = 0; n < barDetail; n++)
        {
            for (int i = 0; i < spectrumBarCount; i++)
            {
                spectrumBars[i, n] = Instantiate(spectrumBarPrefab, this.transform); // 바 생성

                spectrumBars[i, n].transform.eulerAngles = new Vector3(0, 0, (i / (float)spectrumBarCount * 360) + (360 / barDetail * n)); // 바의 각도

                Vector3 circleScreenPosition = Camera.main.WorldToScreenPoint(GameManager.instance.circle.transform.position); // 원의 위치 가져오기

                spectrumBars[i, n].transform.localScale = new Vector3(barSize, 0, 1); // 스펙트럼 막대기의 크기 할당
                spectrumBars[i, n].transform.position = circleScreenPosition 
                                                      + spectrumBars[i, n].transform.up 
                                                      * 70; // 원의 위치 + 스펙트럼 막대기의 방향 벡터
            }
        }
    }

    private void Update()
    {
        if(AudioManager.instance.audioSource.isPlaying)
        {
            AudioManager.instance.audioSource.GetSpectrumData(samples, 0, FFTWindow.Hamming); // 오디오 스펙트럼 값을 가져와서 samples 배열에 넣어줌

            for (int n = 0; n < barDetail; n++)
            {
                for (int i = 0; i < spectrumBarCount; i++)
                {
                    spectrumBars[i, n].transform.localScale = new Vector3(barSize, Mathf.Clamp(samples[i] * 3, 0, 2), 1); // 오디오 스펙트럼 값 할당
                }
            }
        }
    }

    public void ChangeSpectrumColor(Color color)
    {
        for (int n = 0; n < barDetail; n++)
        {
            for (int i = 0; i < spectrumBarCount; i++)
            {
                spectrumBars[i, n].color = new Color(color.r, color.g, color.b, 1);
            }
        }
    }
}
