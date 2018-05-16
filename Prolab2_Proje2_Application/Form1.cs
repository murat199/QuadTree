using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prolab2_Proje2_Application
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
        }
        public QuadTree qtree;
        public QuadSearch qSearch;

        Graphics graphicForm;
        Pen pen = new Pen(Color.Black);
        SolidBrush sBrush = new SolidBrush(Color.Black);

        ArrayList ArrListSourceX = new ArrayList();
        ArrayList ArrListSourceY = new ArrayList();
        ArrayList ArrListSourceIndex = new ArrayList();

        ArrayList ArrListSearchX = new ArrayList();
        ArrayList ArrListSearchY = new ArrayList();
        ArrayList ArrListSearchIndex = new ArrayList();

        public int numSearchX = 0;
        public int numSearchY = 0;
        public int numSearchR = 0;

        int circleStartX = 0, circleStartY = 0, circleEndX = 0, circleEndY = 0;
        bool drawCircleOk=false;
        bool checkDrawCircle=false;
        
        private void DrawBorder()
        {
            pen.Width = 0;
            pen.Color = Color.Black;
            Rectangle init = new Rectangle(0, 0, 512, 512);
            graphicForm.DrawRectangle(pen, init);
        }
        /// <summary>
        /// Form ekranına parametre olarak aldığı düğümün başlangıcından bitişine yatay ve dikey olarak çizgi çizer
        /// </summary>
        /// <param name="q">yatay-dikey çizgileri çizilecek düğüm</param>
        private void DrawLine(QuadNode q)
        {
            pen.Width = 0;
            pen.Color = q.NodeColor;
            sBrush.Color = q.NodeColor;
            graphicForm.DrawLine(pen, q.OrX1, q.y, q.OrX2, q.y);
            graphicForm.DrawLine(pen, q.x, q.OrY1, q.x, q.OrY2);
            graphicForm.FillEllipse(sBrush, q.x - 5, q.y - 5, 10, 10);
        }
        /// <summary>
        /// Form ekranına merkez-x, merkez-y koordinatında r yarıçapında daire çizer
        /// </summary>
        /// <param name="x">merkez-x koordinatı</param>
        /// <param name="y">merkez-y koordinatı</param>
        /// <param name="r">yarıçap uzunluğu</param>
        private void DrawCircle(int x, int y, int r)
        {
            pen.Color = Color.Black;
            pen.Width = 3;
            if (x != 0 && y != 0 && r != 0)
            {
                graphicForm.DrawEllipse(pen, x - r, y - r, r * 2, r * 2);
                DrawCircleLine();
            }
        }
        /// <summary>
        /// Form ekranına eklenen 'Arama Dairesi'nin içinde kalan düğümlerin çizgilerini çizer
        /// </summary>
        private void DrawCircleLine()
        {
            for (int i = 0; i < qSearch.isEmptySearchList(); i++)
            {
                pen.Color = Color.Red;
                pen.Width = 0;
                sBrush.Color = Color.Red;
                graphicForm.DrawLine(pen, (int)qSearch.SearchListStartX[i], (int)qSearch.SearchListY[i], (int)qSearch.SearchListEndX[i], (int)qSearch.SearchListY[i]);
                graphicForm.DrawLine(pen, (int)qSearch.SearchListX[i], (int)qSearch.SearchListStartY[i], (int)qSearch.SearchListX[i], (int)qSearch.SearchListEndY[i]);
                graphicForm.FillEllipse(sBrush, (int)qSearch.SearchListX[i] - 5, (int)qSearch.SearchListY[i] - 5, 10, 10);
            }
        }
        /// <summary>
        /// Çizim alanını temizler
        /// </summary>
        private void DrawClean()
        {
            pen.Width = 0;
            graphicForm.Clear(Color.White);
        }
        /// <summary>
        /// Düğümleri yatay ve dikey çizgilerini çizer
        /// </summary>
        /// <param name="QN">Başlangıç düğüm parametresi</param>
        private void PanelDraw(QuadNode QN)
        {
            if (QN == qtree.root)
            {
                PanelDraw(QN.KuzeyDogu);
                DrawLine(QN);
            }
            if (QN != null)
            {
                if (QN.KuzeyDogu != null)
                {
                    PanelDraw(QN.KuzeyDogu);
                    DrawLine(QN.KuzeyDogu);
                }
                if (QN.KuzeyBati != null)
                {
                    PanelDraw(QN.KuzeyBati);
                    DrawLine(QN.KuzeyBati);
                }
                if (QN.GuneyDogu != null)
                {
                    PanelDraw(QN.GuneyDogu);
                    DrawLine(QN.GuneyDogu);
                }
                if (QN.GuneyBati != null)
                {
                    PanelDraw(QN.GuneyBati);
                    DrawLine(QN.GuneyBati);
                }
            }
        }
        private void ClearDrawCircle()
        {
            numSearchX = 0;
            numSearchY = 0;
            numSearchR = 0;
            ArrListSearchX.Clear();
            ArrListSearchY.Clear();
            ArrListSearchIndex.Clear();
            ArrListSearchIndex.Add(0);
            dataGridViewSearch.Rows.Clear();
            qSearch.ClearSearchList();
        }
        /// <summary>
        /// Aranılan dairenin içine düşen düğümleri datagridview nesnesine ekler
        /// </summary>
        private void AddRowDtSearch()
        {
            dataGridViewSearch.Rows.Clear();
            ArrListSearchIndex.Clear();
            ArrListSearchIndex.Add(0);
            for (int i = 0; i < qSearch.isEmptySearchList(); i++)
            {
                dataGridViewSearch.Rows.Add(ArrListSearchIndex.Count, qSearch.SearchListX[i], qSearch.SearchListY[i]);
                ArrListSearchIndex.Add(ArrListSearchIndex.Count);
            }
            this.dataGridViewSearch.Sort(this.dtGridViewSearchX, ListSortDirection.Ascending);
        }
        /// <summary>
        /// Aranılan dairenin içine düşen düğümleri datagridview nesnesinde yok ise ekler
        /// </summary>
        /// <param name="x">merkez-x koordinat</param>
        /// <param name="y">merkez-y koordinat</param>
        private void AddRowDtSearch(int x, int y)
        {
            if (!(ArrListSearchX.Contains(x)) && !(ArrListSearchY.Contains(y)))
            {
                ArrListSearchX.Add(x);
                ArrListSearchY.Add(y);
                dataGridViewSearch.Rows.Add(ArrListSearchIndex.Count, x, y);
                ArrListSearchIndex.Add(ArrListSearchIndex.Count);
            }
            this.dataGridViewSearch.Sort(this.dtGridViewSearchX, ListSortDirection.Ascending);
        }
        /// <summary>
        /// Eklenen düğümleri datagridview nesnesie ekler
        /// </summary>
        /// <param name="x">merkez-x koordinatı</param>
        /// <param name="y">merkez-y koordinatı</param>
        private void AddRowDtSource(int x, int y)
        {
            if (!(ArrListSourceX.Contains(x)) || !(ArrListSourceY.Contains(y)))
            {
                ArrListSourceX.Add(x);
                ArrListSourceY.Add(y);
                dataGridViewNodes.Rows.Add(ArrListSourceIndex.Count, x, y);
                ArrListSourceIndex.Add(ArrListSourceIndex.Count);
            }
            //this.dataGridViewNodes.Sort(this.dtGridViewNodeIndex, ListSortDirection.Descending);
            this.dataGridViewNodes.Sort(this.dtGridViewNodeX, ListSortDirection.Ascending);
        }
        /// <summary>
        /// Eklenen düğümlerin koordinatı ile daire içine düşen düğümlerin koordinatlarını tutan datagridview nesnesinin elemanlarını siler
        /// </summary>
        private void CleanDtGridView()
        {
            dataGridViewNodes.Rows.Clear();
            dataGridViewSearch.Rows.Clear();
            CleanArrList();
        }
        private void CleanDtGridViewSearch()
        {
            //numSearchR = 0;
            //numSearchX = 0;
            //numSearchY = 0;
            dataGridViewSearch.Rows.Clear();
            qSearch.ClearSearchList();
            ArrListSearchX.Clear();
            ArrListSearchY.Clear();
            ArrListSearchIndex.Clear();
            ArrListSearchIndex.Add(0);
        }
        /// <summary>
        /// Yapılan arama sorgusunda bilgileri tutan ArrayList yapılarının elemanlarını sıfırlar
        /// </summary>
        private void CleanArrList()
        {
            //numSearchX = 0;
            //numSearchY = 0;
            //numSearchR = 0;
            qSearch.ClearSearchList();
            ArrListSearchX.Clear();
            ArrListSearchY.Clear();
            ArrListSearchIndex.Clear();
            ArrListSearchIndex.Add(0);

            ArrListSourceX.Clear();
            ArrListSourceY.Clear();
            ArrListSourceIndex.Clear();
            ArrListSourceIndex.Add(0);
        }
        /// <summary>
        /// Formu yeniden yükler
        /// </summary>
        private void RefreshForm()
        {
            DrawClean();
            DrawBorder();
            PanelDraw(qtree.root);
            DrawCircle(numSearchX, numSearchY, numSearchR);
        }
        /// <summary>
        /// Yapılan arama sorgusunda forma eklenen dairenin koordinat ve yarıçapını ArrayList'e ekler
        /// </summary>
        /// <param name="Q">sorgu yapılacak düğüm</param>
        private void AddCircleLineList(QuadNode Q)
        {
            if (DistanceTwoPoints(Q.x, Q.y, numSearchX, numSearchY) < numSearchR)
            {
                if (!qSearch.SearchListX.Contains(Q.x) && !qSearch.SearchListY.Contains(Q.y))
                {
                    AddRowDtSearch(Q.x, Q.y);
                    qSearch.SearchListX.Add(Q.x);
                    qSearch.SearchListY.Add(Q.y);
                    qSearch.SearchListStartX.Add(Q.OrX1);
                    qSearch.SearchListStartY.Add(Q.OrY1);
                    qSearch.SearchListEndX.Add(Q.OrX2);
                    qSearch.SearchListEndY.Add(Q.OrY2);
                }
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
        /// Ağacın ana dğümünü çocuk düğümlerden koparır
        /// </summary>
        private void CleanQtreeRoot()
        {
            qtree.DeleteNodes(ref qtree.root);
            qtree.root = null;
            qtree.totalNode = 0;
        }
        private void FormHome_Paint(object sender, PaintEventArgs e)
        {
            graphicForm = this.CreateGraphics();
            DrawBorder();
        }
        private void FormHome_Load(object sender, EventArgs e)
        {
            qtree = new QuadTree();
            qSearch = new QuadSearch();
            ArrListSourceIndex.Add(0);
            ArrListSearchIndex.Add(0);
        }

        private void FormHome_Click(object sender, EventArgs e)
        {
            if (drawCircleOk == false)
            {
                var relativePoint = this.PointToClient(Cursor.Position);
                if (relativePoint.X > 0 && relativePoint.X < 512 && relativePoint.Y > 0 && relativePoint.Y < 512)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Cross;
                    this.Cursor = Cursors.Cross;
                    qtree.Add(relativePoint.X, relativePoint.Y);
                    AddRowDtSource(relativePoint.X, relativePoint.Y);
                    PanelDraw(qtree.root);
                    if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
                    {
                        AddCircleLineList(qtree.tempNode);
                        DrawCircle(numSearchX, numSearchY, numSearchR);
                    }
                }
            }
        }

        private void btnAddXYNode_Click(object sender, EventArgs e)
        {
            qtree.Add(Convert.ToInt32(numericTxtX.Value), Convert.ToInt32(numericTxtY.Value));
            AddRowDtSource(Convert.ToInt32(numericTxtX.Value), Convert.ToInt32(numericTxtY.Value));
            PanelDraw(qtree.root);
            if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
            {
                AddCircleLineList(qtree.tempNode);
                DrawCircle(numSearchX, numSearchY, numSearchR);
            }
        }

        private void btnSearchCircleNodes_Click(object sender, EventArgs e)
        {
            if (qtree.root == null)
            {
                MessageBox.Show("Henüz daire eklenmemiş...");
            }
            else
            {
                qSearch.ClearSearchList();
                if (qtree.root == null)
                {
                    MessageBox.Show("Henüz daire eklenmemiş...");
                }
                else
                {
                    int tempX = numSearchX;
                    int tempY = numSearchY;
                    int tempR = numSearchR;
                    numSearchX = Convert.ToInt32(numericTxtCircleX.Value);
                    numSearchY = Convert.ToInt32(numericTxtCircleY.Value);
                    numSearchR = Convert.ToInt32(numericTxtCircleR.Value);
                    if (tempX != numSearchX && tempY != numSearchY)
                    {
                        if (tempR == numSearchR)
                        {
                            MessageBox.Show("Değerleri değiştirip tekrar deneyin!");
                        }
                        else
                        {
                            qSearch.NodeSeacrh(qtree.root, numSearchX, numSearchY, numSearchR);
                            DrawCircle(numSearchX, numSearchY, numSearchR);
                            RefreshForm();
                        }
                        
                    }
                    else
                    {
                        qSearch.NodeSeacrh(qtree.root, numSearchX, numSearchY, numSearchR);
                        DrawCircle(numSearchX, numSearchY, numSearchR);
                        RefreshForm();
                    }
                }
                AddRowDtSearch();
            }
        }

        private void btnAddRandNode_Click(object sender, EventArgs e)
        {
            /*
            int rand_x, rand_y;
            Random rand = new Random();
            for (int i = 0; i < numericTxtRand.Value; i++)
            {
                rand_x = rand.Next(1, 512);
                rand_y = rand.Next(1, 512);
                qtree.Add(rand_x, rand_y);
                AddRowDtSource(rand_x, rand_y);
                PanelDraw(qtree.root);
                if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
                {
                    AddCircleLineList(qtree.tempNode);
                    DrawCircle(numSearchX, numSearchY, numSearchR);
                }
            }
             * */
            int rand_x, rand_y;
            Random rand = new Random();
            for (int i = 0; i < numericTxtRand.Value; i++)
            {
                rand_x = rand.Next(1, 512);
                rand_y = rand.Next(1, 512);
                qtree.Add(rand_x, rand_y);
                AddRowDtSource(rand_x, rand_y);
            }
            PanelDraw(qtree.root);
            if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
            {
                AddCircleLineList(qtree.tempNode);
                DrawCircle(numSearchX, numSearchY, numSearchR);
            }
        }

        private void btnResetNodes_Click(object sender, EventArgs e)
        {
            drawCircleOk = false;
            checkDrawCircle = false;
            CleanQtreeRoot();
            DrawClean();
            DrawBorder();
            CleanDtGridView();
            numSearchX = 0;
            numSearchY = 0;
            numSearchR = 0;
            ArrListSearchX.Clear();
            ArrListSearchY.Clear();
            ArrListSearchIndex.Clear();
            ArrListSearchIndex.Add(0);
            dataGridViewSearch.Rows.Clear();
        }

        private void btnResetCircle_Click(object sender, EventArgs e)
        {
            if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
            {
                ClearDrawCircle();
                RefreshForm();
            }
            else
            {
                MessageBox.Show("Daire ekli değil!");
            }
        }

        private void btnPageRefresh_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void btnGetTotalNodes_Click(object sender, EventArgs e)
        {
            if (qtree.root == null)
            {
                MessageBox.Show("Henüz düğüm eklenmedi!");
            }
            else
            {
                MessageBox.Show("Toplam düğüm sayısı : " + qtree.totalNode.ToString());
            }
        }

        private void btnGetCircleNodes_Click(object sender, EventArgs e)
        {
            if (qtree.root == null)
            {
                MessageBox.Show("Henüz düğüm eklenmedi!");
            }
            else
            {
                if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
                {
                    MessageBox.Show("Daire içine düşen düğüm sayısı : " + qSearch.isEmptySearchList().ToString());
                }
                else
                {
                    MessageBox.Show("Henüz daire eklenmedi!");
                }
            }
        }

        private void btnCircleLarging_Click(object sender, EventArgs e)
        {
            CleanDtGridViewSearch();
            if (qtree.root == null)
            {
                MessageBox.Show("Henüz daire eklenmemiş...");
            }
            else
            {
                if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
                {
                    numSearchR += Convert.ToInt32(numerictxtCircleLarging.Value);
                    qSearch.NodeSeacrh(qtree.root, numSearchX, numSearchY, numSearchR);
                    DrawCircle(numSearchX, numSearchY, numSearchR);
                    RefreshForm();
                    AddRowDtSearch();
                }
                else
                {
                    MessageBox.Show("Daire ekli değil!");
                }
            }
        }

        private void btnCircleShrink_Click(object sender, EventArgs e)
        {
            CleanDtGridViewSearch();
            if (qtree.root == null)
            {
                MessageBox.Show("Henüz daire eklenmemiş...");
            }
            else
            {
                if (numSearchX != 0 && numSearchY != 0 && numSearchR != 0)
                {
                    if (numSearchR * 2 >= 0)
                    {
                        numSearchR -= Convert.ToInt32(numerictxtCircleLarging.Value);
                        qSearch.NodeSeacrh(qtree.root, numSearchX, numSearchY, numSearchR);
                        DrawCircle(numSearchX, numSearchY, numSearchR);
                        RefreshForm();
                        AddRowDtSearch();
                    }
                    else
                    {
                        MessageBox.Show("Yarıçapı eksi değer yapılamaz!");
                    }
                }
                else
                {
                    MessageBox.Show("Daire ekli değil!");
                }
            }
        }

        private void circleOkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (qtree.root == null)
            {
                MessageBox.Show("Önce düğüm eklenmeli!");
                circleOkCheckBox.Checked = false;
            }
            else
            {
                if (circleOkCheckBox.Checked == true)
                {
                    drawCircleOk = true;

                }
                else
                {
                    drawCircleOk = false;
                    ClearDrawCircle();
                    DrawClean();
                    RefreshForm();

                }
            }
        }

        private void FormHome_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawCircleOk == true)
            {
                //bırakılan
                ClearDrawCircle();
                checkDrawCircle = false;
                numSearchX = Math.Min(circleStartX, circleEndX) + (Math.Max(circleStartX, circleEndX) - Math.Min(circleStartX, circleEndX)) / 2;
                numSearchY = Math.Min(circleStartY, circleEndY) + (Math.Max(circleStartX, circleEndX) - Math.Min(circleStartX, circleEndX)) / 2;
                numSearchR = (Math.Max(circleStartX, circleEndX) - Math.Min(circleStartX, circleEndX)) / 2;
                qSearch.NodeSeacrh(qtree.root, numSearchX, numSearchY, numSearchR);
                PanelDraw(qtree.root);
                DrawCircle(numSearchX, numSearchY, numSearchR);
                AddRowDtSearch();
            }
        }

        private void FormHome_MouseDown(object sender, MouseEventArgs e)
        {
            //basılan
            if (drawCircleOk == true)
            {
                var relativePoint = this.PointToClient(Cursor.Position);
                circleStartX = relativePoint.X;
                circleStartY = relativePoint.Y;
                checkDrawCircle = true;
            }
        }

        private void FormHome_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawCircleOk == true)
            {
                if (checkDrawCircle == true)
                {
                    var relativePoint = this.PointToClient(Cursor.Position);
                    circleEndX = relativePoint.X;
                    circleEndY = relativePoint.Y;
                    graphicForm.Clear(Color.White);
                    graphicForm.DrawEllipse(pen, Math.Min(circleStartX, circleEndX), Math.Min(circleStartY, circleEndY), (Math.Max(circleStartX, circleEndX) - Math.Min(circleStartX, circleEndX)), (Math.Max(circleStartX, circleEndX) - Math.Min(circleStartX, circleEndX)));
                    DrawBorder();
                    PanelDraw(qtree.root);   
                }
            }
        }
    }
}
