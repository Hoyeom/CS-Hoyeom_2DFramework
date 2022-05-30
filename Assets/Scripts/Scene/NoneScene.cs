using Contents;
using Manager.Core;
using UnityEngine;

namespace DefaultNamespace.Manager
{
    public class NoneScene : BaseScene
    {
        protected override void Initialize()
        {
            base.Initialize();
            Map map = new Map(10, 10);
        }

        public override void Clear()
        {
            
        }
    }
}