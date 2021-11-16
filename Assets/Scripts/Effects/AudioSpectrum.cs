using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSpectrum : MonoBehaviour
{
    public Image spectrumBarPrefab;
    [Header("2�� �������� ����")]
    public int spectrumBarCount = 64;
    [Range(1, 5)]
    public int barDetail = 5;

    Image[,] spectrumBars;

    private float[] samples;

    private float barSize = 0f;

    void Start()
    {
        spectrumBars = new Image[spectrumBarCount, barDetail]; // ����Ʈ�� �������� �迭 ����
        samples = new float[spectrumBarCount]; // GetSpectrumData �Լ��� ���� ���� �迭 ����

        barSize = 1 / (float)spectrumBarCount * 3; // ����Ʈ�� ������� ���� ũ��

        for(int n = 0; n < barDetail; n++)
        {
            for (int i = 0; i < spectrumBarCount; i++)
            {
                spectrumBars[i, n] = Instantiate(spectrumBarPrefab, this.transform); // �� ����

                spectrumBars[i, n].transform.eulerAngles = new Vector3(0, 0, (i / (float)spectrumBarCount * 360) + (360 / barDetail * n)); // ���� ����

                Vector3 circleScreenPosition = Camera.main.WorldToScreenPoint(GameManager.instance.circle.transform.position); // ���� ��ġ ��������

                spectrumBars[i, n].transform.localScale = new Vector3(barSize, 0, 1); // ����Ʈ�� ������� ũ�� �Ҵ�
                spectrumBars[i, n].transform.position = circleScreenPosition 
                                                      + spectrumBars[i, n].transform.up 
                                                      * 70; // ���� ��ġ + ����Ʈ�� ������� ���� ����
            }
        }
    }

    private void Update()
    {
        if(AudioManager.instance.audioSource.isPlaying)
        {
            AudioManager.instance.audioSource.GetSpectrumData(samples, 0, FFTWindow.Hamming); // ����� ����Ʈ�� ���� �����ͼ� samples �迭�� �־���

            for (int n = 0; n < barDetail; n++)
            {
                for (int i = 0; i < spectrumBarCount; i++)
                {
                    spectrumBars[i, n].transform.localScale = new Vector3(barSize, Mathf.Clamp(samples[i] * 3, 0, 2), 1); // ����� ����Ʈ�� �� �Ҵ�
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
