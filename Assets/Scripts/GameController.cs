using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Texture2D mapImage;
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] BoidManager boidManager;

    void Start()
    {
        mapGenerator.Initialize(mapImage, boidManager);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
