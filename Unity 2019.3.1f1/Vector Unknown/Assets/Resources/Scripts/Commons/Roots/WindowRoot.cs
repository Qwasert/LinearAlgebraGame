using UnityEngine;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected ResourceService resourceService = null;
    protected AudioService audioService = null;

    public void SetWindowState(bool isActive)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }

        if (isActive)
        {
            InitWindow();
        }
        else
        {
            ClearWindow();
        }
    }

    protected virtual void InitWindow()
    {
        resourceService = GameRoot.instance.resourceService;
        audioService = GameRoot.instance.audioService;
        try
        {
            GameObject.Find("MainCamera").GetComponent<CameraController>().isLock = false;
        }
        catch { }
        try
        {
            GameObject.Find("Main Camera").GetComponent<CameraController>().isLock = false;
        }
        catch { }
    }

    protected virtual void ClearWindow()
    {
        resourceService = null;
        audioService = null;
        try
        {
            GameObject.Find("MainCamera").GetComponent<CameraController>().isLock = true;
        }
        catch { }
        try
        {
            GameObject.Find("Main Camera").GetComponent<CameraController>().isLock = true;
        }
        catch { }
    }

    #region Common Tool Functions

    #region SetActive
    protected void SetActive(GameObject gameObject, bool state = true)
    {
        gameObject.SetActive(state);
    }

    protected void SetActive(Transform transform, bool state = true)
    {
        transform.gameObject.SetActive(state);
    }

    protected void SetActive(RectTransform rectTransform, bool state = true)
    {
        rectTransform.gameObject.SetActive(state);
    }

    protected void SetActive(Image image, bool state = true)
    {
        image.transform.gameObject.SetActive(state);
    }

    protected void SetActive(Text text, bool state = true)
    {
        text.transform.gameObject.SetActive(state);
    }
    #endregion

    #region SetText
    protected void SetText(Text text, string context = "")
    {
        text.text = context;
    }

    protected void SetText(Transform transform, int number = 0)
    {
        SetText(transform.GetComponent<Text>(), number);
    }

    protected void SetText(Transform transform, string context = "")
    {
        SetText(transform.GetComponent<Text>(), context);
    }

    protected void SetText(Text text, int number = 0)
    {
        SetText(text, number.ToString());
    }
    #endregion

    #endregion
}
