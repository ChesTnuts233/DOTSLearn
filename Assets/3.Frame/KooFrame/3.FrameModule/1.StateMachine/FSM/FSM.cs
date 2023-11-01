using System;
using System.Collections.Generic;

namespace KooFrame.Module
{
    /// <summary>
    /// 状态机宿主接口
    /// 本行为树 默认宿主有一个控制宿主主要逻辑的Ctrl字段
    /// </summary>
    public interface IFSMOwner<TOwner> where TOwner : class
    {
        TOwner ownerCtrl { get; }
        string curStateName { set; } //用来查看状态的属性
    }
    

    /// <summary>
    /// 有限状态机 Finite-state Machine
    /// </summary>
    public class FSM<TOwner> where TOwner : class
    {
        /// <summary>
        /// 状态字典 Key:状态枚举的值，Value 具体状态
        /// </summary>
        private Dictionary<Type, FSMState<TOwner>> stateDic = new();


        /// <summary>
        /// 当前状态
        /// </summary>
        public Type CurMainStateType { get; private set; } = null;

        //public Type CurSubStateType 

        /// <summary>
        /// 当前生效的状态
        /// </summary>
        private FSMState<TOwner> curState;

        /// <summary>
        /// 宿主
        /// </summary>
        private TOwner owner;

        private IFSMOwner<TOwner> ownerInterface;

        /// <summary>
        /// 状态共享数据字典
        /// </summary>
        private Dictionary<string, object> stateShareDataDic;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="owner">宿主</param>
        /// <param name="enableStateShareData">是否启用状态共享数据，需要注意存在装箱 拆箱的情况</param>
        /// <typeparam name="T">初始状态类型</typeparam>
        public void Init<T>(TOwner owner, bool enableStateShareData = false) where T : FSMState<TOwner>, new()
        {
            this.owner = owner;
            ownerInterface = owner as IFSMOwner<TOwner>;
            if (enableStateShareData) stateShareDataDic = new Dictionary<string, object>();
            ChangeState<T>();
        }

        /// <summary>
        /// 初始化（无默认状态，状态机待机）
        /// </summary>
        /// <param name="owner">宿主</param>
        public void Init(TOwner owner, bool enableStateShareData = false)
        {
            if (enableStateShareData) stateShareDataDic = new Dictionary<string, object>();
            this.owner = owner;
        }


        #region 状态方法

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T">具体要切换到的状态脚本类型</typeparam>
        /// <param name="newState">新状态</param>
        /// <param name="reCurState">新状态和当前状态一致的情况下，是否也要切换</param>
        /// <returns></returns>
        public bool ChangeState<T>(bool reCurState = false) where T : FSMState<TOwner>, new()
        {
            Type stateType = typeof(T);
            // 状态一致，并且不需要刷新状态，则切换失败
            if (stateType == CurMainStateType && !reCurState) return false;

            // 退出当前状态
            if (curState != null)
            {
                curState.Exit();
                curState.RemoveUpdate(curState.Update);
                curState.RemoveLateUpdate(curState.LateUpdate);
                curState.RemoveFixedUpdate(curState.FixedUpdate);
            }

            // 进入新状态
            curState = GetOrNewState<T>();
            CurMainStateType = stateType;
            curState.Enter();
            curState.AddUpdate(curState.Update);
            curState.AddLateUpdate(curState.LateUpdate);
            curState.AddFixedUpdate(curState.FixedUpdate);
            ownerInterface.curStateName = stateType.Name;

            return true;
        }

        /// <summary>
        /// 从对象池获取、或添加一个新的状态 
        /// </summary>
        private FSMState<TOwner> GetOrNewState<T>() where T : FSMState<TOwner>, new()
        {
            Type stateType = typeof(T);
            if (stateDic.TryGetValue(stateType, out var st)) return st;
            FSMState<TOwner> state = ResSystem.GetOrNew<T>();
            state.InitInternalData(this);
            state.Init(owner);
            stateDic.Add(stateType, state);
            return state;
        }

        /// <summary>
        /// 移除状态 回收入对象池
        /// </summary>
        /// <typeparam name="T">需要移除的状态</typeparam>
        private bool RemoveState<T>()
        {
            Type stateType = typeof(T);
            if (!stateDic.ContainsKey(stateType)) return false; //如果不存在状态 
            //如果不是当前状态
            if (stateType != CurMainStateType && stateDic.ContainsKey(stateType))
            {
                stateDic.Remove(stateType);
                curState.ObjectPushPool();
                return true;
            }

            //如果此状态是当前状态 处理当前状态的额外逻辑
            if (stateType == CurMainStateType && curState != null)
            {
                curState.Exit();
                curState.RemoveUpdate(curState.Update);
                curState.RemoveLateUpdate(curState.LateUpdate);
                curState.RemoveFixedUpdate(curState.FixedUpdate);
                CurMainStateType = null;
                curState.ObjectPushPool();
                return true;
            }

            // 其他情况
            return false;
        }


