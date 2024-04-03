using UnityEngine;
using System.Collections;
using System.IO.Ports;

[ExecuteInEditMode]
public class ReadArduino : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM6", 9600);

    public int data_qtd = 5;
    public int[] datas;
    void Start()
    {
        data_stream.Open();
        datas = new int[data_qtd];

    }
    private void Update()
    {
        ReadMultipleValues();
    }

    void ReadMultipleValues()
    {
        string[] fragmented_data = data_stream.ReadLine().Split(',');

        for (int i = 0; i < fragmented_data.Length; i++)
        {
            string value = fragmented_data[i];
            int.TryParse(value, out datas[i]);
        }
        data_stream.DiscardInBuffer();
    }
}
