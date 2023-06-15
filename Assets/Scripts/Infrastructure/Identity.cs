using UnityEngine;

namespace Infrastructure
{
    public class Identity
    {
        public enum Camps
        {
            Red,
            Blue,
            Other,
            Unknown
        }

        public enum Roles
        {
           Nothing,
           Hero,
           Engineer,
           Infantry
        }
        
        [SerializeField] public Camps camp;

        [SerializeField] public Roles role;

        [SerializeField] public int serial;
        
        /// <summary>
        /// 默认初始化函数。
        /// </summary>
        public Identity()
        {
            camp = Camps.Other;
            role = Roles.Nothing;
            serial = 0;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="camp">实体所属阵营</param>
        /// <param name="role">实体类型</param>
        /// <param name="serial">实体序号</param>
        public Identity(Camps camp = Camps.Other, Roles role = Roles.Nothing, int serial = 0)
        {
            this.camp = camp;
            this.role = role;
            this.serial = serial;
        }
    }
}