using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver
{
    void DoDash(); //for player movements
    void DoMoveDir(Vector2 aDir); //for movement controls
    void DoLeftAction(); //left option
    void DoRightAction(); //right option
    void DoSubmitAction(); //space option
    void DoCancelAction(); //esc option
}
