using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_4_ksis
{
    class FtpClient
    {
        private string _UserName = "";
        private string _Password = "";
        private string _Host = "";

        public string UserName {
            get { return _UserName; }
            set { if (value != null)
                    _UserName = value;
            }
        }

        public string Password
        {
            get { return _Password; }
            set { if (value != null) _Password = value; }
        }

        public string Host
        {
            get { return _Host; }
            set { if (value != null) _Host = value; }
        }

    }
}
