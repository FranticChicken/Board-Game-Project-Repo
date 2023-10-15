using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public bool pauseMenuActive;
    

    
    
    Button quitButton;
    Button resumeButton;
    Transform pauseMenuBackground;


    public void OnResumeButtonClick()
    {
        pauseMenuBackground.gameObject.SetActive(false);
        
        pauseMenuActive = false;
        
    }

    public void OnQuitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

  
    


    // Start is called before the first frame update
    void Start()
    {
        pauseMenuActive = false;
        resumeButton = transform.Find("Pause Menu Background").Find("Resume Button").GetComponent<Button>();
        quitButton = transform.Find("Pause Menu Background").Find("Quit Button").GetComponent<Button>();


        pauseMenuBackground = transform.Find("Pause Menu Background");
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape was pressed");
            if(pauseMenuActive == false)
            {
                Debug.Log("pause menu is here!");
                pauseMenuBackground.gameObject.SetActive(true);
                pauseMenuActive = true;
                
            }
            else if(pauseMenuActive == true)
            {
                pauseMenuBackground.gameObject.SetActive(false);
                pauseMenuActive = false;
                
            }
        }
        


    }
}
