using System.Collections.Generic;
using Infrastructure;

namespace Infrastructure
{
    /// <summary>
    /// 帧内发送缓存，用于游戏进行时通过Dispatcher发送指令。
    /// </summary>
    public class SendCache
    {
        public IAction action;
        public Identity owner;
    }
    /// <summary>
    /// <c>Dispatcher</c> 负责转发系统中的所有事件。
    /// <br/>每个 <c>StoreBase</c> 都会注册到此处。
    /// </summary>
    public class Dispatcher : Singleton<Dispatcher>
    {
        
        /// <summary>
        /// 用于Dispatcher储存注册对某一个action感兴趣的所有StoreBase。
        /// </summary>
        private readonly Dictionary<string, List<StoreBase>> _stores = new Dictionary<string, List<StoreBase>>();

        /// <summary>
        /// 声明队列在Dispatcher中发送指令。
        /// </summary>
        private readonly Queue<SendCache> _sendCache = new Queue<SendCache>();
        
        
        /// <summary>
        /// 以 <c>StoreBase</c> 订阅的action来将其注册到 <c>Dispatcher</c>的字典中。
        /// 目的是Dispatcher可以根据StoreBase的订阅情况来分发action。
        /// </summary>
        /// <param name="store"><c>StoreBase</c></param>
        public void Register(StoreBase store)
        {
            // 将每个注册对象感兴趣的action存到一个字典中。
            // 用于之后分发action的筛选(只把事件分发给对订阅了action的store)。
            foreach (var action in store.InputActions())
            {   
                // 每一个字符串对应着对他“感兴趣”的一些对象
                if (_stores.ContainsKey(action))
                {
                    _stores[action].Add(store);
                }
                else
                {   
                    //将一个 store 对象添加到 _stores 字典中的一个列表中，使用 某个action 作为键。
                    _stores[action] = new List<StoreBase> { store };
                }
            }
        }
        
        /// <summary>
        /// 通过此 <c>Dispatcher</c> 发送事件。
        /// </summary>
        /// <param name="action">要发送的事件</param>
        public void Send(IAction action) 
        {

            // 当触发事件时，将事件入队到_sendCache，之后在FixedUpdate中进行处理
            _sendCache.Enqueue(new SendCache
            {
                action = action,
                owner = null,
            });
        }

        private void FixedUpdate()
        {
            while (_sendCache.Count>0)
            {
                var send = _sendCache.Dequeue();
                if (_stores.ContainsKey(send.action.ActionName()))
                {
                    foreach (var store in _stores[send.action.ActionName()])
                    {
                        // 调用store的Receive函数分发action
                        store.Receive(send.action);
                    }
                }
            }
        }
    }
    
}
