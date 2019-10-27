using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    /* Controls the general functionability of buttons. */
    public GameObject[] LinePrefabs;
    public GameObject tutorialPopUp;
    public Rigidbody2D playerRb;
    public Button TapDeleteButton;
    public LineCreator LC;
    public float tapDeleteDistance = 0.1f;
    public bool tapDeleteActive;

    private List<GameObject> hiddenLinesFromScene;
    private GameObject[] activeLinesInScene;
    private ColorBlock prevTapDeleteBtnColors;

    private void Start()
    {
        hiddenLinesFromScene = new List<GameObject>();
        prevTapDeleteBtnColors = TapDeleteButton.colors;
    }

    private void Update()
    {
        if (tapDeleteActive && Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            // We create a "+" made out of 4 raycast with the size of tapDeleteDistance in order to check if we hit a line
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.down, tapDeleteDistance);
            if (hit.collider != null) CheckHitCollider(hit);
            else
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.up, tapDeleteDistance);
                if (hit.collider != null) CheckHitCollider(hit);
                else
                {
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.right, tapDeleteDistance);
                    if (hit.collider != null) CheckHitCollider(hit);
                    else
                    {
                        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.left, tapDeleteDistance);
                        if (hit.collider != null) CheckHitCollider(hit);
                    }
                }
            }   
        }
    }

    private void CheckHitCollider(RaycastHit2D hit)
    {
        if (hit.collider.tag == "Line")
        {
            hiddenLinesFromScene.Add(hit.collider.gameObject);
            hit.collider.gameObject.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        if (CanUseButton())
        {
            GameManager.ReloadLevel();
        }
    }

    private bool CanUseButton()
    {
        if (!tutorialPopUp.activeSelf && playerRb.bodyType == RigidbodyType2D.Static) return true;
        else return false;
    }

    public void Undo()
    {
        if (CanUseButton())
        {
            activeLinesInScene = GetActiveLinesInScene();
            if (activeLinesInScene.Length > 0)
            {
                hiddenLinesFromScene.Add(activeLinesInScene[activeLinesInScene.Length - 1]);
                activeLinesInScene[activeLinesInScene.Length - 1].SetActive(false);
            }
        }
    }

    private GameObject[] GetActiveLinesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void Redo()
    {
        if (CanUseButton())
        {
            if (hiddenLinesFromScene.Count > 0)
            {
                hiddenLinesFromScene[hiddenLinesFromScene.Count - 1].SetActive(true);
                hiddenLinesFromScene.Remove(hiddenLinesFromScene[hiddenLinesFromScene.Count - 1]);
            }
        }
    }

    public void TapDelete()
    {
        if (CanUseButton())
        {
            if (tapDeleteActive)
            {
                // enable drawing lines script
                LC.enabled = true; 
                tapDeleteActive = false;
                // Change background of button to the old one:
                TapDeleteButton.colors = prevTapDeleteBtnColors;
            }
            else
            {
                LC.enabled = false; // disable drawing lines script
                tapDeleteActive = true;
                // Change background of button to the new one:
                ColorBlock colorBlock = prevTapDeleteBtnColors;
                colorBlock.highlightedColor = new Color(colorBlock.highlightedColor.r, colorBlock.highlightedColor.r, colorBlock.highlightedColor.b, 0.4f);
                colorBlock.normalColor = new Color(colorBlock.normalColor.r, colorBlock.normalColor.r, colorBlock.normalColor.b, 0.4f);
                TapDeleteButton.colors = colorBlock;
            }
        }
    }

    public void Save()
    {
        if (!tutorialPopUp.activeSelf)
        {
            SaveSystem.SaveLines(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>());
        }
    }

    public void Load()
    {
        if (CanUseButton())
        {
            //Delete all previous lines
            activeLinesInScene = GetActiveLinesInScene();
            for (int i = 0; i < activeLinesInScene.Length; i++)
            {
                Destroy(activeLinesInScene[i]);
            }
            for (int i = 0; i < hiddenLinesFromScene.Count; i++)
            {
                Destroy(hiddenLinesFromScene[i]);
            }
            hiddenLinesFromScene.Clear();
            // Load the saved lines
            LinesData data = SaveSystem.LoadLines();
            int nrOfLinesInScene;
            int[] eachLineInScenePrefabType;
            nrOfLinesInScene = data.nrOfLinesInScene;
            eachLineInScenePrefabType = data.eachLineInScenePrefabType;
            List<float>[] eachLineInScenePointsListX = data.eachLineInScenePointsListx;
            List<float>[] eachLineInScenePointsListY = data.eachLineInScenePointsListy;
            for (int i = 0; i < nrOfLinesInScene; i++)
            {
                GameObject thisGO = Instantiate(LinePrefabs[eachLineInScenePrefabType[i]]);
                Line thisLine = thisGO.GetComponent<Line>();
                int counter = 0;
                while (counter < eachLineInScenePointsListX[i].Count)
                {
                    thisLine.SetPoint(new Vector2(eachLineInScenePointsListX[i][counter], eachLineInScenePointsListY[i][counter]));
                    counter++;
                }
            }
        }
    }
}
