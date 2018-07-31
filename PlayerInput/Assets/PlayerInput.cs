using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

class InputActionName
{
    public const string TriggerLeft = "Grenade";
    public const string TriggerRight = "Fire";

    public const string StickLeftX = "MoveX";
    public const string StickLeftY = "MoveY";
    public const string StickRighX = "LookX";
    public const string StickRighY = "LookY";

    public const string StickButtonLeft = "Crouch";
    public const string StickButtonRight = "Aim";

    public const string BumperLeft = "Use";
    public const string BumperRight = "Melee";

    public const string Up = "Attack";
    public const string Down = "Activate";
    public const string Left = "Reload";
    public const string Right = "Jump";

    public const string DPadUp = "NextWeapon";
    public const string DPadDown = "PrevWeapon";
    public const string DPadLeft = "PrevGrenade";
    public const string DPadRight = "NextGrenade";

    public const string Back = "Inventory";
    public const string Start = "Pause";
}

public enum EKeyAction
{
    keyDown,
    keyPressing,
    keyUp
}

// 当前只把rewired当作获取源，如果将来rewired的Touch模块不可用，就把Rewired剥离出来，与移动端的Touch并列
public class PlayerInput : MonoBehaviour
{
    public delegate void StickCallback(Vector2 vec);
    public delegate void TriggerCallback(float value);
    public delegate void KeyCallback();

    // stick
    public event StickCallback stickLeft_Listener;
    public event StickCallback stickRight_Listener;

    public event KeyCallback keyStickLeft_Listener_Down;
    public event KeyCallback keyStickLeft_Listener_Pressing;
    public event KeyCallback keyStickLeft_Listener_Up;
    public event KeyCallback keyStickRight_Listener_Down;
    public event KeyCallback keyStickRight_Listener_Pressing;
    public event KeyCallback keyStickRight_Listener_Up;

    // trigger
    public event TriggerCallback triggerLeft_Listener;
    public event TriggerCallback triggerRight_Listener;

    public event KeyCallback keyUp_Listener_Down;
    public event KeyCallback keyUp_Listener_Pressing;
    public event KeyCallback keyUp_Listener_Up;
    public event KeyCallback keyDown_Listener_Down;
    public event KeyCallback keyDown_Listener_Pressing;
    public event KeyCallback keyDown_Listener_Up;
    public event KeyCallback keyLeft_Listener_Down;
    public event KeyCallback keyLeft_Listener_Pressing;
    public event KeyCallback keyLeft_Listener_Up;
    public event KeyCallback keyRight_Listener_Down;
    public event KeyCallback keyRight_Listener_Pressing;
    public event KeyCallback keyRight_Listener_Up;

    // keys
    public event KeyCallback keyPadUp_Listener_Down;
    public event KeyCallback keyPadUp_Listener_Pressing;
    public event KeyCallback keyPadUp_Listener_Up;
    public event KeyCallback keyPadDown_Listener_Down;
    public event KeyCallback keyPadDown_Listener_Pressing;
    public event KeyCallback keyPadDown_Listener_Up;
    public event KeyCallback keyPadLeft_Listener_Down;
    public event KeyCallback keyPadLeft_Listener_Pressing;
    public event KeyCallback keyPadLeft_Listener_Up;
    public event KeyCallback keyPadRight_Listener_Down;
    public event KeyCallback keyPadRight_Listener_Pressing;
    public event KeyCallback keyPadRight_Listener_Up;

    // back
    public event KeyCallback keyBack_Listener_Down;
    public event KeyCallback keyBack_Listener_Pressing;
    public event KeyCallback keyBack_Listener_Up;
    // start
    public event KeyCallback keyStart_Listener_Down;
    public event KeyCallback keyStart_Listener_Pressing;
    public event KeyCallback keyStart_Listener_Up;

    // bumper
    public event KeyCallback bumperLeft_Listener_Down;
    public event KeyCallback bumperLeft_Listener_Pressing;
    public event KeyCallback bumperLeft_Listener_Up;
    public event KeyCallback bumperRight_Listener_Down;
    public event KeyCallback bumperRight_Listener_Pressing;
    public event KeyCallback bumperRight_Listener_Up;

    static PlayerInput pi;

    Player playerRewired;

    public bool available;

