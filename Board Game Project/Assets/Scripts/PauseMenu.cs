using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

   
    

    
    
    Button quitButton;
   


    

    public void OnQuitButtonClick()
    {
       
        SceneManager.LoadScene("MainMenu");
    }

    
    


    // Start is called before the first frame update
    void Start()
    {
        
        quitButton = transform.Find("Quit Button").GetComponent<Button>();


       
       
        quitButton.onClick.AddListener(OnQuitButtonClick); 
    }

}
