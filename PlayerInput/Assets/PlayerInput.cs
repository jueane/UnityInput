using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

// 与rewired的键位名字映射
class InputActionName
{
    public const string TriggerLeft = "TriggerLeft";
    public const string TriggerRight = "TriggerRight";

    public const string StickLeftX = "MoveX";
    public const string StickLeftY = "MoveY";
    public const string StickRighX = "LookX";
    public const string StickRighY = "LookY";

    public const string StickButtonLeft = "StickButtonLeft";
    public const string StickButtonRight = "StickButtonRight";

    public const string BumperLeft = "BumperLeft";
    public const string BumperRight = "BumperRight";

    public const string Up = "Up";
    public const string Down = "Down";
    public const string Left = "Left";
    public const string Right = "Right";

    public const string DPadUp = "DPadUp";
    public const string DPadDown = "DPadDown";
    public const string DPadLeft = "DPadLeft";
    public const string DPadRight = "DPadRight";

    public const string Back = "Back";
    public const string Start = "Start";
}


public enum EKeyAction
{
    keyDown,
    keyPressing,
    keyUp
}

// callback signature
public delegate void StickCallback(Vector2 vec);
public delegate void TriggerCallback(float value);
public delegate void KeyCallback();

// 当前只把rewired当作获取源，如果将来rewired的Touch模块不可用，就把Rewired剥离出来，与移动端的Touch并列
public class PlayerInput : MonoBehaviour
{
    const float DEAD_ZONE = 0.3f;

    // stick
    event StickCallback stickLeft_Listener;
    event StickCallback stickRight_Listener;

    event KeyCallback keyStickLeft_Listener_Down;
    event KeyCallback keyStickLeft_Listener_Pressing;
    event KeyCallback keyStickLeft_Listener_Up;
    event KeyCallback keyStickRight_Listener_Down;
    event KeyCallback keyStickRight_Listener_Pressing;
    event KeyCallback keyStickRight_Listener_Up;

    // trigger
    event TriggerCallback triggerLeft_Listener;
    event TriggerCallback triggerRight_Listener;

    event KeyCallback keyUp_Listener_Down;
    event KeyCallback keyUp_Listener_Pressing;
    event KeyCallback keyUp_Listener_Up;
    event KeyCallback keyDown_Listener_Down;
    event KeyCallback keyDown_Listener_Pressing;
    event KeyCallback keyDown_Listener_Up;
    event KeyCallback keyLeft_Listener_Down;
    event KeyCallback keyLeft_Listener_Pressing;
    event KeyCallback keyLeft_Listener_Up;
    event KeyCallback keyRight_Listener_Down;
    event KeyCallback keyRight_Listener_Pressing;
    event KeyCallback keyRight_Listener_Up;

    // keys
    event KeyCallback keyPadUp_Listener_Down;
    event KeyCallback keyPadUp_Listener_Pressing;
    event KeyCallback keyPadUp_Listener_Up;
    event KeyCallback keyPadDown_Listener_Down;
    event KeyCallback keyPadDown_Listener_Pressing;
    event KeyCallback keyPadDown_Listener_Up;
    event KeyCallback keyPadLeft_Listener_Down;
    event KeyCallback keyPadLeft_Listener_Pressing;
    event KeyCallback keyPadLeft_Listener_Up;
    event KeyCallback keyPadRight_Listener_Down;
    event KeyCallback keyPadRight_Listener_Pressing;
    event KeyCallback keyPadRight_Listener_Up;

    // back
    event KeyCallback keyBack_Listener_Down;
    event KeyCallback keyBack_Listener_Pressing;
    event KeyCallback keyBack_Listener_Up;
    // start
    event KeyCallback keyStart_Listener_Down;
    event KeyCallback keyStart_Listener_Pressing;
    event KeyCallback keyStart_Listener_Up;

