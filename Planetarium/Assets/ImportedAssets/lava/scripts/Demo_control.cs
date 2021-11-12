using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace EasyGameStudio.Jeremy
{
    public class Demo_control : MonoBehaviour
    {
        public AudioSource audio_source;
        public AudioClip audio_clip_ka;

        public int index;

        public GameObject[] game_obj_array;


        // Start is called before the first frame update
        void Start()
        {
            this.change_to_index();
        }
        public void on_next_btn()
        {
            this.index++;
            if (this.index >= this.game_obj_array.Length)
                this.index = 0;

            this.change_to_index();
        }
        public void on_previous_btn()
        {
            this.index--;
            if (this.index < 0)
                this.index = this.game_obj_array.Length - 1;

            this.change_to_index();
        }
        private void change_to_index()
        {
            for (int i = 0; i < this.game_obj_array.Length; i++)
            {
                if (i == this.index)
                {
                    this.game_obj_array[i].SetActive(true);
                }
                else
                { 
                    this.game_obj_array[i].SetActive(false);
                }
            }

            this.audio_source.PlayOneShot(this.audio_clip_ka);
        }
    }
}