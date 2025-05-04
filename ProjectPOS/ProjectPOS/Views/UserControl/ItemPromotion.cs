using ProjectPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Views
{  
    public partial class ItemPromotion : UserControl
    {
        private PromotionModel _promotion = new PromotionModel();
        public ItemPromotion()
        {
            InitializeComponent();
        }
        public void Init(PromotionModel promotion)
        {
            if (promotion != null)
            {
                _promotion = promotion;
                txtID.Text = promotion.PromotionID.ToString();
                txtTitle.Text = promotion.PromotionName;
                txtDiscountType.Text = promotion.DiscountType;
                txtStartDate.Text = promotion.StartDate.ToString("dd/MM/yyyy");
                txtEndDate.Text = promotion.EndDate.ToString("dd/MM/yyyy");
            }
        }
    }
}
