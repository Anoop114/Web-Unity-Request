using TMPro;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


namespace Assets.Scripts.Web
{
    public class UrlCall : MonoBehaviour
    {
        #region Variables 
        // variables that are going to used in the scripts.

        //public
        public GameObject LoadImageBtn;
        public ReadAndWriteFile storeImage;
        //private
        private string UrlOutput;
        [SerializeField] private TMP_InputField urlInput;
        [SerializeField] private TMP_Text DisplayUrl;

        //private unchanged
        private readonly string inputUrlName = "InputURL";

        
        #endregion

        #region UnityFunctions
        private void Awake()
        {
            //if there is not any reference of input Text field.
            if (urlInput == null)
            {
                urlInput = GameObject.Find(inputUrlName).GetComponent<TMP_InputField>();
            }
        }
        #endregion

        #region FunctionToGetJsonData
        /// <summary>
        /// Call this function of button click.
        /// </summary>
        public void CheckUrl()
        {
            GetData(
                (string error) =>
                {
                    DisplayUrl.text = "Error : " + error;
                },
                (string success) =>
                {
                    DisplayUrl.text = "Get Data SucessFull";
                    UrlOutput = "{\"Items\":" + success + "}";
                    //File.WriteAllText(Application.dataPath + "/UrlData.json", DisplayUrl.text);
                    StartCoroutine(DisplayJson());
                    
                    
                }
            );
        }
        /// <summary>
        /// Call the Url function to Get the json data from web
        /// </summary>
        /// <param name="OnError"></param>
        /// <param name="OnSuccess"></param>
        private void GetData(Action<string> OnError, Action<string> OnSuccess)
        {
            StartCoroutine(GetDataFromUrl(OnError, OnSuccess));
        }

        /// <summary>
        /// Get the url from user and check weather the url is correct or not,
        /// if Correct give the Json data that stored in URL.
        /// </summary>
        /// <param name="OnError"></param>
        /// <param name="OnSuccess"></param>
        /// <returns></returns>
        private IEnumerator GetDataFromUrl(Action<string> OnError, Action<string> OnSuccess)
        {
            using UnityWebRequest getUrlJsonData = UnityWebRequest.Get(urlInput.text);
            yield return getUrlJsonData.SendWebRequest();

            //handle error
            if (getUrlJsonData.result != UnityWebRequest.Result.Success)
            {
                OnError(getUrlJsonData.error);
            }
            //On Success
            else
            {
                OnSuccess(getUrlJsonData.downloadHandler.text);
            }
        }
        #endregion

        #region FunctionToGetTexture
        /// <summary>
        /// This Function handle The download and Calling Of the Textures Functions.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisplayJson()
        {
            UrlList data = JsonUtility.FromJson<UrlList>(UrlOutput);
            int totalImgages = data.Items.Length;
            for (int i = 0; i < totalImgages; i++)
            {
                yield return new WaitForSeconds(1f);
                string url = data.Items[i].url;
                GetTexture(url,
                    (Texture2D sucess) => 
                    {
                        Sprite outImage = Sprite.Create(sucess, new Rect(0, 0, sucess.width, sucess.height), new Vector2(.5f, .5f), 100f);
                        storeImage.WriteImageOnDisk(outImage, "Image" + i + ".jpg");
                        DisplayUrl.text = "Download Success";
                        LoadImageBtn.SetActive(true);
                    },
                    (string error) => 
                    {
                        DisplayUrl.text = "Error : " + error;
                    }
                );
            }
            
        }

        /// <summary>
        /// This function call the GetTextureFromJsonUrl.
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="OnComplete"></param>
        /// <param name="OnError"></param>
        private void GetTexture(string URL, Action<Texture2D> OnComplete, Action<string> OnError)
        {
            StartCoroutine(GetTextureFromJsonUrl(URL, OnComplete, OnError));
        }

        /// <summary>
        /// Use this function to get Texture from the Json data.
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="OnComplete"></param>
        /// <param name="OnError"></param>
        /// <returns></returns>
        private IEnumerator GetTextureFromJsonUrl(string URL, Action<Texture2D> OnComplete, Action<string> OnError)
        {
            using UnityWebRequest getTexture = UnityWebRequestTexture.GetTexture(URL);
            yield return getTexture.SendWebRequest();

            //handle error
            if (getTexture.result != UnityWebRequest.Result.Success)
            {
                OnError(getTexture.error);
            }
            //On Success
            else
            {
                Texture2D myTexture = ((DownloadHandlerTexture)getTexture.downloadHandler).texture;
                OnComplete(myTexture);
            }
        }
        #endregion
    }
}