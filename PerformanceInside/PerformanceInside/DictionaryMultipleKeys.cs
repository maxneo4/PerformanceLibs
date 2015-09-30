using System;

namespace Neo.PerformanceInside
{
    internal class DictionaryMultipleKeys
    {
        #region Field

        private readonly string _objectsRepresentation;
        private object[] _objects;

        #endregion

        public DictionaryMultipleKeys(params object[] objects)
        {
            _objects = objects;
            _objectsRepresentation = string.Join("#", objects);
        }

        public object[] GetObjects
        {
            get { return _objects; }
        }

        public override int GetHashCode()
        {
            return _objectsRepresentation.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is DictionaryMultipleKeys && obj.GetHashCode() == GetHashCode();
        }
    }
}
