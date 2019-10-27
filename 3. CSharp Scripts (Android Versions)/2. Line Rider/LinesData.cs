using System.Collections.Generic;
[System.Serializable]
public class LinesData {

    public int nrOfLinesInScene;
    public int[] eachLineInScenePrefabType;
    public List<float>[] eachLineInScenePointsListx;
    public List<float>[] eachLineInScenePointsListy;

    public LinesData (GameManager gameManager)
    {
        nrOfLinesInScene = gameManager.ReturnNrOfLinesInScene();
        eachLineInScenePrefabType = gameManager.EachLineInScenePrefabType();
        eachLineInScenePointsListx = new List<float>[nrOfLinesInScene];
        eachLineInScenePointsListy = new List<float>[nrOfLinesInScene];
        eachLineInScenePointsListx = gameManager.EachLineInScenePointsListX();
        eachLineInScenePointsListy = gameManager.EachLineInScenePointsListY();
    }
}
