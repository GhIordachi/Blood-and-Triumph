using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemy);
    }
}
