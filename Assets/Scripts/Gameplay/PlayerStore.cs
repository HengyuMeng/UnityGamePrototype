using System;
using Gameplay.Events;
using Infrastructure;
using Infrastructure.Input;
using Misc;
using UnityEngine;

namespace Gameplay
{
    public class PlayerStore : StoreBase
    {
        public GameObject controlledObject;
        public StoreBase localRobot;
        public int sensitivity = 5;

        protected void FixedUpdate()
        {
            // base.FixedUpdate();
            // 打包输入数据再进行发送
            var primaryAxis = BitUtil.CompressAxisInput(InputManager.Instance().primaryAxis);
            var secondaryAxis = BitUtil.CompressAxisInput(InputManager.Instance().secondaryAxis);
            var input = new BitMap32();

            input.SetByte(2, primaryAxis);
            input.SetByte(3, secondaryAxis);
            input.SetBit(2, InputManager.Instance().ButtonStatus[InputActionID.FunctionA]);
            input.SetBit(3, InputManager.Instance().ButtonStatus[InputActionID.FunctionB]);
            input.SetBit(4, InputManager.Instance().ButtonStatus[InputActionID.FunctionC]);
            input.SetBit(5, InputManager.Instance().ButtonStatus[InputActionID.FunctionD]);
            input.SetBit(6, InputManager.Instance().ButtonStatus[InputActionID.FunctionE]);
            input.SetBit(7, InputManager.Instance().ButtonStatus[InputActionID.FunctionF]);
            input.SetBit(8, InputManager.Instance().ButtonStatus[InputActionID.FunctionG]);
            input.SetBit(9, InputManager.Instance().ButtonStatus[InputActionID.FunctionH]);
            input.SetBit(10, InputManager.Instance().ButtonStatus[InputActionID.FunctionI]);
            input.SetBit(11, InputManager.Instance().ButtonStatus[InputActionID.FunctionJ]);
            input.SetBit(12, InputManager.Instance().ButtonStatus[InputActionID.FunctionK]);
            input.SetBit(13, InputManager.Instance().ButtonStatus[InputActionID.FunctionL]);
            input.SetBit(14, InputManager.Instance().ButtonStatus[InputActionID.FunctionM]);
            input.SetBit(0, InputManager.Instance().ButtonStatus[InputActionID.Fire]);
            input.SetBit(1, InputManager.Instance().ButtonStatus[InputActionID.SecondaryFire]);
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                CmdSendViewInput(InputManager.Instance().RawView() * sensitivity);
            }

            CmdSendInput(input.GetData());
            
        }
        
        /// <summary>
        /// 发送视角控制数据。
        /// </summary>
        /// <param name="data">输入数据</param>
        private void CmdSendViewInput(Vector2 data)
        {
            Dispatcher.Instance().Send(new ViewControl
            {
                Receiver = localRobot.id,
                X = data.x,
                Y = data.y
            });
        }

        /// <summary>
        /// 发送其他输入数据。
        /// </summary>
        /// <param name="data">输入数据</param>
        private void CmdSendInput(uint data)
        {
            var input = new BitMap32(data);
            var fire = input.GetBit(0);
            var secondaryFire = input.GetBit(1);
            var functionA = input.GetBit(2);
            var functionB = input.GetBit(3);
            var functionC = input.GetBit(4);
            var functionD = input.GetBit(5);
            var functionE = input.GetBit(6);
            var functionF = input.GetBit(7);
            var functionG = input.GetBit(8);
            var functionH = input.GetBit(9);
            var functionI = input.GetBit(10);
            var functionJ = input.GetBit(11);
            var functionK = input.GetBit(12);
            var functionL = input.GetBit(13);
            var functionM = input.GetBit(14);
            var primaryAxis = BitUtil.DecompressAxisInput(input.GetByte(2));
            var secondaryAxis = BitUtil.DecompressAxisInput(input.GetByte(3));
            Debug.Log($"{primaryAxis.x}");
            Dispatcher.Instance().Send(new PrimaryAxis
            {
                Receiver = localRobot.id,
                X = primaryAxis.x,
                Y = primaryAxis.y
            });
            Dispatcher.Instance().Send(new SecondaryAxis
            {
                Receiver = localRobot.id,
                X = secondaryAxis.x,
                Y = secondaryAxis.y
            });
            Dispatcher.Instance().Send(new StateControl
            {
                Receiver = localRobot.id,
                Fire = fire,
                SecondaryFire = secondaryFire,
                FunctionA = functionA,
                FunctionB = functionB,
                FunctionC = functionC,
                FunctionD = functionD,
                FunctionE = functionE,
                FunctionF = functionF,
                FunctionG = functionG,
                FunctionH = functionH,
                FunctionI = functionI,
                FunctionJ = functionJ,
                FunctionK = functionK,
                FunctionL = functionL,
                FunctionM = functionM
            });
        }
    }
}