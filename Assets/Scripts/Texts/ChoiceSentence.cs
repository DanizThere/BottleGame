using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSentence : SimpleSentence
{
    public SimpleSentence[] sentences;
    public List<Button> buttons = new List<Button>();
    //private void Start()
    //{
    //    SetListeners();
    //}

    //private void SetListeners()
    //{
    //    for(int i = 0; i < buttons.Count; i++) {
    //        buttons[i].onClick.AddListener(() => sentences[i].StartType(new System.Threading.CancellationToken()));
    //    }
    //} 
}
