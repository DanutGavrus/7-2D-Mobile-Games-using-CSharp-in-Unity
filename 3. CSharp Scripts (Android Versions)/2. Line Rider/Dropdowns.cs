using UnityEngine;
using UnityEngine.UI;

public class Dropdowns : MonoBehaviour {
    /* Controls the general functionability of dropdowns. */

    public GameObject[] LinePrefabs;
    public Dropdown LineType;
    public Dropdown LineSound;
    public Dropdown LineBackground;

    private int lineTypePrevState;
    private int lineSoundPrevState;
    private int lineBackgroundPrevState;

    private void Start()
    {
        // Initialize the Line Prefab Normal
        LineCreator.LinePrefab = LinePrefabs[0];
        //Set previous state of dropdowns to see if their value has changed
        LineType.value = 1;
        lineTypePrevState = LineType.value;
        lineSoundPrevState = LineSound.value;
        lineBackgroundPrevState = LineBackground.value;
    }

    private void Update()
    {
        // 1 for LineType
        if (lineTypePrevState != LineType.value)
        {
            lineTypePrevState = LineType.value; //Set the new value, no infinite loop
            LineSound.value = 0; // Set other dropdown to NONE
            lineSoundPrevState = LineSound.value;
            LineBackground.value = 0; // Set other dropdown to NONE
            lineBackgroundPrevState = LineBackground.value;
            switch (LineType.value) // Select the prefab that is used in the LineCreator
            {
                case 1:
                    LineCreator.LinePrefab = LinePrefabs[0]; // Normal
                    break;
                case 2:
                    LineCreator.LinePrefab = LinePrefabs[1]; // Boost
                    break;
                case 3:
                    LineCreator.LinePrefab = LinePrefabs[2]; // Bouncy
                    break;
                case 4:
                    LineCreator.LinePrefab = LinePrefabs[3]; // Trampoline
                    break;
            }
        }
        // 2 for LineSound
        if (lineSoundPrevState != LineSound.value)
        {
            lineSoundPrevState = LineSound.value; //Set the new value, no infinite loop
            LineType.value = 0; // Set other dropdown to NONE
            lineTypePrevState = LineType.value;
            LineBackground.value = 0; // Set other dropdown to NONE
            lineBackgroundPrevState = LineBackground.value;
            switch (LineSound.value) // Select the prefab that is used in the LineCreator
            {
                case 1:
                    LineCreator.LinePrefab = LinePrefabs[4]; // DO
                    break;
                case 2:
                    LineCreator.LinePrefab = LinePrefabs[5]; // RE
                    break;
                case 3:
                    LineCreator.LinePrefab = LinePrefabs[6]; // MI
                    break;
                case 4:
                    LineCreator.LinePrefab = LinePrefabs[7]; // FA
                    break;
                case 5:
                    LineCreator.LinePrefab = LinePrefabs[8]; // SOL
                    break;
                case 6:
                    LineCreator.LinePrefab = LinePrefabs[9]; // LA
                    break;
                case 7:
                    LineCreator.LinePrefab = LinePrefabs[10]; // SI
                    break;
            }
        }
        // 3 for LineBackground
        if (lineBackgroundPrevState != LineBackground.value)
        {
            lineBackgroundPrevState = LineBackground.value; //Set the new value, no infinite loop
            LineSound.value = 0; // Set other dropdown to NONE
            lineSoundPrevState = LineSound.value;
            LineType.value = 0; // Set other dropdown to NONE
            lineTypePrevState = LineType.value;
            switch (LineBackground.value) // Select the prefab that is used in the LineCreator
            {
                case 1:
                    LineCreator.LinePrefab = LinePrefabs[11]; // RED
                    break;
                case 2:
                    LineCreator.LinePrefab = LinePrefabs[12]; // GREEN
                    break;
                case 3:
                    LineCreator.LinePrefab = LinePrefabs[13]; // BLUE
                    break;
            }
        }
        // If all 3 are 0(none), then force LineType to 1
        if (LineType.value == 0 && LineSound.value == 0 && LineBackground.value == 0)
        {
            LineType.value = 1;
            lineTypePrevState = LineType.value;
            LineCreator.LinePrefab = LinePrefabs[0];
        }
    }
}
