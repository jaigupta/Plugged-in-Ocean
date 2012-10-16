using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Basics;
using Slb.Ocean.Geometry;

namespace GeoCommunication
{
    class CONSTS
    {
        public static int MAX_USERS = 30;
        public static int CHAT_PORT = 1986;
        public static int MAX_TRANSFER = 1024 * 10 ;
        public static int TIME_WAIT_TRANSFER = 50;
    }
    enum OBJTYPE
    {
        FILE,
        SIESMIC_CUBE,
        WELL_LOG
    }
    [Serializable]
    class tryclass
    {
        public int i;
        public char c;
        public tryclass(int i, char c)
        {
            this.i = i;
            this.c = c;
        }
    }
    [Serializable]
    class SerializableSeismicCube
    {
        public SeismicCube cube;
        public SerializableSeismicCube(SeismicCube cube)
        {
            this.cube = cube;
        }
    }
}
