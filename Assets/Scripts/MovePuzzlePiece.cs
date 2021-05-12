using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePuzzlePiece : MonoBehaviour
{
    private SpriteRenderer rend;
    public PlayerButton upButton, rightButton, leftButton, downButton, CWButton, CCWButton, BackwardsButton, ForwardButton;

    public bool pieceLocked;
    
    // Start is called before the first frame update
    void Start()
    {
        pieceLocked = true;
        rend = gameObject.GetComponent<SpriteRenderer>();
        rend.sortingOrder = 0;

        //Enables all the relevant buttons for the plug player.
        upButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);

        // Disable rotation buttons
        CWButton.gameObject.SetActive(false);
        CCWButton.gameObject.SetActive(false);
        // Disable z-directional buttons (unnecessary for 2D puzzle)
        BackwardsButton.gameObject.SetActive(false);
        ForwardButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pieceLocked)
        {
            rend.sortingOrder = 1;

            if (rightButton.IsPressed)
            {
                gameObject.transform.position += (transform.right * 0.3f);
            }
            else if (leftButton.IsPressed)
            {
                gameObject.transform.position += (transform.right * -0.3f);
            }
            else if (upButton.IsPressed)
            {
                gameObject.transform.position += (transform.up * 0.3f);
            }
            else if (downButton.IsPressed)
            {
                gameObject.transform.position += (transform.up * -0.3f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!pieceLocked && other.gameObject.name == gameObject.name)
        {
            print("Collision detected for " + gameObject.name);
            gameObject.transform.position = other.gameObject.transform.position;
            pieceLocked = true;
            GetComponent<GameManager>().WinGame();
        }
    }
}
