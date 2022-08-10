using UnityEngine.UI;
using UnityEngine;
public class PieGraph : MonoBehaviour
{
    public Image[] wedge;


    public void showGraph(params float[] values)
    {
        float totalValues = 0;
        for(int i = 0; i < wedge.Length; i++)
        {

            totalValues += findPrecentege(values,i);
            wedge[i].fillAmount = totalValues;

        }
    }

    private float findPrecentege(float[] values,int index)
    {
        float totalAmount = 0;
        for(int i = 0; i < values.Length ; i++)
        {
            totalAmount += values[i];
        }
        return values[index] / totalAmount;
    }

 
}