        /// <summary>
        /// 停止工作
        /// 把所有状态都释放，但是状态机本身FSM未来还可以工作
        /// </summary>
        public void Stop()
        {
            // 处理当前状态的额外逻辑
            if (curState != null)
            {
                curState.Exit();
                curState.RemoveUpdate(curState.Update);
                curState.RemoveLateUpdate(curState.LateUpdate);
                curState.RemoveFixedUpdate(curState.FixedUpdate);
                curState = null;
            }

            CurMainStateType = null;
            // 处理缓存中所有状态的逻辑
            foreach (var state in stateDic.Values)
            {
                state.UnInit();
            }

            stateDic.Clear();
        }

        #endregion

        #region 状态共享数据

        //共享数据可以在状态类内使用，也可以通过stateMachine调用API进行增删改查

        /// <summary>
        /// 尝试获取共享的数据
        /// </summary>
        /// <param name="key">需要共享的数据的Key</param>
        /// <param name="data">共享的数据</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetShareData<T>(string key, out T data)
        {
            bool res = stateShareDataDic.TryGetValue(key, out object stateData);
            if (res)
            {
                data = (T)stateData;
            }
            else
            {
                data = default(T);
            }

            return res;
        }

        public void AddShareData(string key, object data)
        {
            stateShareDataDic.Add(key, data);
        }

        public bool RemoveShareData(string key)
        {
            return stateShareDataDic.Remove(key);
        }

        public bool ContainsShareData(string key)
        {
            return stateShareDataDic.ContainsKey(key);
        }

        public bool UpdateShareData(string key, object data)
        {
            if (ContainsShareData(key))
            {
                stateShareDataDic[key] = data;
                return true;
            }
            else return false;
        }

        public void CleanShareData()
        {
            stateShareDataDic?.Clear();
        }

        #endregion


        /// <summary>
        /// 销毁，宿主应该释放掉StateMachine的引用
        /// </summary>
        public void Destroy()
        {
            // 处理所有状态
            Stop();
            // 清除共享数据
            CleanShareData();
            // 放弃所有资源的引用
            owner = null;
            // 放进对象池
            this.ObjectPushPool();
        }


        // /// <summary>
        // /// 添加状态
        // /// </summary>
        // /// <param name="state"></param>
        // public void AddState(FSMState state)
        // {
        //     if (state == null)
        //     {
        //         Debug.LogError("FSMState不能为空");
        //         return;
        //     }
        //
        //     if (curState == null)
        //     {
        //         curState = state;
        //         currentStateID = state.StateID;
        //     }
        //
        //     if (stateDic.ContainsKey(state.StateID))
        //     {
        //         Debug.LogError("状态" + state.StateID + "已经存在，无法重复添加");
        //         return;
        //     }
        //
        //     stateDic.Add(state.StateID, state);
        // }
        //
        // /// <summary>
        // /// 删除状态
        // /// </summary>
        // /// <param name="id"></param>
        // public void DeleteState(StateID id)
        // {
        //     if (id == StateID.NullStateID)
        //     {
        //         Debug.LogError("无法删除空状态");
        //         return;
        //     }
        //
        //     if (stateDic.ContainsKey(id) == false)
        //     {
        //         Debug.LogError("无法删除不存在的状态：" + id);
        //         return;
        //     }
        //
        //     stateDic.Remove(id);
        // }
        //
        // public void PerformTransition(Transition transform)
        // {
        //     if (transform == Transition.NullTransition)
        //     {
        //         Debug.LogError("无法执行空的转换条件");
        //         return;
        //     }
        //
        //     StateID id = curState.GetOutputState(transform);
        //     if (id == StateID.NullStateID)
        //     {
        //         Debug.LogWarning("当前状态" + currentStateID + "无法根据转换条件" + transform + "发生转换");
        //         return;
        //     }
        //
        //     if (stateDic.ContainsKey(id) == false)
        //     {
        //         Debug.LogError("在状态机里面不存在状态" + id + "，无法进行状态转换！");
        //         return;
        //     }
        //
        //     FSMState state = stateDic[id];
        //     curState.DoAfterLeaving();
        //     curState = state;
        //     currentStateID = id;
        //     curState.DoBeforeEntering();
        // }
    }
}