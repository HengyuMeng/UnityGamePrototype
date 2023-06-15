using System.Collections.Generic;
using System.Linq;
using Gameplay.Events;
using Infrastructure;
using UnityEngine;

namespace Controllers
{
    public class SphereStoreBase : StoreBase
    {
        public Rigidbody rb;
        public override void Start()
        {
            base.Start();
            rb = gameObject.GetComponent<Rigidbody>();
        }
        
        //移动组件
        public Transform moveElement;
        
        //视角旋转组件
        public Transform rotateElement;
        
        
        /// <summary>
        /// 声明接收事件。
        /// </summary>
        /// <returns></returns>
        public override List<string> InputActions()
        {
            return base.InputActions().Concat(new List<string>
            {
                // 输入
                ActionID.Input.PrimaryAxis,
                ActionID.Input.ViewControl,
                ActionID.Input.StateControl
            }).ToList();
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void Receive(IAction action)
        {
            base.Receive(action);
            switch (action.ActionName())
            {
                case ActionID.Input.PrimaryAxis:
                    var act = (PrimaryAxis) action;
                    if (act.Receiver == id)
                    { 
                        var move = new Vector3(act.X, 0, act.Y) * Time.fixedDeltaTime * 3;
                        moveElement.transform.position = rotateElement.transform.TransformPoint(move);
                    }
                    break;

                case ActionID.Input.ViewControl:
                    var viewControlAction = (ViewControl) action;
                    if (viewControlAction.Receiver == id)
                    {
                        // 控制视角
                        var rotation = new Vector3(viewControlAction.Y * -1, viewControlAction.X, 0) * Time.fixedDeltaTime;
                        rotateElement.Rotate(rotation);
                        rotateElement.Rotate(Vector3.back * rotateElement.rotation.eulerAngles.z);
                    }
                    break;
            }
        }
    }
}