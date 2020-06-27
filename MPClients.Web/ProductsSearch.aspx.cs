using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MPClients.PageControllers;
using NHibernate;
using MPClients.DataAccess.NHibernate;
using MPClients.DataAccess.Domain;
using System.Collections.Generic;
using NHibernate.Expression;
using Telerik.Web.UI;
using MPClients.Common;
using System.Data.SqlClient;
using System.Linq;

namespace MPClients
{
    public partial class ProductsSearch : BasePage
    {
        private IList<VWProducts> allResults = new List<VWProducts>();
        private IList<String> currentDetailsId;
        bool searchOnlyCategories = true;
        protected override void PageLoad()
        {
            uxGrid.ClientSettings.Selecting.UseClientSelectColumnOnly = true;

            if (!this.IsPostBack)
            {
                LoadTireSizes();

                // tie-in for Enter key
                cmbSize1.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize2.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize3.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");

                cmbSize1_2.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize2_2.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize3_2.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");

                cmbSize1_3.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize2_3.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize3_3.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");

                cmbSize1_4.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize2_4.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");
                cmbSize3_4.Attributes.Add("onKeyPress", "doClick('" + uxSearchButton.ClientID + "',event)");

                if (!MetaSapiens.PageMethods.PageMethodsEngine.InvokeMethod(this, true))
                    throw new Exception("Page method not found!");
                SetShopCartPermissions();

            }
            else
            {

            }
            if (Request.QueryString["FromMainMenu"] == "1")
            {
                this.DoLoad();
            }
        }

        private void SetShopCartPermissions()
        {
            bool allowAddToSC = !System.Web.Security.Roles.IsUserInRole(Helper.SEARCH_ONLY_ROLE);
            uxGrid.MasterTableView.GetColumn("AddToChart").Visible = allowAddToSC;
            uxGrid.MasterTableView.GetColumn("QuantityDesired").Visible = allowAddToSC;

            uxViewShoppingCart.Visible = allowAddToSC;
            uxViewShoppingCart2.Visible = allowAddToSC;

        }

        private void MakeInvisiblePanels()
        {
            uiSearchCriteria.Visible = false;
            uxResultsMessages.Visible = false;
            uxResultsGrid.Visible = false;
        }

        protected override void OnInit(EventArgs e)
        {
            uxSearchButton.Click += new EventHandler(uxSearchButton_Click);
            uxSearchAgain.Click += new EventHandler(uxReviseSearchCriteria_Click);
            uxSearchAgain2.Click += new EventHandler(uxReviseSearchCriteria_Click);
            uxReviseSearchCriteria.Click += new EventHandler(uxReviseSearchCriteria_Click);
            uxGrid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(uxGrid_NeedDataSource);
            uxGrid.ItemCommand += new GridCommandEventHandler(uxGrid_ItemCommand);
            uxGrid.ItemDataBound += new GridItemEventHandler(uxGrid_ItemDataBound);
            uxGrid.ItemCreated += new GridItemEventHandler(uxGrid_ItemCreated);
            uxGrid.PageIndexChanged += new GridPageChangedEventHandler(uxGrid_PageIndexChanged);
            uxViewShoppingCart.Click += new EventHandler(uxViewShoppingCart_Click);
            uxViewShoppingCart2.Click += new EventHandler(uxViewShoppingCart_Click);
            uxClear.Click += new EventHandler(uxClear_Click);
            uxViewResults.Click += new EventHandler(uxViewResults_Click);
            uxAddSelectedTop.Click += new EventHandler(uxAddSelectedTop_Click);
            uxAddSelectedBottom.Click += new EventHandler(uxAddSelectedTop_Click);
            base.OnInit(e);
        }

        void uxAddSelectedTop_Click(object sender, EventArgs e)
        {
            List<GridDataItem> items = new List<GridDataItem>();
            foreach (GridDataItem item in uxGrid.Items)
            {
                if (item.Selected)
                {
                    items.Add(item);
                }
            }
            if (items.Count > 0)
            {
                ItemProcessing(items);
            }
        }

        void uxViewResults_Click(object sender, EventArgs e)
        {
            DisplayResults();
        }

        void uxClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductsSearch.aspx"); ;
        }

