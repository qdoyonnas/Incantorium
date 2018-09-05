using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOrb : MenuOrb
{
    protected override void DoFinalAction()
    {
        Application.Quit();
    }
}
