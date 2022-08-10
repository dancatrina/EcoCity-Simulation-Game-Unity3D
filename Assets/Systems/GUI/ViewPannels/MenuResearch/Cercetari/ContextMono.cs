using UnityEngine.EventSystems;
using UnityEngine;
using CodeMonkey.Utils;

public class ContextMono : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("GameObj")]
    public GameObject popUpFereastraDetaliiTehnologii;
    public int distantaFataDeButon;
    

    bool isActive = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DetaliiTehnologie containerFereastra = popUpFereastraDetaliiTehnologii.GetComponent<DetaliiTehnologie>();
        GameObject takeCurrentButton = eventData.pointerCurrentRaycast.gameObject;
        ATehnologie UiScriptInfo = takeCurrentButton.GetComponent<ATehnologie>();
        containerFereastra.transform.position = takeCurrentButton.transform.position;
        containerFereastra.transform.position = new Vector2(containerFereastra.transform.position.x + distantaFataDeButon, containerFereastra.transform.position.y);
        FunctionTimer.Create(() =>
        {
            containerFereastra.descriere.text = UiScriptInfo.descriere;
            isActive = true;
            popUpFereastraDetaliiTehnologii.SetActive(isActive);
        }, 1, "f");
    }
    void Destroy()
    {
        FunctionTimer.StopAllTimersWithName("f");
        if (isActive == true)
        {
            isActive = false;
            popUpFereastraDetaliiTehnologii.SetActive(isActive);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy();
    }
}
