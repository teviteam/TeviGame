using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadOnlineImage : MonoBehaviour {

    public Image ImageToLoadTheTextureOn;
    public Text NoImageYetText;

    private int currentImage = 0;

    //we access inventory to get hold of 
    private GameObject inventoryObj;
    private Inventory inventory;

    void Start()
    {
        inventoryObj = GameObject.Find("InventoryObject");
        if (inventoryObj)
        {
            inventory = inventoryObj.GetComponent<Inventory>();
        }
    }

    //Function added to the plantnet gallery UI menu to load plant pictures
    public void DisplayPlantnetPicturesGallery()
    {
        Texture2D myGalleryTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
        if (inventory.plantnetResultDatas.Count > 0)
        {
            //# todo get dynamic server access
            NoImageYetText.gameObject.SetActive(false);
            ImageToLoadTheTextureOn.gameObject.SetActive(true);
            string imageUrl = "https://metamakers.falmouth.games/kreskanta/images_upload/" + inventory.plantnetResultDatas[0].image_name+".png";
            Debug.Log("url of the image to load" + imageUrl);
            StartCoroutine(setImage(imageUrl, myGalleryTexture));
        }
        else
        {
            NoImageYetText.gameObject.SetActive(true);
            ImageToLoadTheTextureOn.gameObject.SetActive(false);
        }
    }

    IEnumerator setImage(string url, Texture2D myTexture)
    {
        //we verify there is a UI image and a texture to load on
        if (ImageToLoadTheTextureOn)
        {
            Debug.Log("Begin loading the image");

            WWW www = new WWW(url);
            yield return www;
            www.LoadImageIntoTexture(myTexture);
            www.Dispose();
            www = null;

            //adapt UI image size to the proportions of the initial picture
            print("Size is " + myTexture.width + " by " + myTexture.height);
            float imageWidth = myTexture.width;
            float imageHeight = myTexture.height;
            float myImageRatio = imageWidth / imageHeight;
            Debug.Log("myImageRatio:" + myImageRatio+ " ui size width" +ImageToLoadTheTextureOn.rectTransform.sizeDelta.x);
            ImageToLoadTheTextureOn.rectTransform.sizeDelta = new Vector2(myTexture.width, myTexture.height);

            //scale to fit the screen
            float intendedWidth = 700;
            float scaleFactor = intendedWidth / myTexture.width;
            ImageToLoadTheTextureOn.rectTransform.localScale = new Vector3 (scaleFactor, scaleFactor, 1); 

            //to load a texture on UI we have to create a sprite
            Rect rec = new Rect(0, 0, myTexture.width, myTexture.height);
            Sprite myGallerySprite = Sprite.Create(myTexture, rec, new Vector2(0, 0), 1);

            //apply the sprite created to the UI
            ImageToLoadTheTextureOn.GetComponent<Image>().sprite = myGallerySprite;

            //rotate if not portrait and not turned already
            if ((myTexture.width > myTexture.height) && ImageToLoadTheTextureOn.transform.rotation==Quaternion.identity)
            {
                ImageToLoadTheTextureOn.transform.Rotate(new Vector3(0, 0, 270));
            }
        }
        else
        {
            Debug.Log("Not texture to load on");
        }
    }
    /*
    public int getCameraPhotoOrientation(Context context, Uri imageUri, string imagePath)
    {
        int rotate = 0;
        try
        {
            context.getContentResolver().notifyChange(imageUri, null);
            File imageFile = new File(imagePath);

            ExifInterface exif = new ExifInterface(imageFile.getAbsolutePath());
            int orientation = exif.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_NORMAL);

            switch (orientation)
            {
                case ExifInterface.ORIENTATION_ROTATE_270:
                    rotate = 270;
                    break;
                case ExifInterface.ORIENTATION_ROTATE_180:
                    rotate = 180;
                    break;
                case ExifInterface.ORIENTATION_ROTATE_90:
                    rotate = 90;
                    break;
            }

            Log.i("RotateImage", "Exif orientation: " + orientation);
            Log.i("RotateImage", "Rotate value: " + rotate);
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
        return rotate;
    }*/
}
