using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habraken_SLE
{
    public class Element
    {
        public Element()
        {
            throw new System.NotImplementedException();
        }

        protected int posX
        {
            get; set;

        }

        protected int posY
        {
            get; set;

        }

        protected int height
        {
            get; set;

        }

        protected int width
        {
            get; set;

        }

        protected int rotation
        {
            get; set;

        }

        protected int elementID
        {
            get; set;

        }

        public void Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}