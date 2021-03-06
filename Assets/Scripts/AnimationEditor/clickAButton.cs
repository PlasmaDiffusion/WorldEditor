﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickAButton : clickButton
{
    //For the animation tool direction buttons

    public Vector2 dValue;

    protected override void clicked()
    {
        base.clicked();
        if (lastCreated != null && lastCreated.GetComponent<DirectionObject>() != null)
        {
            lastCreated.GetComponent<DirectionObject>().dValue = dValue * GameObject.FindObjectOfType<UnityEngine.UI.Slider>().value;
        }
    }
}