    // bumper
    event KeyCallback bumperLeft_Listener_Down;
    event KeyCallback bumperLeft_Listener_Pressing;
    event KeyCallback bumperLeft_Listener_Up;
    event KeyCallback bumperRight_Listener_Down;
    event KeyCallback bumperRight_Listener_Pressing;
    event KeyCallback bumperRight_Listener_Up;

    // 全键
    public struct AllKeys
    {
        public static Vector2 StickLeft
        {
            get
            {
                return new Vector2(playerRewired.GetAxis(InputActionName.StickLeftX), playerRewired.GetAxis(InputActionName.StickLeftY));
            }
        }

        public static Vector2 StickRight
        {
            get
            {
                return new Vector2(playerRewired.GetAxis(InputActionName.StickRighX), playerRewired.GetAxis(InputActionName.StickRighY));
            }
        }

        public static float TriggerLeft { get { return PlayerInput.playerRewired.GetAxis(InputActionName.TriggerLeft); } }
        public static float TriggerRight { get { return PlayerInput.playerRewired.GetAxis(InputActionName.TriggerRight); } }

        public static bool StickButtonLeft { get { return PlayerInput.playerRewired.GetButton(InputActionName.StickButtonLeft); } }
        public static bool StickButtonRight { get { return PlayerInput.playerRewired.GetButton(InputActionName.StickButtonRight); } }

        public static bool BumperLeft { get { return PlayerInput.playerRewired.GetButton(InputActionName.BumperLeft); } }
        public static bool BumperRight { get { return PlayerInput.playerRewired.GetButton(InputActionName.BumperRight); } }

        public static bool Up { get { return PlayerInput.playerRewired.GetButton(InputActionName.Up); } }
        public static bool Down { get { return PlayerInput.playerRewired.GetButton(InputActionName.Down); } }
        public static bool Left { get { return PlayerInput.playerRewired.GetButton(InputActionName.Left); } }
        public static bool Right { get { return PlayerInput.playerRewired.GetButton(InputActionName.Right); } }

        public static bool DPadUp { get { return PlayerInput.playerRewired.GetButton(InputActionName.DPadUp); } }
        public static bool DPadDown { get { return PlayerInput.playerRewired.GetButton(InputActionName.DPadDown); } }
        public static bool DPadLeft { get { return PlayerInput.playerRewired.GetButton(InputActionName.DPadLeft); } }
        public static bool DPadRight { get { return PlayerInput.playerRewired.GetButton(InputActionName.DPadRight); } }

        public static bool Back { get { return PlayerInput.playerRewired.GetButton(InputActionName.Back); } }
        public static bool Start { get { return PlayerInput.playerRewired.GetButton(InputActionName.Start); } }

        public static Vector2 MouseScreenPostion
        {
            get
            {
                if (PlayerInput.playerRewired.controllers.hasMouse)
                {
                    return PlayerInput.playerRewired.controllers.Mouse.screenPosition;
                }
                else
                {
                    return Vector2.zero;
                }

            }
        }
    }


    static PlayerInput pi;

    public static bool available;

    static Player playerRewired;

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
            var x = playerRewired.GetAxis(InputActionName.StickLeftX);
            var y = playerRewired.GetAxis(InputActionName.StickLeftY);
            x = (x > -DEAD_ZONE && x < DEAD_ZONE) ? 0 : x;
            y = (y > -DEAD_ZONE && y < DEAD_ZONE) ? 0 : y;
            stickLeft_Listener(new Vector2(x, y));
        }
        if (stickRight_Listener != null)
        {
            var x = playerRewired.GetAxis(InputActionName.StickRighX);
            var y = playerRewired.GetAxis(InputActionName.StickRighY);
            x = (x > -DEAD_ZONE && x < DEAD_ZONE) ? 0 : x;
            y = (y > -DEAD_ZONE && y < DEAD_ZONE) ? 0 : y;
            stickRight_Listener(new Vector2(x, y));
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
