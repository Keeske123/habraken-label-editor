using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habraken_SLE
{
    public class Barcode : Element
    {
        public Barcode()
        {
            throw new System.NotImplementedException();
        }

        public int font
        {
            get;
            set;
        }

        public string ratio
        {
            get;
            set;
        }

        public int type
        {
            get;
            set;
        }

        public int code
        {
            get;
            set;
        }

        public Toolbox Toolbox
        {
            get;
            set;
        }

        public void GenereateBarcode()
        {
            throw new System.NotImplementedException();
        }
    }
}