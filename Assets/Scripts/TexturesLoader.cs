using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TexturesLoader : MonoBehaviour
{
    #region Variables

    private const string GetTexturesLink = "https://picsum.photos/200/200";

    #endregion


    #region Public methods

    public void LoadTextures(int texturesNumber, Action<List<Sprite>> completeCallback)
    {
        StartCoroutine(GetTextures(texturesNumber, completeCallback));
    }

    #endregion


    #region Private methods

    private IEnumerator GetTextures(int texturesNumber, Action<List<Sprite>> completeCallback)
    {
        List<Sprite> sprites = new List<Sprite>();

        for (int i = 0; i < texturesNumber; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(GetTexturesLink);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D myTexture = ((DownloadHandlerTexture) www.downloadHandler).texture;
                Sprite sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height),
                    Vector2.zero);

                sprites.Add(sprite);
            }
        }

        completeCallback?.Invoke(sprites);
    }

    #endregion
}
