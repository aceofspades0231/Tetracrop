using UnityEngine;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField]
    TMP_InputField nameInput;

    private void Awake()
    {
        nameInput.characterLimit = 3;
    }

    
    private void Update()
    {
        
    }
}
