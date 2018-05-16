using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolab2_Proje2_Application
{
    /// <summary>
    /// Prolab2_Proje2_Application.QuadSearch sınıfı
    /// Aramaya eklenen daire içine düşen düğümleri tutar
    /// NodeSeacrh metodu içerir
    /// DistanceTwoPoint metodu içerir
    /// ClearSearchList metodu içerir
    /// isEmptySearchList metodu içerir
    /// </summary>
    public class QuadSearch
    {
        public ArrayList SearchListX = new ArrayList();
        public ArrayList SearchListY = new ArrayList();
        public ArrayList SearchListStartX = new ArrayList();
        public ArrayList SearchListStartY = new ArrayList();
        public ArrayList SearchListEndX = new ArrayList();
        public ArrayList SearchListEndY = new ArrayList();


        /// <summary>
        /// Daire içine düşen düğümleri bulur
        /// </summary>
        /// <param name="Q">Aramaya başlanacak düğümdür</param>
        /// <param name="x">Arama yapılacak dairenin merkez x koordinatı</param>
        /// <param name="y">Arama yapılacak dairenin merkez y koordinatı</param>
        /// <param name="r">Arama yapılacak dairenin yarıçap uzunluğu</param>
        public void NodeSeacrh(QuadNode Q, int x, int y, int r)
        {
            if (DistanceTwoPoints(Q.x, Q.y, x, y) < r)
            {
                SearchListX.Add(Q.x);
                SearchListY.Add(Q.y);
                SearchListStartX.Add(Q.OrX1);
                SearchListStartY.Add(Q.OrY1);
                SearchListEndX.Add(Q.OrX2);
                SearchListEndY.Add(Q.OrY2);
            }
            if (Q.KuzeyDogu != null)
            {
                NodeSeacrh(Q.KuzeyDogu, x, y, r);
            }
            if (Q.KuzeyBati != null)
            {
                NodeSeacrh(Q.KuzeyBati, x, y, r);
            }
            if (Q.GuneyBati != null)
            {
                NodeSeacrh(Q.GuneyBati, x, y, r);
            }
            if (Q.GuneyDogu != null)
            {
                NodeSeacrh(Q.GuneyDogu, x, y, r);
            }
        }
        /// <summary>
        /// (x1,y1) - (x2,y2) değerleri verilen iki nokta arası uzaklığı hesaplar
        /// </summary>
        /// <param name="x1">1.noktanın merkez x koordinatı</param>
        /// <param name="y1">1.noktanın merkez y koordinatı</param>
        /// <param name="x2">2.noktanın merkez x koordinatı</param>
        /// <param name="y2">2.noktanın merkez y koordinatı</param>
        /// <returns>int</returns>
        private int DistanceTwoPoints(int x1, int y1, int x2, int y2)
        {
            return (int)(Math.Sqrt(Math.Pow((y2 - y1), 2) + Math.Pow((x2 - x1), 2)));
        }
        /// <summary>
        /// Daire içine düşen düğüm bilgilerini tutan ArrayList yapılarını temizler
        /// </summary>
        public void ClearSearchList()
        {
            SearchListX.Clear();
            SearchListY.Clear();
            SearchListStartX.Clear();
            SearchListStartY.Clear();
            SearchListEndX.Clear();
            SearchListEndY.Clear();
        }
        /// <summary>
        /// Daire içine düşen düğüm sayısını dömdürür
        /// </summary>
        /// <returns>Metodun dönüş tipi int</returns>
        public int isEmptySearchList()
        {
            return SearchListX.Count;
        }
    }
}
