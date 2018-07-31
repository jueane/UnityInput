using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System;

namespace Gameplay
{
    public class TestInput : MonoBehaviour
    {
        public Text tx1;
        public Text tx2;
        public Text tx3;
        public Text tx4;
        public Text tg1;

        public Text tx5;
        public Text tx6;
        public Text tx7;

        public Text up;
        public Text down;
        public Text left;
        public Text right;


        // BODPlayerInput playerinput;

        public Text joyList;

        Player player;

        // Use this for initialization
        void Start()
        {
            player = ReInput.players.GetPlayer(0);

            PlayerInput.ListenStickLeft(OnMove);
            PlayerInput.ListenStickRight(OnLook);

            PlayerInput.ListenTriggerLeft(OnTriggerLeft);

            PlayerInput.ListenTriggerRight(OnTriggerRight);

            PlayerInput.ListenKeyUp(OnKeyUp, EKeyAction.keyDown);
            PlayerInput.ListenKeyDown(OnKeyDown, EKeyAction.keyDown);
            PlayerInput.ListenKeyLeft(OnKeyLeft, EKeyAction.keyDown);
            PlayerInput.ListenKeyRight(OnKeyRight, EKeyAction.keyDown);
        }

        int lastCountJoys = 0;

        // Update is called once per frame
        void Update()
        {
        }

        void OnMove(Vector2 vec)
        {
            tx1.text = "移动：  " + vec;
        }
        void OnLook(Vector2 vec)
        {
            tx2.text = "寻找：  " + vec;
        }

        void OnTriggerLeft(float a)
        {
            tx3.text = "trigger left: " + a;
        }

        void OnTriggerRight(float a)
        {
        }

        void OnKeyAPress()
        {

            print("key a has been pressed.");
            // pInput.keyAListener -= OnKeyAPress;

        }
        void OnKeyUp()
        {
            print("press up");
        }
        void OnKeyDown()
        {
            print("press down");
        }
        void OnKeyLeft()
        {
            print("press left");
        }
        void OnKeyRight()
        {
            print("press right");
        }

    }
}

