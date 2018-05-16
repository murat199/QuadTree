using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolab2_Proje2_Application
{
    /// <summary>
    /// Prolab2_Proje2_Application.QuadTree sınıfı
    /// Dörtlü ağacın oluşturulmasını sağlar
    /// Add metodu içerir
    /// AddR metodu içerir
    /// ColorAddList metodu içerir
    /// RGBClear metodu içerir
    /// </summary>
    public class QuadTree
    {
        public QuadNode root;
        public QuadNode tempNode;
        public int totalNode = 0;
        private Random rand = new Random();
        private ArrayList R = new ArrayList();
        private ArrayList G = new ArrayList();
        private ArrayList B = new ArrayList();
        private ArrayList IndexId = new ArrayList();
        
        /// <summary>
        /// Ağacın root düğümüne boş değer atanır
        /// </summary>
        public QuadTree()
        {
            root = null;
        }
        
        /// <summary>
        /// Ağacın root düğümünü oluşturur
        /// </summary>
        /// <param name="initial_x">Ağaca eklenecek root'un x koordinatı</param>
        /// <param name="initial_y">Ağaca eklenecek root'un y koordinatı</param>
        public QuadTree(int initial_x, int initial_y)
        {
            RGBClear();
            ColorAddList(0);
            root = new QuadNode(initial_x, initial_y, 0, 0, 512, 512, (int)(R[0]), (int)(G[0]), (int)(B[0]));
        }
        
        /// <summary>
        /// Ağaca düğüm eklenmesini sağlar
        /// </summary>
        /// <param name="x">Eklecenek düğümün x koordinatı</param>
        /// <param name="y">Eklecenek düğümün y koordinatı</param>
        public void Add(int x, int y)
        {
            int stateNode = 0;
            if (root == null)
            {
                RGBClear();
                ColorAddList(0);
                totalNode++;
                root = new QuadNode(x, y, 0, 0, 512, 512, (int)(R[0]), (int)(G[0]), (int)(B[0]));
            }
            else
            {
                int colorIndex = 0;
                AddR(ref root, ref root, x, y, ref stateNode, ref colorIndex);
            }
        }
        
        /// <summary>
        /// Rekürsif olarak ağaçta dolaşılarak düğümün hangi bölgeye yerleştirileceği belirlenir
        /// </summary>
        /// <param name="Q">Rekürsif olarak dolaşılan düğümeleri tutar</param>
        /// <param name="topNode">Eklenecek düğümün bir üst düğümünü tutar</param>
        /// <param name="x">Eklecenek düğümün x koordinatı</param>
        /// <param name="y">Eklecenek düğümün y koordinatı</param>
        /// <param name="stateNode">Eklecenek düğümün kaçıncı bölgede olduğunu tutar</param>
        /// <param name="colorListIndex">Eklecenek düğümün renk index'ini tutar</param>
        public void AddR(ref QuadNode Q, ref QuadNode topNode, int x, int y, ref int stateNode, ref int colorListIndex)
        {
            if (Q == null)
            {
                totalNode++;
                if (stateNode == 1)
                {
                    ColorAddList(colorListIndex);
                    QuadNode newNode = new QuadNode(x, y, (topNode.x), (topNode.OrY1), (topNode.OrX2), (topNode.y), (int)(R[colorListIndex]), (int)(G[colorListIndex]), (int)(B[colorListIndex]));
                    Q = newNode;
                    tempNode = newNode;
                    return;
                }
                if (stateNode == 2)
                {
                    ColorAddList(colorListIndex);
                    QuadNode newNode = new QuadNode(x, y, (topNode.OrX1), (topNode.OrY1), (topNode.x), (topNode.y), (int)(R[colorListIndex]), (int)(G[colorListIndex]), (int)(B[colorListIndex]));
                    Q = newNode;
                    tempNode = newNode;
                    return;
                }
                if (stateNode == 3)
                {
                    ColorAddList(colorListIndex);
                    QuadNode newNode = new QuadNode(x, y, (topNode.OrX1), (topNode.y), (topNode.x), (topNode.OrY2), (int)(R[colorListIndex]), (int)(G[colorListIndex]), (int)(B[colorListIndex]));
                    Q = newNode;
                    tempNode = newNode;
                    return;
                }
                if (stateNode == 4)
                {
                    ColorAddList(colorListIndex);
                    QuadNode newNode = new QuadNode(x, y, (topNode.x), (topNode.y), (topNode.OrX2), (topNode.OrY2), (int)(R[colorListIndex]), (int)(G[colorListIndex]), Convert.ToInt16(B[colorListIndex]));
                    Q = newNode;
                    tempNode = newNode;
                    return;
                }
            }
            if (x > Q.x && y < Q.y && x < 512 && y > 0)
            {
                //1.bölge
                //KuzeyDoğu
                stateNode = 1;
                colorListIndex++;
                
            }
            if (x < Q.x && y < Q.y && x > 0 && y > 0)
            {
                //2.bölge
                //KuzeyBatı
                stateNode = 2;
                colorListIndex++;
                AddR(ref Q.KuzeyBati, ref Q, x, y, ref stateNode, ref colorListIndex);
            }
            if (x < Q.x && y > Q.y && x > 0 && y < 512)
            {
                //3.bölge
                //GüneyBatı
                stateNode = 3;
                colorListIndex++;
                AddR(ref Q.GuneyBati, ref Q, x, y, ref stateNode, ref colorListIndex);
            }
            if (x > Q.x && y > Q.y && x < 512 && y < 512)
            {
                //4.bölge
                //GüneyDoğu
                stateNode = 4;
                colorListIndex++;
                AddR(ref Q.GuneyDogu, ref Q, x, y, ref stateNode, ref colorListIndex);
            }
        }
        /// <summary>
        /// Rekürsif olarak düğümleri dolaşarak düğüm çocuklarını siler
        /// </summary>
        /// <param name="Q">Silinecek düğüm</param>
        public void DeleteNodes(ref QuadNode Q)
        {
            if (Q.KuzeyDogu != null)
            {
                DeleteNodes(ref Q.KuzeyDogu);
                Q.GuneyDogu = null;
                Q.GuneyBati = null;
                Q.KuzeyBati = null;
                Q.KuzeyDogu = null;
            }
            if (Q.KuzeyBati != null)
            {
                DeleteNodes(ref Q.KuzeyBati);
                Q.GuneyDogu = null;
                Q.GuneyBati = null;
                Q.KuzeyBati = null;
                Q.KuzeyDogu = null;
            }
            if (Q.GuneyBati != null)
            {
                DeleteNodes(ref Q.GuneyBati);
                Q.GuneyDogu = null;
                Q.GuneyBati = null;
                Q.KuzeyBati = null;
                Q.KuzeyDogu = null;
            }
            if (Q.GuneyDogu != null)
            {
                DeleteNodes(ref Q.GuneyDogu);
                Q.GuneyDogu = null;
                Q.GuneyBati = null;
                Q.KuzeyBati = null;
                Q.KuzeyDogu = null;
            }
        }
        
        /// <summary>
        /// Düğüme rastgele R,G,B renk değerleri üretir ve ArrayList'e ekler
        /// </summary>
        /// <param name="index">Düğümün renk index değerini tutar</param>
        private void ColorAddList(int index)
        {
            if (!(IndexId.Contains(index)))
            {
                IndexId.Add(index);
                R.Add(rand.Next(0, 255));
                G.Add(rand.Next(0, 255));
                B.Add(rand.Next(0, 255));
            }
        }
        
        /// <summary>
        /// R,G,B,IndexId ArrayList yapılarını temizler
        /// </summary>
        private void RGBClear()
        {
            R.Clear();
            G.Clear();
            B.Clear();
            IndexId.Clear();
        }
    }
}
