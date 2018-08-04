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

    // 所有键状态查询
    public struct AllKeys
    {
        public static Vector2 StickLeft
        {
            get
            {
                return new Vector2(rewired.GetAxis(InputActionName.StickLeftX), rewired.GetAxis(InputActionName.StickLeftY));
            }
        }

        public static Vector2 StickRight
        {
            get
            {
                return new Vector2(rewired.GetAxis(InputActionName.StickRighX), rewired.GetAxis(InputActionName.StickRighY));
            }
        }

        public static float TriggerLeft { get { return rewired.GetAxis(InputActionName.TriggerLeft); } }
        public static float TriggerRight { get { return rewired.GetAxis(InputActionName.TriggerRight); } }

        public static bool StickButtonLeft { get { return rewired.GetButton(InputActionName.StickButtonLeft); } }
        public static bool StickButtonRight { get { return rewired.GetButton(InputActionName.StickButtonRight); } }

        public static bool BumperLeft { get { return rewired.GetButton(InputActionName.BumperLeft); } }
        public static bool BumperRight { get { return rewired.GetButton(InputActionName.BumperRight); } }

        public static bool Up { get { return rewired.GetButton(InputActionName.Up); } }
        public static bool Down { get { return rewired.GetButton(InputActionName.Down); } }
        public static bool Left { get { return rewired.GetButton(InputActionName.Left); } }
        public static bool Right { get { return rewired.GetButton(InputActionName.Right); } }

        public static bool DPadUp { get { return rewired.GetButton(InputActionName.DPadUp); } }
        public static bool DPadDown { get { return rewired.GetButton(InputActionName.DPadDown); } }
        public static bool DPadLeft { get { return rewired.GetButton(InputActionName.DPadLeft); } }
        public static bool DPadRight { get { return rewired.GetButton(InputActionName.DPadRight); } }

        public static bool Back { get { return rewired.GetButton(InputActionName.Back); } }
        public static bool Start { get { return rewired.GetButton(InputActionName.Start); } }

        public static Vector2 MouseScreenPostion
        {
            get
            {
                if (rewired.controllers.hasMouse)
                {
                    return rewired.controllers.Mouse.screenPosition;
                }
                else
                {
                    return Vector2.zero;
                }
            }
        }
    }

    static PlayerInput instance;

    public static bool available;

    static Player rewired;

    // Use this for initialization
    void Awake()
    {
        rewired = ReInput.players.GetPlayer(0);

        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // stick
        if (stickLeft_Listener != null)
        {
            var x = rewired.GetAxis(InputActionName.StickLeftX);
            var y = rewired.GetAxis(InputActionName.StickLeftY);
            x = (x > -DEAD_ZONE && x < DEAD_ZONE) ? 0 : x;
            y = (y > -DEAD_ZONE && y < DEAD_ZONE) ? 0 : y;
            stickLeft_Listener(new Vector2(x, y));
        }
        if (stickRight_Listener != null)
        {
            var x = rewired.GetAxis(InputActionName.StickRighX);
            var y = rewired.GetAxis(InputActionName.StickRighY);
            x = (x > -DEAD_ZONE && x < DEAD_ZONE) ? 0 : x;
            y = (y > -DEAD_ZONE && y < DEAD_ZONE) ? 0 : y;
            stickRight_Listener(new Vector2(x, y));
        }

        // stick key left
        if (keyStickLeft_Listener_Down != null && rewired.GetButtonDown(InputActionName.StickButtonLeft))
        {
            keyStickLeft_Listener_Down();
        }
        if (keyStickLeft_Listener_Pressing != null && rewired.GetButton(InputActionName.StickButtonLeft))
        {
            keyStickLeft_Listener_Pressing();
        }
        if (keyStickLeft_Listener_Up != null && rewired.GetButtonUp(InputActionName.StickButtonLeft))
        {
            keyStickLeft_Listener_Up();
        }

        // stick key right
        if (keyStickRight_Listener_Down != null && rewired.GetButtonDown(InputActionName.StickButtonRight))
        {
            keyStickLeft_Listener_Down();
        }
        if (keyStickRight_Listener_Pressing != null && rewired.GetButton(InputActionName.StickButtonRight))
        {
            keyStickRight_Listener_Pressing();
        }
        if (keyStickRight_Listener_Up != null && rewired.GetButtonUp(InputActionName.StickButtonRight))
        {
            keyStickRight_Listener_Up();
        }

        // trigger
        if (triggerLeft_Listener != null)
        {
            triggerLeft_Listener(rewired.GetAxis(InputActionName.TriggerLeft));
        }
        if (triggerRight_Listener != null)
        {
            triggerRight_Listener(rewired.GetAxis(InputActionName.TriggerRight));
        }

        // bumper left
        if (bumperLeft_Listener_Down != null && rewired.GetButtonDown(InputActionName.BumperLeft))
        {
            bumperLeft_Listener_Down();
        }
        if (bumperLeft_Listener_Pressing != null && rewired.GetButton(InputActionName.BumperLeft))
        {
            bumperLeft_Listener_Pressing();
        }
        if (bumperLeft_Listener_Up != null && rewired.GetButtonUp(InputActionName.BumperLeft))
        {
            bumperLeft_Listener_Up();
        }
        // bumper right
        if (bumperRight_Listener_Down != null && rewired.GetButtonDown(InputActionName.BumperRight))
        {
            bumperLeft_Listener_Down();
        }
        if (bumperRight_Listener_Pressing != null && rewired.GetButton(InputActionName.BumperRight))
        {
            bumperLeft_Listener_Pressing();
        }
        if (bumperRight_Listener_Up != null && rewired.GetButtonUp(InputActionName.BumperRight))
        {
            bumperLeft_Listener_Up();
        }

        // DPad up
        if (keyPadUp_Listener_Down != null && rewired.GetButtonDown(InputActionName.DPadUp))
        {
            keyPadUp_Listener_Down();
        }
        if (keyPadUp_Listener_Pressing != null && rewired.GetButton(InputActionName.DPadUp))
        {
            keyPadUp_Listener_Pressing();
        }
        if (keyPadUp_Listener_Up != null && rewired.GetButtonUp(InputActionName.DPadUp))
        {
            keyPadUp_Listener_Up();
        }
        // DPad down
        if (keyPadDown_Listener_Down != null && rewired.GetButtonDown(InputActionName.DPadDown))
        {
            keyPadDown_Listener_Down();
        }
        if (keyPadDown_Listener_Pressing != null && rewired.GetButton(InputActionName.DPadDown))
        {
            keyPadDown_Listener_Pressing();
        }
        if (keyPadDown_Listener_Up != null && rewired.GetButtonUp(InputActionName.DPadDown))
        {
            keyPadDown_Listener_Up();
        }
        // DPad left
        if (keyPadLeft_Listener_Down != null && rewired.GetButtonDown(InputActionName.DPadLeft))
        {
            keyPadLeft_Listener_Down();
        }
        if (keyPadLeft_Listener_Pressing != null && rewired.GetButton(InputActionName.DPadLeft))
        {
            keyPadLeft_Listener_Pressing();
        }
        if (keyPadLeft_Listener_Up != null && rewired.GetButtonUp(InputActionName.DPadLeft))
        {
            keyPadLeft_Listener_Up();
        }
        // DPad right
        if (keyPadRight_Listener_Down != null && rewired.GetButtonDown(InputActionName.DPadRight))
        {
            keyPadRight_Listener_Down();
        }
        if (keyPadRight_Listener_Pressing != null && rewired.GetButton(InputActionName.DPadRight))
        {
            keyPadRight_Listener_Pressing();
        }
        if (keyPadRight_Listener_Up != null && rewired.GetButtonUp(InputActionName.DPadRight))
        {
            keyPadRight_Listener_Up();
        }

        // key up
        if (keyUp_Listener_Down != null && rewired.GetButtonDown(InputActionName.Up))
        {
            keyUp_Listener_Down();
        }
        if (keyUp_Listener_Pressing != null && rewired.GetButton(InputActionName.Up))
        {
            keyUp_Listener_Pressing();
        }
        if (keyUp_Listener_Up != null && rewired.GetButtonUp(InputActionName.Up))
        {
            keyUp_Listener_Up();
        }
        // key down
        if (keyDown_Listener_Down != null && rewired.GetButtonDown(InputActionName.Down))
        {
            keyDown_Listener_Down();
        }
        if (keyDown_Listener_Pressing != null && rewired.GetButton(InputActionName.Down))
        {
            keyDown_Listener_Pressing();
        }
        if (keyDown_Listener_Up != null && rewired.GetButtonUp(InputActionName.Down))
        {
            keyDown_Listener_Up();
        }
        // key left
        if (keyLeft_Listener_Down != null && rewired.GetButtonDown(InputActionName.Left))
        {
            keyLeft_Listener_Down();
        }
        if (keyLeft_Listener_Pressing != null && rewired.GetButton(InputActionName.Left))
        {
            keyLeft_Listener_Pressing();
        }
        if (keyLeft_Listener_Up != null && rewired.GetButtonUp(InputActionName.Left))
        {
            keyLeft_Listener_Up();
        }
        // key right
        if (keyRight_Listener_Down != null && rewired.GetButtonDown(InputActionName.Right))
        {
            keyRight_Listener_Down();
        }
        if (keyRight_Listener_Pressing != null && rewired.GetButton(InputActionName.Right))
        {
            keyRight_Listener_Pressing();
        }
        if (keyRight_Listener_Up != null && rewired.GetButtonUp(InputActionName.Right))
        {
            keyRight_Listener_Up();
        }

        // key back
        if (keyBack_Listener_Down != null && rewired.GetButtonDown(InputActionName.Back))
        {
            keyBack_Listener_Down();
        }
        if (keyBack_Listener_Pressing != null && rewired.GetButton(InputActionName.Back))
        {
            keyBack_Listener_Pressing();
        }
        if (keyBack_Listener_Up != null && rewired.GetButtonUp(InputActionName.Back))
        {
            keyBack_Listener_Up();
        }
        // key start
        if (keyStart_Listener_Down != null && rewired.GetButtonDown(InputActionName.Start))
        {
            keyStart_Listener_Down();
        }
        if (keyStart_Listener_Pressing != null && rewired.GetButton(InputActionName.Start))
        {
            keyStart_Listener_Pressing();
        }
        if (keyStart_Listener_Up != null && rewired.GetButtonUp(InputActionName.Start))
        {
            keyStart_Listener_Up();
        }
    }

    // stick left
    public static void ListenStickLeft(StickCallback cb)
    {
        instance.stickLeft_Listener += cb;
    }
    // stick right
    public static void ListenStickRight(StickCallback cb)
    {
        instance.stickRight_Listener += cb;
    }
    // trigger left
    public static void ListenTriggerLeft(TriggerCallback cb)
    {
        instance.triggerLeft_Listener += cb;
    }
    // trigger right
    public static void ListenTriggerRight(TriggerCallback cb)
    {
        instance.triggerRight_Listener += cb;
    }
    // bumper left
    public static void ListenBumperLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.bumperLeft_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.bumperLeft_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.bumperLeft_Listener_Down += cb;
        }
    }
    // bumper right
    public static void ListenBumperRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.bumperRight_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.bumperRight_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.bumperRight_Listener_Up += cb;
        }
    }
    // pad
    public static void ListenKeyPadUp(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyPadUp_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyPadUp_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyPadUp_Listener_Up += cb;
        }
    }
    public static void ListenKeyPadDown(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyPadDown_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyPadDown_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyPadDown_Listener_Up += cb;
        }
    }
    public static void ListenKeyPadLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyPadLeft_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyPadLeft_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyPadLeft_Listener_Up += cb;
        }
    }
    public static void ListenKeyPadRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyPadRight_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyPadRight_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyRight_Listener_Up += cb;
        }
    }
    // directions
    public static void ListenKeyUp(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyUp_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyUp_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyUp_Listener_Up += cb;
        }
    }
    public static void ListenKeyDown(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyDown_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyDown_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyDown_Listener_Up += cb;
        }
    }
    public static void ListenKeyLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyLeft_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyLeft_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyLeft_Listener_Up += cb;
        }
    }
    public static void ListenKeyRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyRight_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyRight_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyRight_Listener_Up += cb;
        }
    }

    // functions
    public static void ListenKeyBack(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyBack_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyBack_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyBack_Listener_Up += cb;
        }
    }
    public static void ListenKeyStart(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyStart_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyStart_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyStart_Listener_Up += cb;
        }
    }
    // stick key    
    public static void ListenKeyStickLeft(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyStickLeft_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyStickLeft_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyStickLeft_Listener_Up += cb;
        }
    }
    public static void ListenKeyStickRight(KeyCallback cb, EKeyAction eAct)
    {
        if ((eAct & EKeyAction.keyDown) == EKeyAction.keyDown)
        {
            instance.keyStickRight_Listener_Down += cb;
        }
        if ((eAct & EKeyAction.keyPressing) == EKeyAction.keyPressing)
        {
            instance.keyStickRight_Listener_Pressing += cb;
        }
        if ((eAct & EKeyAction.keyUp) == EKeyAction.keyUp)
        {
            instance.keyStickRight_Listener_Up += cb;
        }
    }
}
