using System.Collections;
using TMPro;
using UnityEngine;

internal class RecordingButtonView  : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _iconText;

    public void ResetElement()
    {
        _iconText.gameObject.SetActive(true);
        _iconText.faceColor = Color.white;
        _text.gameObject.SetActive(false);
        
    }

    public IEnumerator StartRecordingCoroutine()
    {
        _iconText.gameObject.SetActive(false);
        _text.gameObject.SetActive(true);
        _text.SetText("3");
        yield return new WaitForSeconds(1);
        _text.SetText("2");
        yield return new WaitForSeconds(1);
        _text.SetText("1");
        yield return new WaitForSeconds(1);
        _iconText.faceColor = Color.red;
        _text.gameObject.SetActive(false);
        _iconText.gameObject.SetActive(true);
        
    }
}