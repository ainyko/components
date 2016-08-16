using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TrafficInfoManagerUI
{
    public  class MyTabControl : System.Windows.Forms.TabControl
    {
        Image backImage;
        // 对父窗体的引用 
        Form oldman; 
        public MyTabControl()
        {

            base.SetStyle(ControlStyles.UserPaint |                      // 控件将自行绘制，而不是通过操作系统来绘制  
                     ControlStyles.OptimizedDoubleBuffer |          // 该控件首先在缓冲区中绘制，而不是直接绘制到屏幕上，这样可以减少闪烁  
                     ControlStyles.AllPaintingInWmPaint |           // 控件将忽略 WM_ERASEBKGND 窗口消息以减少闪烁  
                     ControlStyles.ResizeRedraw |                   // 在调整控件大小时重绘控件  
                     ControlStyles.SupportsTransparentBackColor,    // 控件接受 alpha 组件小于 255 的 BackColor 以模拟透明  
                     true);
            base.UpdateStyles();
            this.SizeMode = TabSizeMode.Fixed;  // 大小模式为固定  
            this.ItemSize = new Size(100, 100);   // 设定每个标签的尺寸  
            backImage = new Bitmap(this.GetType(), "TabBackground.bmp");// 从资源文件中获取背景图  需要将资源背景图改成嵌入资源
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < this.TabCount; i++)
            {
                //e.Graphics.DrawRectangle(Pens.Red, this.GetTabRect(i));//GetTabRect获得指定index的标签的矩形位置

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                if (this.SelectedIndex == i)
                {
                    e.Graphics.DrawImage(backImage, this.GetTabRect(i));
                }
                Rectangle bounds = this.GetTabRect(i);
                PointF textPoint = new PointF();
                SizeF textSize = TextRenderer.MeasureText(this.TabPages[i].Text, this.Font);

                // 注意要加上每个标签的左偏移量X  
                textPoint.X
                    = bounds.X + (bounds.Width - textSize.Width) / 2;
                textPoint.Y
                    = bounds.Bottom - textSize.Height - this.Padding.Y;

                // Draw highlights  
                e.Graphics.DrawString(
                    this.TabPages[i].Text,
                    this.Font,
                    SystemBrushes.ControlLightLight,    // 高光颜色  
                    textPoint.X,
                    textPoint.Y);

                // 绘制正常文字  
                textPoint.Y--;
                e.Graphics.DrawString(
                    this.TabPages[i].Text,
                    this.Font,
                    SystemBrushes.ControlText,    // 正常颜色  
                    textPoint.X,
                    textPoint.Y);

                if (this.ImageList != null)
                {
                    int index = this.TabPages[i].ImageIndex;
                    string key = this.TabPages[i].ImageKey;
                    Image icon = new Bitmap(1, 1);

                    if (index > -1)
                    {
                        icon = this.ImageList.Images[index];
                    }
                    if (!string.IsNullOrEmpty(key))
                    {
                        icon = this.ImageList.Images[key];
                    }
                    e.Graphics.DrawImage(
                        icon,
                        bounds.X + (bounds.Width - icon.Width) / 2,
                        bounds.Top + this.Padding.Y);
                }
                //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;  

            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            // 如果没有劫持到，则搜索  
            if (oldman == null)
                oldman = this.FindForm();

            oldman.Text = this.TabPages[0].Text;
        }

        protected override void OnSelected(TabControlEventArgs e)
        {

            Parent.Text = e.TabPage.Text;
        }


      
     
    }
}
