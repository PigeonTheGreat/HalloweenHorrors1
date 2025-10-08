using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
        [SerializeField] public Text scoreText;

        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        
            // Start is called before the first frame update
            void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        
}
