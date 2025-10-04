using System.Collections.Generic;
using Architecture.Units.Core;
using UnityEngine;

namespace Architecture
{
    
    public interface IUnitManager
    {
        List<BaseUnit> UnitList { get; }
        void AddUnit(BaseUnit unit);
        void RemoveUnit(BaseUnit unit);
    }
    
    public class UnitManager : IUnitManager
    {
        private List<BaseUnit> _unitList = new List<BaseUnit>();
        public List<BaseUnit> UnitList => _unitList;
        
        public void AddUnit(BaseUnit unit)
        {
            if (!_unitList.Contains(unit))
                _unitList.Add(unit);
        }

        public void RemoveUnit(BaseUnit unit)
        {
            if (_unitList.Contains(unit))
                _unitList.Remove(unit);
        }
    }
}