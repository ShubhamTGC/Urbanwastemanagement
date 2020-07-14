using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YoutubePlayer
{
    public class TriviaHandler : MonoBehaviour
    {
        public List<string> TriviaMsg;
        public Text ShowMSg;
        public Image LoadingBar;
        [HideInInspector]
        public bool Laodingstart;
        [SerializeField]
        private float totaltime;
        [SerializeField]
        private float LimitValue;
        private float currentTime;
        public bool videoplayed = false;
        public YoutubePlayer youtube;
        // Start is called before the first frame update
        void Start()
        {

        }

        void OnEnable()
        {
            System.Random ran = new System.Random();
            int randomnum = ran.Next(1, TriviaMsg.Count);
            //int randomindex = UnityEngine.Random.Range(1, TriviaMsg.Count + 1);
            ShowMSg.text = TriviaMsg[randomnum];
            Laodingstart = true;
            StartCoroutine(CustomLoader());
        }

        void OnDisable()
        {
            Laodingstart = false;
            currentTime = 0f;
            LoadingBar.fillAmount = 0f;

        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator CustomLoader()
        {
           if(currentTime < LimitValue)
            {
                yield return new WaitForSeconds(0.5f);
                currentTime += 2f;
                LoadingBar.fillAmount = currentTime / totaltime;
            }
            else
            {
                while (!youtube.videoplayed)
                {
                    yield return new WaitForSeconds(0.1f);

                }
                yield return new WaitForSeconds(0.5f);
                currentTime += 2f;
                LoadingBar.fillAmount = currentTime / totaltime;
                if(LoadingBar.fillAmount == 1)
                {
                    Laodingstart = false;
                    this.gameObject.SetActive(false);
                }

            }

            if (Laodingstart)
            {
                StartCoroutine(CustomLoader());
            }

            

        }
       
    }



    
}

