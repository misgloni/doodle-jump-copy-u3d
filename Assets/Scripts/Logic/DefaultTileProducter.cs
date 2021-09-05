using System;
using Random = UnityEngine.Random;

namespace Sources.Scripts.Logic
{
    public class DefaultTileProducter : ITileProducter
    {
        private float _fromHigh;
        private float _toHigh;
        private float _maxInterval; //每个间隔最大高度
        private float _minInterval;
        private float _shrink;
        private float _currentHigh;
        private bool _isFirst = true;

        public float CurrentHigh => _currentHigh;

        public DefaultTileProducter(float fromHigh, float toHigh, float maxInterval, float minInterval, float shrink)
        {
            _fromHigh = fromHigh;
            _toHigh = toHigh;
            _maxInterval = maxInterval;
            _minInterval = minInterval;
            _shrink = shrink;
            _currentHigh = fromHigh;
        }

        public TileInfo Product()
        {
            TileInfo ret = new TileInfo();
            string tileName;
            if (_isFirst) //等于的时候直接创建一个基础Tile
            {
                tileName = "BaseTile";
                _isFirst = false;
            }
            else
            {
                _currentHigh += Random.Range(_minInterval, _maxInterval);
                //处理TileName
                tileName = "BaseTile";
            }

            if (_currentHigh >= _toHigh)
            {
                return null;
            }
            ret.High = _currentHigh;
            ret.Width = Random.Range(0 + _shrink, 1 - _shrink);
            ret.TileName = tileName;
            return ret;
        }
    }
}