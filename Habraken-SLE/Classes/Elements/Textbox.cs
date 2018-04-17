using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habraken_SLE
{
    public class Textbox : Element
    {
        public Textbox()
        {
            throw new System.NotImplementedException();
        }

        public string fontType
        {
            get;set;
        }

        public int fontSize
        {
            get; set;

        }

        public string fontWeight
        {
            get; set;

        }

        public Toolbox Toolbox
        {
            get; set;

        }
    }
}