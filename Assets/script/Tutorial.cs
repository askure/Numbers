using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Panel, BackButton, NextButton, FinishButton;
    [SerializeField] Image image;
    List<Sprite> Photo;
    int index;

    public void  SetUpTutorial(List<Sprite> photo)
    {
        Photo = photo;
        index = 0;
        if (Photo.Count == 0)  return;
        if (Photo.Count == 1) NextButton.SetActive(false);
        else FinishButton.SetActive(false);
        BackButton.SetActive(false);
        image.sprite = Photo[index];
    }

    public void Next()
    {
        if ((index + 1) == Photo.Count) return;
        index++;
        image.sprite = Photo[index];
        BackButton.SetActive(true);
        if(index == Photo.Count - 1)
        {
            NextButton.SetActive(false);
            FinishButton.SetActive(true);
        }
    }
    public void Back()
    {
        if (index == 0) return;
        index--;
        image.sprite = Photo[index];
        if (index == 0) BackButton.SetActive(false);
    }

    public void Finish()
    {
        Destroy(Panel);
    }
   
}