    // Use this for initialization
    void Start()
    {
        playerRewired = ReInput.players.GetPlayer(0);

        if (pi == null)
        {
            pi = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // stick
        if (stickLeft_Listener != null)
        {
            var moveaxis = new Vector2(playerRewired.GetAxis(InputActionName.StickLeftX), playerRewired.GetAxis(InputActionName.StickLeftY));
            stickLeft_Listener(moveaxis);
        }
        if (stickRight_Listener != null)
        {
            var moveaxis = new Vector2(playerRewired.GetAxis(InputActionName.StickRighX), playerRewired.GetAxis(InputActionName.StickRighY));
            stickRight_Listener(moveaxis);
        }

        // stick key left
        if (keyStickLeft_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.StickButtonLeft))
        {
            keyStickLeft_Listener_Down();
        }
        if (keyStickLeft_Listener_Pressing != null && playerRewired.GetButton(InputActionName.StickButtonLeft))
        {
            keyStickLeft_Listener_Pressing();
        }
        if (keyStickLeft_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.StickButtonLeft))
        {
            keyStickLeft_Listener_Up();
        }

        // stick key right
        if (keyStickRight_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.StickButtonRight))
        {
            keyStickLeft_Listener_Down();
        }
        if (keyStickRight_Listener_Pressing != null && playerRewired.GetButton(InputActionName.StickButtonRight))
        {
            keyStickRight_Listener_Pressing();
        }
        if (keyStickRight_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.StickButtonRight))
        {
            keyStickRight_Listener_Up();
        }

        // trigger
        if (triggerLeft_Listener != null)
        {
            triggerLeft_Listener(playerRewired.GetAxis(InputActionName.TriggerLeft));
        }
        if (triggerRight_Listener != null)
        {
            triggerRight_Listener(playerRewired.GetAxis(InputActionName.TriggerRight));
        }

        // bumper left
        if (bumperLeft_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.BumperLeft))
        {
            bumperLeft_Listener_Down();
        }
        if (bumperLeft_Listener_Pressing != null && playerRewired.GetButton(InputActionName.BumperLeft))
        {
            bumperLeft_Listener_Pressing();
        }
        if (bumperLeft_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.BumperLeft))
        {
            bumperLeft_Listener_Up();
        }
        // bumper right
        if (bumperRight_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.BumperRight))
        {
            bumperLeft_Listener_Down();
        }
        if (bumperRight_Listener_Pressing != null && playerRewired.GetButton(InputActionName.BumperRight))
        {
            bumperLeft_Listener_Pressing();
        }
        if (bumperRight_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.BumperRight))
        {
            bumperLeft_Listener_Up();
        }

        // DPad up
        if (keyPadUp_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.DPadUp))
        {
            keyPadUp_Listener_Down();
        }
        if (keyPadUp_Listener_Pressing != null && playerRewired.GetButton(InputActionName.DPadUp))
        {
            keyPadUp_Listener_Pressing();
        }
        if (keyPadUp_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.DPadUp))
        {
            keyPadUp_Listener_Up();
        }
        // DPad down
        if (keyPadDown_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.DPadDown))
        {
            keyPadDown_Listener_Down();
        }
        if (keyPadDown_Listener_Pressing != null && playerRewired.GetButton(InputActionName.DPadDown))
        {
            keyPadDown_Listener_Pressing();
        }
        if (keyPadDown_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.DPadDown))
        {
            keyPadDown_Listener_Up();
        }
        // DPad left
        if (keyPadLeft_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.DPadLeft))
        {
            keyPadLeft_Listener_Down();
        }
        if (keyPadLeft_Listener_Pressing != null && playerRewired.GetButton(InputActionName.DPadLeft))
        {
            keyPadLeft_Listener_Pressing();
        }
        if (keyPadLeft_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.DPadLeft))
        {
            keyPadLeft_Listener_Up();
        }
        // DPad right
        if (keyPadRight_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.DPadRight))
        {
            keyPadRight_Listener_Down();
        }
        if (keyPadRight_Listener_Pressing != null && playerRewired.GetButton(InputActionName.DPadRight))
        {
            keyPadRight_Listener_Pressing();
        }
        if (keyPadRight_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.DPadRight))
        {
            keyPadRight_Listener_Up();
        }

        // key up
        if (keyUp_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.Up))
        {
            keyUp_Listener_Down();
        }
        if (keyUp_Listener_Pressing != null && playerRewired.GetButton(InputActionName.Up))
        {
            keyUp_Listener_Pressing();
        }
        if (keyUp_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.Up))
        {
            keyUp_Listener_Up();
        }
        // key down
        if (keyDown_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.Down))
        {
            keyDown_Listener_Down();
        }
        if (keyDown_Listener_Pressing != null && playerRewired.GetButton(InputActionName.Down))
        {
            keyDown_Listener_Pressing();
        }
        if (keyDown_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.Down))
        {
            keyDown_Listener_Up();
        }
        // key left
        if (keyLeft_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.Left))
        {
            keyLeft_Listener_Down();
        }
        if (keyLeft_Listener_Pressing != null && playerRewired.GetButton(InputActionName.Left))
        {
            keyLeft_Listener_Pressing();
        }
        if (keyLeft_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.Left))
        {
            keyLeft_Listener_Up();
        }
        // key right
        if (keyRight_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.Right))
        {
            keyRight_Listener_Down();
        }
        if (keyRight_Listener_Pressing != null && playerRewired.GetButton(InputActionName.Right))
        {
            keyRight_Listener_Pressing();
        }
        if (keyRight_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.Right))
        {
            keyRight_Listener_Up();
        }

        // key back
        if (keyBack_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.Back))
        {
            keyBack_Listener_Down();
        }
        if (keyBack_Listener_Pressing != null && playerRewired.GetButton(InputActionName.Back))
        {
            keyBack_Listener_Pressing();
        }
        if (keyBack_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.Back))
        {
            keyBack_Listener_Up();
        }        
        // key start
        if (keyStart_Listener_Down != null && playerRewired.GetButtonDown(InputActionName.Start))
        {
            keyStart_Listener_Down();
        }
        if (keyStart_Listener_Pressing != null && playerRewired.GetButton(InputActionName.Start))
        {
            keyStart_Listener_Pressing();
        }
        if (keyStart_Listener_Up != null && playerRewired.GetButtonUp(InputActionName.Start))
        {
            keyStart_Listener_Up();
        }
    }

    // stick left
    public static void ListenStickLeft(StickCallback cb)
    {
        pi.stickLeft_Listener += cb;
    }
    // stick right
    public static void ListenStickRight(StickCallback cb)
    {
        pi.stickRight_Listener += cb;
    }
    // trigger left
    public static void ListenTriggerLeft(TriggerCallback cb)
    {
        pi.triggerLeft_Listener += cb;
    }
    // trigger right
    public static void ListenTriggerRight(TriggerCallback cb)
    {
        pi.triggerRight_Listener += cb;
    }
    // bumper left
    public static void ListenBumperLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.bumperLeft_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.bumperLeft_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.bumperLeft_Listener_Down += cb;
        }
    }
    // bumper right
    public static void ListenBumperRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.bumperRight_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.bumperRight_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.bumperRight_Listener_Up += cb;
        }
    }
    // pad
    public static void ListenKeyPadUp(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyPadUp_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyPadUp_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyPadUp_Listener_Up += cb;
        }
    }
    public static void ListenKeyPadDown(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyPadDown_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyPadDown_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyPadDown_Listener_Up += cb;
        }
    }
    public static void ListenKeyPadLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyPadLeft_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyPadLeft_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyPadLeft_Listener_Up += cb;
        }
    }
    public static void ListenKeyPadRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
        }
    }
    // directions
    public static void ListenKeyUp(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyUp_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyUp_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyUp_Listener_Up += cb;
        }
    }
    public static void ListenKeyDown(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyDown_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyDown_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyDown_Listener_Up += cb;
        }
    }
    public static void ListenKeyLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyLeft_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyLeft_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyLeft_Listener_Up += cb;
        }
    }
    public static void ListenKeyRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyRight_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyRight_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyRight_Listener_Up += cb;
        }
    }

    // functions
    public static void ListenKeyBack(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyBack_Listener_Down();
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyBack_Listener_Pressing();
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyBack_Listener_Up();
        }
    }
    public static void ListenKeyStart(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyStart_Listener_Down();
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyStart_Listener_Pressing();
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyStart_Listener_Up();
        }
    }
    // stick key    
    public static void ListenKeyStickLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyStickLeft_Listener_Down();
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyStickLeft_Listener_Pressing();
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyStickLeft_Listener_Up();
        }
    }
    public static void ListenKeyStickRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            pi.keyStickRight_Listener_Down();
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            pi.keyStickRight_Listener_Pressing();
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            pi.keyStickRight_Listener_Up();
        }
    }
}
