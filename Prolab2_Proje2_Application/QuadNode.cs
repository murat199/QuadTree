using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Prolab2_Proje2_Application
{
    /// <summary>
    /// Prolab2_Proje2_Application.QuadNode sınıfı
    /// Her düğümün değelerini tutar
    /// </summary>
    public class QuadNode
    {
        public int x;
        public int y;

        public int OrX1;
        public int OrY1;
        public int OrX2;
        public int OrY2;

        public Color NodeColor;

        public QuadNode KuzeyDogu;
        public QuadNode KuzeyBati;
        public QuadNode GuneyDogu;
        public QuadNode GuneyBati;

        /// <summary>
        /// Eklenen yeni düğümün bilgilerini tutar
        /// </summary>
        /// <param name="x">Ağaca eklenecek düğümün x koordinatı</param>
        /// <param name="y">Ağaca eklenecek düğümün y koordinatı</param>
        /// <param name="x1">Ağaca eklenecek düğümün baş x koordinatı</param>
        /// <param name="y1">Ağaca eklenecek düğümün baş y koordinatı</param>
        /// <param name="x2">Ağaca eklenecek düğümün son x koordinatı</param>
        /// <param name="y2">Ağaca eklenecek düğümün son y koordinatı</param>
        /// <param name="R">Ağaca eklenecek düğümün R renk değeri</param>
        /// <param name="G">Ağaca eklenecek düğümün G renk değeri</param>
        /// <param name="B">Ağaca eklenecek düğümün B renk değeri</param>
        public QuadNode(int x, int y, int x1, int y1, int x2, int y2, int R, int G, int B)
        {
            this.x = x;
            this.y = y;
            this.OrX1 = x1;
            this.OrX2 = x2;
            this.OrY1 = y1;
            this.OrY2 = y2;

            NodeColor = Color.FromArgb(R, G, B);

            KuzeyBati = null;
            KuzeyDogu = null;
            GuneyBati = null;
            GuneyDogu = null;
        }
    }
}
