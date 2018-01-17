using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseMapGenerator.Models
{
    public class XmlNoiseData
    {
        private int _size;

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        private int _dimensions;

        public int Dimensions
        {
            get { return _dimensions; }
            set { _dimensions = value; }
        }

        private List<float> _noiseData;

        public List<float> NoiseData
        {
            get { return _noiseData; }
            set { _noiseData = value; }
        }
    }
}