        void uxViewShoppingCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShoppingCart.aspx");
        }

        void uxGrid_ItemCommand(object source, GridCommandEventArgs e)
        {
           
            if (e.CommandName == "AddToChart")
            {
                ItemProcessing(new List<GridDataItem> { e.Item as GridDataItem });
            }
            else if (e.CommandName == "EditCommandColumn")
            {

            }
            else if (e.CommandName == "ToolTip")
            {
                HtmlInputControl control = e.Item.FindControl("productid") as HtmlInputControl;
                //UpdateToolTip(control.Value, this.RadToolTipManager1.UpdatePanel);
                MasterPage.DisplayMessage(control.Value);
            }
        }

        private void ItemProcessing(List<GridDataItem> items)
        {

            ISession session = UnitOfWork.Session;
            ITransaction transaction = session.BeginTransaction();
            List<string> productIds = new List<string>();
            Orders orders;
            orders = session.Get<Orders>(ShoppingCartId);

            if (orders == null)
            {
                MembershipUser SecurityMembershipUser = Membership.GetUser();
                string userID = SecurityMembershipUser.ProviderUserKey.ToString();

                string query = @"select orders.*
                        from Orders orders, MembershipUsers up
                        where orders.UserId = up.UserID and up.ClientID='" + UserClientID + "' ";

                if (!string.IsNullOrEmpty(Territory))
                {
                    query = @"select orders.*
                        from Orders orders, MembershipUsers up
                        where orders.UserId = up.UserID and orders.OnBehalfOf='" + UserClientID + "' and up.UserID='" + userID + "' ";
                }
                IQuery sqlQuery = UnitOfWork.GetIsolatedSession().
                    CreateSQLQuery( query, "Orders", typeof(MPClients.DataAccess.Domain.Orders));
                IList ordenes = sqlQuery.List();
                long countRows = ordenes.Count;

                orders = new Orders(ShoppingCartId);

                orders.OrderDate = DateTime.UtcNow.AddHours(Convert.ToDouble(MPClients.Web.Properties.Settings.Default.TimeZoneOffset));
                orders.OnBehalfOf = UserClientID;
                orders.Status = 2;  // Not Placed
                orders.StatusDate = DateTime.UtcNow.AddHours(Convert.ToDouble(MPClients.Web.Properties.Settings.Default.TimeZoneOffset));
                orders.UserId = new Guid(userID);

                // como los números de orden empiezan en 0, no hace falta el +1 para el nuevo número
                orders.OrderNo = UserClientID.Trim() + '-' + countRows.ToString();

                if (! string.IsNullOrEmpty(Territory))
                {
                    orders.OrderNo = "V" + orders.OrderNo;
                }
                orders.OrderNo = orders.OrderNo.Replace(" ", string.Empty).TrimEnd().TrimStart().Trim();
                session.Save(orders);


            }

            
            foreach (GridDataItem item in items)
            {
                GridDataItem dataBoundItem;
                dataBoundItem = item as GridDataItem;

                item.Selected = false;

                RadMaskedTextBox txtBox = item.FindControl("QuantityDesiredTextBox") as RadMaskedTextBox;
                HtmlInputControl control = item.FindControl("productid") as HtmlInputControl;
                HtmlInputControl uxClientProductPrice = item.FindControl("uxClientProductPrice") as HtmlInputControl;
                HtmlInputControl uxProductName = item.FindControl("uxProductName") as HtmlInputControl;
                HtmlInputControl uxDescription1 = item.FindControl("uxDescription1") as HtmlInputControl;

                int Quantity = Convert.ToInt32(txtBox.Text );


                //Buscamos si existen un produco en la orden
                ICriterion expression = Expression.And(
                    Expression.Eq("OrderID", ShoppingCartId)
                    , Expression.Eq("ProductID", control.Value));
                ICriteria criteria = session.CreateCriteria(typeof(MPClients.DataAccess.Domain.OrderDetail)).Add(expression);
                IList<MPClients.DataAccess.Domain.OrderDetail> carts
                    = criteria.List<MPClients.DataAccess.Domain.OrderDetail>();

                if (carts.Count > 0)
                {
                    uxGrid.StatusBarSettings.ReadyText = "Product was already in cart";
                    dataBoundItem["AddToChart"].Text = "Product was already in cart";

                }
                else
                {
                    MPClients.DataAccess.Domain.OrderDetail shoppingcart;

                    shoppingcart = new MPClients.DataAccess.Domain.OrderDetail(Guid.NewGuid());
                    shoppingcart.OrderID = ShoppingCartId;
                    shoppingcart.Quantity = Quantity;
                    shoppingcart.ProductID = control.Value;
                    shoppingcart.NetPrice = Convert.ToDecimal(uxClientProductPrice.Value);
                    shoppingcart.ProductName = uxProductName.Value;
                    shoppingcart.ShortDescription = uxDescription1.Value;
                    
                    uxGrid.StatusBarSettings.ReadyText = "Product Added";
                    dataBoundItem["AddToChart"].Text = "Product Added";

                    productIds.Add(shoppingcart.ProductID);

                  

                    //Obtenemos un listado de los productos friends que estan en la orden
                    session.Save(shoppingcart);
                }
            }

          
            transaction.Commit();
        }

        //----------------------------------------------------------------------------------

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!base.IsCallback)
            {
                if (uxResultsGrid.Visible && !this.Page.IsCallback)
                {
                    try
                    {
                        DisplayResults();
                    }
                    catch { }
                }
            }
        }

        void uxReviseSearchCriteria_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductsSearch.aspx");
        }

        [MetaSapiens.PageMethods.PageMethod(true)]
        public void DoLoad()
        {
            if (!Page.IsCallback)
            {
                MakeInvisiblePanels();
                uiSearchCriteria.Visible = true;

                MembershipUser membershipUser = Membership.GetUser();
                //string userID = membershipUser.ProviderUserKey.ToString();

                IList<MPClients.DataAccess.Domain.UserCategory> availableCategories =
                    Helper.GetUserCategories(membershipUser.ProviderUserKey.ToString());

                ArrayList userCategories = new ArrayList(availableCategories.Count);

                // hide tire search unless a tire category is visible
                uiTireSearchTable.Visible = false;
                foreach (MPClients.DataAccess.Domain.UserCategory var in availableCategories)
                {
                    userCategories.Add(var.Category);
                    if (Convert.ToInt32(var.Category) >= 29 && Convert.ToInt32(var.Category) <= 34)
                    {
                        uiTireSearchTable.Visible = true;
                    }

                    if (txtIncludedCategories.Value != "")
                    {
                        txtIncludedCategories.Value += ",";
                    }
                    txtIncludedCategories.Value += "'" + var.Category + "'";
                }

                ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(Category));
                criteria.Add(Expression.In("ID", (ICollection)userCategories));
                criteria.AddOrder(new Order("ID", true));

                IList<Category> categories = criteria.List<Category>();

                foreach (Category category in categories)
                {
                    RadTreeNode node = new RadTreeNode(category.ID + "- " + category.Description, category.ID);
                    node.Attributes["tag"] = "1";
                    node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                    uxCategories.Nodes.Add(node);                 
                }
            }
        }

        protected void RadTreeView_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {

            ICriterion expression = Expression.Eq("CategoryID", e.Node.Value);
            ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(SubCategory)).Add(expression);
            criteria.AddOrder(new Order("SubCategoryName", true));
            IList<SubCategory> categories = criteria.List<SubCategory>();

            if (e.Node.Nodes.Count == 0)
            {
                foreach (SubCategory category in categories)
                {
                    RadTreeNode node = new RadTreeNode(category.SubCategoryName, category.SubCategoryName);
                    node.ExpandMode = TreeNodeExpandMode.ClientSide;
                    e.Node.Nodes.Add(node);
                }
            }
        }

        void uxSearchButton_Click(object sender, EventArgs e)
        {
            MakeInvisiblePanels();

            string searchCriteria = GetSearchCriteria();

            if (searchCriteria != "")
            {
                uxResultsMessages.Visible = true;
                // TODO: poner esto en el web.config
                searchCriteria += " AND NOT Product_ID1 LIKE '999%'";

                ISession session = UnitOfWork.GetIsolatedSession();
                IQuery query = session.CreateQuery("SELECT COUNT(*) FROM Product WHERE " + searchCriteria);

                IList resuls = query.List();
                int countRows = Convert.ToInt32(resuls[0]);

                // save the query
                //ISession SearchQuerySession = UnitOfWork.GetIsolatedSession();
                //ITransaction tx = SearchQuerySession.BeginTransaction();

                //SearchQuery sq = new SearchQuery();
                //sq.QueryText = "SELECT * FROM Product WHERE " + searchCriteria;
                //sq.UserId = new Guid(base.UserID);
                //sq.QueryDateTime = DateTime.Now;
                //sq.ResultsCount = countRows;
                //SearchQuerySession.Save(sq);
                //tx.Commit();

                MembershipUsers membershipUser = session.Get<MembershipUsers>(new Guid(base.UserID));
                if (membershipUser != null)
                {
                    ITransaction transaction = session.BeginTransaction();
                    // TODO: guardar el query y la fecha
                    membershipUser.PriceQuerys += 1;
                    // TODO: poner esto en el web.config
                    membershipUser.LastPriceQueryDate = DateTime.UtcNow.AddHours(Convert.ToDouble(MPClients.Web.Properties.Settings.Default.TimeZoneOffset));

                    session.Save(membershipUser);

                    transaction.Commit();

                }
                if (countRows == 0)
                {
                    uxResultsLabel.Text = "Your search did not return any results.  Please review your search criteria and try again.";
                    uxViewResults.Visible = false;
                }
                else if (countRows >= MPClients.Web.Properties.Settings.Default.TOPResults1
                    && countRows < MPClients.Web.Properties.Settings.Default.TOPResults2)
                {
                    uxResultsLabel.Text = String.Format("Your search returned more than {0} results.  Do you wish to revise your search criteria or view all your results?", countRows);
                    uxViewResults.Visible = true;
                }
                else if (countRows > MPClients.Web.Properties.Settings.Default.TOPResults2)
                {
                    uxResultsLabel.Text = "Your search returned too many results.  Please revise your search criteria and search again.";
                    uxViewResults.Visible = false;
                }
                else
                {
                    DisplayResults();
                }

            }
            else
            {
                uxResultsMessages.Visible = true;
                uxResultsLabel.Text = "Your search did not return any results.  Please review your search criteria and try again.";
                uxViewResults.Visible = false;
            }

        }

        private void DisplayResults()
        {
            MakeInvisiblePanels();
            uxResultsGrid.Visible = true;

            AddResultToProduct(TextBox1.Text, false);
            AddResultToProduct(TextBox2.Text, false);
            AddResultToProduct(TextBox3.Text, false);
            AddResultToProduct(TextBox4.Text, false);
            AddResultToProduct(TextBox5.Text, false);
            AddResultToProduct(TextBox6.Text, false);
            AddResultToProduct(TextBox7.Text, false);
            AddResultToProduct(TextBox8.Text, false);
            AddResultToProduct(TextBox9.Text, false);
            AddResultToProduct(TextBox10.Text, false);

            string strNoSelection = "(Select)";
            // si no seleccionó tamaño, remover el texto default
            string strCombinedSizes = ((cmbSize1.SelectedValue == strNoSelection) ? string.Empty : cmbSize1.SelectedValue)
                + ((cmbSize2.SelectedValue == strNoSelection) ? string.Empty : cmbSize2.SelectedValue)
                + ((cmbSize3.SelectedValue == strNoSelection) ? string.Empty : cmbSize3.SelectedValue);
            AddResultToProduct(strCombinedSizes, true);

            // second set of combo boxes
            string strCombinedSizes2 = ((cmbSize1_2.SelectedValue == strNoSelection) ? string.Empty : cmbSize1_2.SelectedValue)
                + ((cmbSize2_2.SelectedValue == strNoSelection) ? string.Empty : cmbSize2_2.SelectedValue)
                + ((cmbSize3_2.SelectedValue == strNoSelection) ? string.Empty : cmbSize3_2.SelectedValue);
            AddResultToProduct(strCombinedSizes2, true);

            // third set of combo boxes
            string strCombinedSizes3 = ((cmbSize1_3.SelectedValue == strNoSelection) ? string.Empty : cmbSize1_3.SelectedValue)
                + ((cmbSize2_3.SelectedValue == strNoSelection) ? string.Empty : cmbSize2_3.SelectedValue)
                + ((cmbSize3_3.SelectedValue == strNoSelection) ? string.Empty : cmbSize3_3.SelectedValue);
            AddResultToProduct(strCombinedSizes3, true);

            // fourth set of combo boxes
            string strCombinedSizes4 = ((cmbSize1_4.SelectedValue == strNoSelection) ? string.Empty : cmbSize1_4.SelectedValue)
                + ((cmbSize2_4.SelectedValue == strNoSelection) ? string.Empty : cmbSize2_4.SelectedValue)
                + ((cmbSize3_4.SelectedValue == strNoSelection) ? string.Empty : cmbSize3_4.SelectedValue);
            AddResultToProduct(strCombinedSizes4, true);

            //AddCategoryResultToProduct()
            IList<RadTreeNode> checkedNodes = uxCategories.CheckedNodes;

            //string selectStm = "FROM VWProducts WHERE " + GetSearchCriteria() + " AND NOT Product_ID1  LIKE '999%' AND Client_ID = '" + UserClientID + "'";
            //IQuery query = UnitOfWork.GetIsolatedSession().CreateQuery(selectStm);
            foreach (var result in allResults)
            {
                result.QuantityDesired = result.QuantityDesired == 0 ? 1 : result.QuantityDesired;
                result.DescriptionExt = string.Format("{0}<br/>{1}<br/>Category: {2}-{3}", 
                    result.Name.Trim(), 
                    result.DescriptionExt.Trim(), 
                    result.ProductCategory.Trim(),
                    result.CategoryDescription.Trim()).Replace("<br/><br/>", "<br/>"); ;
            }
     
            string resultSearch = string.Empty;
            string productsQry = TextBox1.Text != "" ? TextBox1.Text + "," : "";
            productsQry += TextBox2.Text != "" ? TextBox2.Text + "," : "";
            productsQry += TextBox3.Text != "" ? TextBox3.Text + "," : "";
            productsQry += TextBox4.Text != "" ? TextBox4.Text + "," : "";
            productsQry += TextBox5.Text != "" ? TextBox5.Text + "," : "";
            productsQry += TextBox6.Text != "" ? TextBox6.Text + "," : "";
            productsQry += TextBox7.Text != "" ? TextBox7.Text + "," : "";
            productsQry += TextBox8.Text != "" ? TextBox8.Text + "," : "";
            productsQry += TextBox9.Text != "" ? TextBox9.Text + "," : "";
            productsQry += TextBox10.Text != "" ? TextBox10.Text + "," : "";
            productsQry += strCombinedSizes != "" ? strCombinedSizes + "," : "";
            productsQry += strCombinedSizes2 != "" ? strCombinedSizes2 + "," : "";
            productsQry += strCombinedSizes3 != "" ? strCombinedSizes3 + "," : "";
            productsQry += strCombinedSizes4 != "" ? strCombinedSizes4 + "," : "";

            if (productsQry != "")
            {
                if (productsQry.EndsWith(","))
                {
                    resultSearch += " " + productsQry.Substring(0, productsQry.Length - 1);

                }
                else
                {
                    resultSearch += " " + productsQry;

                }
                resultSearch += " <br>";
            }

            if (checkedNodes.Count > 0)
            {
                if (resultSearch != string.Empty)
                {
                    resultSearch = "You restricted your search to products in: ";
                }
                else
                {
                    resultSearch = "You searched for all products in: ";
                }
                int i = 0;
                foreach (RadTreeNode node in checkedNodes)
                {
                    if (i != 0)
                    {
                        resultSearch += " and ";
                    }
                    if (node.ParentNode == null)
                    {
                        resultSearch += "<span style='color:rgb(83,69,120); font-weight:bold'>" + node.Text + "</span>";
                    }
                    else
                    {
                        resultSearch += "<span style='color:rgb(83,69,120); font-weight:bold'>" + node.Text + "</span> (under " + node.ParentNode.Text + ")";
                    }
                    i += 1;
                }
            }
            else
            {
                resultSearch = string.Empty;
            }
            if (resultSearch != string.Empty)
            {
                uxResultsLabel1.Text = resultSearch + ".";
            }
            uiTotal.Value = UserClientID;
            uxGrid.DataSource = allResults;
            uxGrid.DataBind();
        }

        private string GetSearchCriteria()
        {
            string select = string.Empty;
            bool displayTextSearch = false;

            string strNoSelection = "(Select)";
            // si no seleccionó tamaño, remover el texto default
            string strCombinedSizes = ((cmbSize1.SelectedValue == strNoSelection) ? string.Empty : cmbSize1.SelectedValue)
                + ((cmbSize2.SelectedValue == strNoSelection) ? string.Empty : cmbSize2.SelectedValue)
                + ((cmbSize3.SelectedValue == strNoSelection) ? string.Empty : cmbSize3.SelectedValue);
            strCombinedSizes = Helper.NormalizeText(strCombinedSizes);

            // second set of combo boxes
            string strCombinedSizes2 = ((cmbSize1_2.SelectedValue == strNoSelection) ? string.Empty : cmbSize1_2.SelectedValue)
                + ((cmbSize2_2.SelectedValue == strNoSelection) ? string.Empty : cmbSize2_2.SelectedValue)
                + ((cmbSize3_2.SelectedValue == strNoSelection) ? string.Empty : cmbSize3_2.SelectedValue);
            strCombinedSizes2 = Helper.NormalizeText(strCombinedSizes2);

            // third set of combo boxes
            string strCombinedSizes3 = ((cmbSize1_3.SelectedValue == strNoSelection) ? string.Empty : cmbSize1_3.SelectedValue)
                + ((cmbSize2_3.SelectedValue == strNoSelection) ? string.Empty : cmbSize2_3.SelectedValue)
                + ((cmbSize3_3.SelectedValue == strNoSelection) ? string.Empty : cmbSize3_3.SelectedValue);
            strCombinedSizes3 = Helper.NormalizeText(strCombinedSizes3);

            // fourth set of combo boxes
            string strCombinedSizes4 = ((cmbSize1_4.SelectedValue == strNoSelection) ? string.Empty : cmbSize1_4.SelectedValue)
                + ((cmbSize2_4.SelectedValue == strNoSelection) ? string.Empty : cmbSize2_4.SelectedValue)
                + ((cmbSize3_4.SelectedValue == strNoSelection) ? string.Empty : cmbSize3_4.SelectedValue);
            strCombinedSizes4 = Helper.NormalizeText(strCombinedSizes4);

            string productsQry = string.Empty;
            productsQry += Helper.NormalizeText(TextBox1.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox2.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox3.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox4.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox5.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox6.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox7.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox8.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox9.Text) + ",";
            productsQry += Helper.NormalizeText(TextBox10.Text) + ",";

            string strSearchWhereClause = MPClients.Web.Properties.Settings.Default.SearchWhereClause;

            if (productsQry.Trim() != "")
            {
                string[] spliteWhere = productsQry.Split(',');
                for (int i = 0; i < spliteWhere.Length; i++)
                {
                    if (spliteWhere[i] != "")
                    {
                        if (i != 0)
                        {
                            select += " OR ";
                        }
                        //select += String.Format(" Product_ID1 LIKE '%{0}%' OR NameClean LIKE '%{0}%' OR Description1Clean LIKE '%{0}%' OR DescriptionExtClean LIKE '%{0}%' ", spliteWhere[i]);
                        select += String.Format(strSearchWhereClause, spliteWhere[i]);
                        
                        displayTextSearch = true;
                    }
                }

                if (displayTextSearch)
                {
                    select = " ( " + select + " ) ";
                }
            }

            IList<RadTreeNode> checkedNodes = uxCategories.CheckedNodes;

            if (checkedNodes.Count > 0)
            {
                if (!displayTextSearch)
                    select = " (";
                else
                    select += " AND (";

                int i = 0;
                foreach (RadTreeNode node in checkedNodes)
                {
                    //si el nodo es parent entonces buscar en categorias...
                    if (i != 0)
                    {
                        select += " OR ";
                    }
                    if (node.Attributes["tag"] != null)
                    {
                        select += String.Format(" ProductCategory = '{0}' ", node.Value);
                    }
                    else
                    {
                        RadTreeNode parent = node.Parent as RadTreeNode;
                        select += String.Format(" (ProductCategory = '{0}' AND ", parent.Value);
                        select += String.Format(" Description1 = '{0}' )", node.Value);
                    }

                    i += 1;
                }
                select += ")";
            }

            if (select.Length > 0)
            {
                if (txtIncludedCategories.Value != "")
                {
                    select = select == "" ? "" : select += " AND ";
                    select += String.Format(" ProductCategory IN ({0}) ", txtIncludedCategories.Value);
                }
                else
                {
                    select = select == "" ? "" : select += " AND ";
                    select += " AND ProductCategory IS NULL ";
                }
            }
            
            // las gomas no conllevan filtro de categoría
            if (strCombinedSizes.Length > 0)
            {
                if (select.Length > 0)
                {
                    select = "(" + select + ") OR (" + String.Format(strSearchWhereClause, strCombinedSizes) + ")";
                }
                else
                {
                    select = "(" + String.Format(strSearchWhereClause, strCombinedSizes) + ")";
                }
            }

            if (strCombinedSizes2.Length > 0)
            {
                if (select.Length > 0)
                {
                    select = "(" + select + ") OR (" + String.Format(strSearchWhereClause, strCombinedSizes2) + ")";
                }
                else
                {
                    select = "(" + String.Format(strSearchWhereClause, strCombinedSizes2) + ")";
                }
            }

            if (strCombinedSizes3.Length > 0)
            {
                if (select.Length > 0)
                {
                    select = "(" + select + ") OR (" + String.Format(strSearchWhereClause, strCombinedSizes3) + ")";
                }
                else
                {
                    select = "(" + String.Format(strSearchWhereClause, strCombinedSizes3) + ")";
                }
            }

            if (strCombinedSizes4.Length > 0)
            {
                if (select.Length > 0)
                {
                    select = "(" + select + ") OR (" + String.Format(strSearchWhereClause, strCombinedSizes4) + ")";
                }
                else
                {
                    select = "(" + String.Format(strSearchWhereClause, strCombinedSizes4) + ")";
                }
            }

            return select;
        }

        private void AddResultToProduct(string condition, bool IsTireCondition)
        {
            IList<RadTreeNode> checkedNodes = uxCategories.CheckedNodes;
            ICriterion expression = null;
            string encodeCondition = string.Empty;

            // las gomas no se filtran por categoría
            if (condition != "" || (searchOnlyCategories && uxCategories.CheckedNodes.Count > 0 && !IsTireCondition))
            {
                if (condition != "")
                {
                    encodeCondition = Helper.NormalizeText(condition);

                    expression =
                        Expression.Or(
                            Expression.Like("Product_ID1", encodeCondition, MatchMode.Anywhere)
                            , Expression.Like("NameClean", encodeCondition, MatchMode.Anywhere));

                    expression = Expression.Or(expression, Expression.Like("DescriptionExtClean", encodeCondition, MatchMode.Anywhere));
                    expression = Expression.Or(expression, Expression.Like("Description1Clean", encodeCondition, MatchMode.Anywhere));
                    
                }

                if (expression == null)
                {
                    expression = Expression.Eq("ClientID", UserClientID);
                }
                else
                {
                    expression = Expression.And(expression, Expression.Eq("ClientID", UserClientID));
                }
                
                // TODO: sacar esto del web.config
                expression = Expression.And(expression, Expression.Not(Expression.Like("ID", "999", MatchMode.Start)));

                ICriterion categoryCriteria = null;
                if (checkedNodes.Count > 0 && !IsTireCondition)
                {
                    foreach (RadTreeNode node in checkedNodes)
                    {
                        if (node.Attributes["tag"] != null)
                        {
                            if (categoryCriteria == null)
                                categoryCriteria = Expression.Eq("ProductCategory", node.Value);
                            else
                                categoryCriteria = Expression.Or(categoryCriteria, Expression.Eq("ProductCategory", node.Value));
                        }
                        else
                        {
                            RadTreeNode parentNode = node.Parent as RadTreeNode;
                            if (categoryCriteria == null)
                                categoryCriteria = Expression.And(Expression.Eq("ProductCategory", parentNode.Value), Expression.Eq("Description1", node.Value));
                            else
                                categoryCriteria = Expression.Or(categoryCriteria, Expression.And(Expression.Eq("ProductCategory", parentNode.Value), Expression.Eq("Description1", node.Value)));
                        }
                    }
                }

                if (categoryCriteria != null)
                {
                    expression = Expression.And(expression, categoryCriteria);
                }

                if (txtIncludedCategories.Value != "")
                {
                    ICollection includedCategories = txtIncludedCategories.Value.Replace("'", "").Split(',') as ICollection;
                    expression = Expression.And(expression, Expression.In("ProductCategory", includedCategories));
                }
                else
                {
                    expression = Expression.And(expression, Expression.Eq("ProductCategory", "00000"));
                }
                
                ICriteria criteria = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(VWProducts)).Add(expression);
                criteria.AddOrder(new Order("Description1", true));
                criteria.AddOrder(new Order("ID", true));
                IList<VWProducts> results = criteria.List<VWProducts>();

                foreach (VWProducts product in results)
                {
                    product.FromQuery = encodeCondition;
                    allResults.Add(product);
                }
                searchOnlyCategories = false;
            }
        }

        protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
        {
            this.runTooltip = true;
            this.UpdateToolTip(args.Value, args.UpdatePanel);
        }

        private void UpdateToolTip(string value, UpdatePanel panel)
        {
            Control ctrl = Page.LoadControl("ProductDetails.ascx");
            panel.ContentTemplateContainer.Controls.Add(ctrl);
            MPClients.Web.ProductDetails details = (MPClients.Web.ProductDetails)ctrl;
            details.LoadData(value);
        }

        void uxGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            GridDataItem dataBoundItem = e.Item as GridDataItem;
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.CommandItem)
            {
                Control target = e.Item.FindControl("targetControl");
                if (!Object.Equals(target, null))
                {
                    if (!Object.Equals(this.RadToolTipManager1, null))
                    {
                        VWProducts currentRow = (VWProducts)e.Item.DataItem;
                        //Add the button (target) id to the tooltip manager
                        string value = currentRow.Description3 + ";" + currentRow.DescriptionExt + ";" + currentRow.ProductCategory + "-" + currentRow.CategoryDescription;
                        this.RadToolTipManager1.TargetControls.Add(target.ClientID, value, true);
                    }
                }
                //RadNumericTextBox qtyControl = e.Item.FindControl("QuantityDesiredTextBox") as RadNumericTextBox;
                //Control commandButton = dataBoundItem["AddToChart"].Controls[0];
                //if (!Object.Equals(qtyControl, null) && !Object.Equals(commandButton, null))
                //{
                //    qtyControl.EmptyMessage = commandButton.ClientID;
                //}
                //QuantityDesiredTextBox
                //GridButtonColumn
            }
        }

        void uxGrid_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            RadToolTipManager1.TargetControls.Clear();
        }

        void uxGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            e.Item.PreRender += new EventHandler(Item_PreRender);
        }

        bool runTooltip = false;

        void Item_PreRender(object sender, EventArgs e)
        {
            if (!runTooltip)
            {
                if (currentDetailsId == null)
                {
                    currentDetailsId = new List<String>();

                    ICriterion expression1 = Expression.Eq("OrderID", ShoppingCartId);
                    ICriteria criteria1 = UnitOfWork.GetIsolatedSession().CreateCriteria(typeof(MPClients.DataAccess.Domain.OrderDetail)).Add(expression1);
                    IList<MPClients.DataAccess.Domain.OrderDetail> currentDetails
                        = criteria1.List<MPClients.DataAccess.Domain.OrderDetail>();

                    foreach (MPClients.DataAccess.Domain.OrderDetail var in currentDetails)
                    {
                        currentDetailsId.Add(var.ProductID);
                    }
                }

                if (sender is GridDataItem)
                {
                    GridDataItem dataItem = sender as GridDataItem;
                    string id = dataItem["ID"].Text;

                    Control commandButton = null;
                    if (dataItem["AddToChart"].Controls.Count > 0)
                    {
                        commandButton = dataItem["AddToChart"].Controls[0];
                    }

                    RadMaskedTextBox qtyControl = dataItem.FindControl("QuantityDesiredTextBox") as RadMaskedTextBox;

                    if (currentDetailsId.IndexOf(id) == -1)
                    {
                        if (!Object.Equals(qtyControl, null) && !Object.Equals(commandButton, null))
                        {
                            qtyControl.EmptyMessage = commandButton.ClientID   ;
                        }
                    }
                    else
                    {
                        //ctl.Text = "Product was already in cart";
                        dataItem["AddToChart"].Text = "In cart";
                        dataItem["CheckboxSelectColumn"].Enabled  = false;
                        if (!Object.Equals(qtyControl, null))
                        {
                            qtyControl.EmptyMessage = "";
                        }
                    }

                }
            }
        }

        #region Tire Sizes
        private class TireSize
        {
            private String size1;
            private String size2;
            private String size3;

            public TireSize(String size1, String size2, String size3)
            {
                this.size1 = size1;
                this.size2 = size2;
                this.size3 = size3;
            }

            public String Size1
            {
                get { return size1; }
            }

            public String Size2
            {
                get { return size2; }
            }

            public String Size3
            {
                get { return size3; }
            }
        }

        private void LoadTireSizes()
        {
            List<TireSize> tireSizes = GetTireSizes();
            var dataForSize1 = tireSizes.GroupBy(r => r.Size1).Select(y => y.First()).ToArray();

            cmbSize1.Items.Add("(Select)");
            cmbSize1_2.Items.Add("(Select)");
            cmbSize1_3.Items.Add("(Select)");
            cmbSize1_4.Items.Add("(Select)");

            foreach (var tireSize in dataForSize1)
            {
                cmbSize1.Items.Add(new ListItem(tireSize.Size1, tireSize.Size1));
                cmbSize1_2.Items.Add(new ListItem(tireSize.Size1, tireSize.Size1));
                cmbSize1_3.Items.Add(new ListItem(tireSize.Size1, tireSize.Size1));
                cmbSize1_4.Items.Add(new ListItem(tireSize.Size1, tireSize.Size1));
            }
        }

        private List<TireSize> GetTireSizes()
        {
            const int intSize1Index = 0;
            const int intSize2Index = 1;
            const int intSize3Index = 2;

            List<TireSize> tireSizes = new List<TireSize>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("spGetTireSizes", conn);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //if (!reader.IsDBNull(intSize1Index) && !reader.IsDBNull(intSize2Index) && !reader.IsDBNull(intSize3Index))
                        //{
                        tireSizes.Add(new TireSize(reader.GetValue(intSize1Index).ToString(), reader.GetValue(intSize2Index).ToString(), reader.GetValue(intSize3Index).ToString()));
                        //}
                    }
                    reader.Close();
                }

                conn.Close();
            }
            return tireSizes;
        }

        protected void cmbSize1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSize2.Items.Clear();
            cmbSize3.Items.Clear();
            //cmbSize3.Items.Add("(Seleccionar)");

            if (!string.IsNullOrEmpty(cmbSize1.SelectedValue))
            {
                List<TireSize> tireSizes = GetTireSizes();
                tireSizes = tireSizes.Where(r => r.Size1 == cmbSize1.SelectedValue && r.Size2.Length > 0).ToList();

                var dataForSize2 = tireSizes.GroupBy(r => r.Size2).Select(y => y.First()).ToArray();
                if (Convert.ToInt32(dataForSize2.Count().ToString()) == 0)
                {
                    cmbSize2.Enabled = false;
                    cmbSize3.Enabled = false;
                }
                else
                {
                    cmbSize2.Enabled = true;
                    cmbSize2.Items.Add("(Select)");
                    foreach (var tireSize in dataForSize2)
                    {
                        cmbSize2.Items.Add(new ListItem(tireSize.Size2, tireSize.Size2));
                    }
                }
            }
            cmbSize2.Focus();
        }

        protected void Size1IndexChange(DropDownList c1, DropDownList c2, DropDownList c3)
        {
            c2.Items.Clear();
            c3.Items.Clear();

            if (!string.IsNullOrEmpty(c1.SelectedValue))
            {
                List<TireSize> tireSizes = GetTireSizes();
                tireSizes = tireSizes.Where(r => r.Size1 == c1.SelectedValue && r.Size2.Length > 0).ToList();

                var dataForSize2 = tireSizes.GroupBy(r => r.Size2).Select(y => y.First()).ToArray();
                if (Convert.ToInt32(dataForSize2.Count().ToString()) == 0)
                {
                    c2.Enabled = false;
                    c3.Enabled = false;
                }
                else
                {
                    c2.Enabled = true;
                    c2.Items.Add("(Select)");
                    foreach (var tireSize in dataForSize2)
                    {
                        c2.Items.Add(new ListItem(tireSize.Size2, tireSize.Size2));
                    }
                }
            }
            c2.Focus();
        }

        protected void cmbSize1_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Size1IndexChange(cmbSize1_2, cmbSize2_2, cmbSize3_2);
        }

        protected void cmbSize1_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Size1IndexChange(cmbSize1_3, cmbSize2_3, cmbSize3_3);
        }

        protected void cmbSize1_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Size1IndexChange(cmbSize1_4, cmbSize2_4, cmbSize3_4);
        }

        protected void Size2IndexChange(DropDownList c1, DropDownList c2, DropDownList c3)
        {
            c3.Items.Clear();

            if (!string.IsNullOrEmpty(c2.SelectedValue))
            {
                List<TireSize> tireSizes = GetTireSizes();
                tireSizes = tireSizes.Where(r => r.Size1 == c1.SelectedValue && r.Size2 == c2.SelectedValue && r.Size3.Length > 0).ToList();

                var dataForSize3 = tireSizes.GroupBy(r => r.Size3).Select(y => y.First()).ToArray();
                if (Convert.ToInt32(dataForSize3.Count().ToString()) == 0)
                    c3.Enabled = false;
                else
                {
                    c3.Enabled = true;
                    c3.Items.Add("(Select)");
                    foreach (var tireSize in dataForSize3)
                    {
                        c3.Items.Add(new ListItem(tireSize.Size3, tireSize.Size3));
                    }
                }
            }
            c3.Focus();
        }

        protected void cmbSize2_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Size2IndexChange(cmbSize1_2, cmbSize2_2, cmbSize3_2);
        }

        protected void cmbSize2_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Size2IndexChange(cmbSize1_3, cmbSize2_3, cmbSize3_3);
        }

        protected void cmbSize2_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Size2IndexChange(cmbSize1_4, cmbSize2_4, cmbSize3_4);
        }

        protected void cmbSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSize3.Items.Clear();

            if (!string.IsNullOrEmpty(cmbSize2.SelectedValue))
            {
                List<TireSize> tireSizes = GetTireSizes();
                tireSizes = tireSizes.Where(r => r.Size1 == cmbSize1.SelectedValue && r.Size2 == cmbSize2.SelectedValue && r.Size3.Length > 0).ToList();

                var dataForSize3 = tireSizes.GroupBy(r => r.Size3).Select(y => y.First()).ToArray();
                if (Convert.ToInt32(dataForSize3.Count().ToString()) == 0)
                    cmbSize3.Enabled = false;
                else
                {
                    cmbSize3.Enabled = true;
                    cmbSize3.Items.Add("(Select)");
                    foreach (var tireSize in dataForSize3)
                    {
                        cmbSize3.Items.Add(new ListItem(tireSize.Size3, tireSize.Size3));
                    }
                }
            }
            cmbSize3.Focus();
        }
        #endregion
    }
}
