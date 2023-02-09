using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _image;

    [SerializeField] private Sprite _goodSprite;
    [SerializeField] private Sprite _badSprite;

    private static CancellationTokenSource cancelTokenSource;
    private CancellationToken cancelToken;

    public async Task LoadingCircle()
    {
        while (true)
        {
            if (cancelToken.IsCancellationRequested)
                return;
            if (_image.fillAmount == 1)
                _image.fillAmount = 0;
            _image.fillAmount += 0.1f;
            await Task.Delay(100);
        }
    }

    public void GoodScreen()
        => UpdateScreen(_goodSprite);

    public void BadScreen()
        => UpdateScreen(_badSprite);

    private void UpdateScreen(Sprite sprite)
    {
        var img = GetComponent<Image>();
        img.sprite = sprite;
        img.color = Color.white;
    }
    public void InitTokenSource()
    {
        cancelTokenSource = new CancellationTokenSource();
        cancelToken = cancelTokenSource.Token;
    }

    public void StopCleaning()
    {
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();
        _image.gameObject.SetActive(false);
    }

}
