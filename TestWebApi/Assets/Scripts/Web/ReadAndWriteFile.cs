using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Web
{
    public class ReadAndWriteFile : MonoBehaviour
    {
        #region Variables
        public int count = 1;
        public Image Display;
        #endregion

        #region Functions to Read and Write
        /// <summary>
        /// Store Data to File.
        /// </summary>
        /// <param name="textureImage"></param>
        /// <param name="fileName"></param>
        public void WriteImageOnDisk(Sprite textureImage, string fileName)
        {
            if(File.Exists(Application.persistentDataPath + fileName))
            {
                print("File Already exist");
                return;
            }
            byte[] textureBytes = textureImage.texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + fileName, textureBytes);
            print(Application.persistentDataPath + fileName + " File Written On Disk!");
        }
        
        /// <summary>
        /// Load Image From Disk
        /// </summary>
        /// <param name="textureImage"></param>
        /// <param name="fileName"></param>
        private void ReadImageFromDisk(Image textureImage, string fileName)
        {
            if (!File.Exists(Application.persistentDataPath + fileName))
            {
                print("File not exist");
                return;
            }
            byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + fileName);
            Texture2D loadImage = new(0,0);
            loadImage.LoadImage(textureBytes);
            textureImage.sprite = Sprite.Create(loadImage, new Rect(0f, 0f, loadImage.width, loadImage.height), Vector2.zero);
            //textureImage.SetNativeSize();
        }
        #endregion

        #region Button Call
        /// <summary>
        /// Call by Button so the image will shown in UI.
        /// </summary>
        /// <param name="order"></param>
        public void OnButtonCallDisplayFromDisk(int order)
        {
            if(order == 1)
            {
                ReadImageFromDisk(Display, "Image" + order + ".jpg");
            }
            if(order == 2)
            {
                ReadImageFromDisk(Display, "Image" + order + ".jpg");
            }
        }
        #endregion
    }
}