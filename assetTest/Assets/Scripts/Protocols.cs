using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocols
{
    public class Packets
    {
        public class req_Login {
            public string id;
            public string pw;
        }
        public class res_Login {
            public SoloData result;
        }


        public class req_Register {
            public string id;
            public string pw;
        }
        public class res_Register {
            public bool result;
        }

        public class req_UpdateSolo {
            public string id;
            public int newScore;
        }
        public class res_UpdateSolo {
        }

        public class req_UpdateDuo {
            public string id;
            public int newScore;
        }
        public class res_UpdateDuo {
        }
    }
}