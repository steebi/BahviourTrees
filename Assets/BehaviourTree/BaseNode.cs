using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{

    public interface IBaseNode
    {
        // called on initialization
        void OnInitialize();
        // called on termination
        void OnTermination();
        Status OnTick();
    }

}
